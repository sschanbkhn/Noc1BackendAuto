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

namespace Network.API.Controllers
{
    [ApiController]
    [AuthorizeFilter]
    [Route("api/[controller]")]
    public class Sys_DashboardController : ControllerBase
    {                           
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_DashboardController> _logger;
        public Sys_DashboardController(IServiceWrapper service, ILogger<Sys_DashboardController> logger)
        {                                            
            _service = service;
            _logger = logger;
        }
        
    }
}
