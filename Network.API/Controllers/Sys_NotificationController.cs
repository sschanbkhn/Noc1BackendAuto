using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Infrastructure.Authorization;
using Network.API.Service;
using Network.API.Controllers;
using Network.API.Model;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Controllers
{
    public class Sys_NotificationController : ApiControllerBase<Sys_Notification>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_NotificationController> _logger;
        public Sys_NotificationController(IServiceWrapper service, ILogger<Sys_NotificationController> logger) :base(service, logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}
