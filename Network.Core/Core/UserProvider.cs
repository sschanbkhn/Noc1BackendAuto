using Microsoft.AspNetCore.Http;
using Network.Core.Constant;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Core
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public bool IsAuthenticated
        {
            get
            {
                bool isAuthenticated = false;
                try
                {
                    isAuthenticated = _context.HttpContext.User.Identity.IsAuthenticated;
                }
                catch { }
                return isAuthenticated;
            }
        }

        public Guid Id
        {
            get
            {
                Guid userId = Guid.Empty;
                try
                {
                    userId = Guid.Parse(_context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                catch { }
                return userId;
            }
        }
        public string UserName
        {
            get
            {
                string userName = string.Empty;
                try
                {
                    userName = _context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                }
                catch { }
                return userName;
            }
        }        
    }
}
