using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Network.API.Infrastructure.Authentication;
using Network.Core.Models;

namespace Network.API.Infrastructure.Hubs
{    
    //[Authorize]
    public class NotificationHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private readonly DomainDbContext _dbContext;        

        public NotificationHub(DomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task SendVerifyRegisteredUsers(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var who = "admin";
                var notis = await GetNotis(who);                
                foreach (var connectionId in _connections.GetConnections(who))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveRegisteredUsers", JsonConvert.SerializeObject(notis));
                }
            }
        }
        public async Task<List<NotificationView>> GetNotis(string receiver)
        {
            return await _dbContext.Sys_Notifications.Where(o => o.Receiver == receiver).Select(o => new NotificationView 
            { Type = o.Type, Title = o.Title, Content = o.Content, CreatedDateTime = o.CreatedDateTime }).OrderBy(o => o.CreatedDateTime).ToListAsync();
        }
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var token = httpContext.Request.Query["access_token"];            
            string name = GetUserNameFromToken(token);
            if(!string.IsNullOrEmpty(name))
            {
                _connections.Add(name, Context.ConnectionId);
            }                
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception ex)
        {
            var httpContext = Context.GetHttpContext();
            var token = httpContext.Request.Query["access_token"];
            string name = GetUserNameFromToken(token);
            if (!string.IsNullOrEmpty(name))
            {
                _connections.Remove(name, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(ex);
        }
        private string GetUserNameFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var claimsIdentity = handler.ReadJwtToken(token).Payload;
                return claimsIdentity.First(o => o.Key == ClaimTypes.Name).Value.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
