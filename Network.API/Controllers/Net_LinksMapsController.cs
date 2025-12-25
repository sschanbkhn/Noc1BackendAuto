using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Network.API.Infrastructure.Authentication;
using Network.Core.Interfaces;
using Network.API.Service;
using Network.Core.Models;
using Network.Core.Constant;
using System.Security.Claims;
using Network.Core.Helpers;
using Network.Core.Core;
using Microsoft.Extensions.Configuration;
using Network.API.Infrastructure.Authorization;
using Network.API.Model;
using Newtonsoft.Json;
using Google.Apis.Auth;
using System.Collections.Generic;
using Network.API.Infrastructure;
using FacebookCore;
using Network.InfraCore.Email;

namespace Network.API.Controllers
{
    [ApiController]
    [AuthorizeFilter]
    [Route("api/[controller]")]
    public class Net_LinksMapsController : ControllerBase
    {        
        private readonly IUserProvider _userProvider;        
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IServiceWrapper _service;
        private readonly ILogger<Net_LinksMapsController> _logger;
        private readonly AppSettings appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Net_LinksMapsController(IServiceWrapper service, IUserProvider userService, IJwtAuthManager jwtAuthManager, ILogger<Net_LinksMapsController> logger, IConfiguration rootConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            appSettings = new AppSettings();
            rootConfiguration.Bind(appSettings);
            _userProvider = userService;            
            _jwtAuthManager = jwtAuthManager;
            _service = service;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Switches")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSwitchesAsync()
        {
            try
            {
                var items = await _service.Net_Devices.GetSwitchesAsync(); // ⚠️ Bổ sung "await"
                return ResponseMessage.Success(items);
            }
            catch (SecurityTokenException ex)
            {
                return ResponseMessage.Error(ex.Message, null, 401);
            }
        }

        [HttpGet("Connections")]
        [AllowAnonymous]
        public async Task<IActionResult> GetConnectionsAsync()
        {
            try
            {
                var items = await _service.Net_NetworkLinks.GetConnectionsAsync();
                return ResponseMessage.Success(items);
            }
            catch (SecurityTokenException ex)
            {
                return ResponseMessage.Error(ex.Message, null, 401);
            }
        }
    }
}
