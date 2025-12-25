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
    public class Sys_PermissionController : ApiControllerBase<Sys_Permission>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_CategoryController> _logger;
        public Sys_PermissionController(IServiceWrapper service, ILogger<Sys_CategoryController> logger) :base(service, logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost("Save")]
        [AuthorizeFilter]
        public async Task<IActionResult> Save([FromBody] Guid[] resourceIds, Guid roleId, bool isFunc)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Save params: (resourceIds = {0}, roleId = {1}, isFunc = {2})", resourceIds != null && resourceIds.Length > 0 ? string.Join(", ", resourceIds): "", roleId, isFunc));
                var items = await _service.Sys_Permission.SaveAsync(resourceIds, roleId, isFunc);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Save : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("GetByRoleId")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetByRoleId(Guid roleId, bool isFunc)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetByRoleId params: (roleId = {0}, isFunc = {1})", roleId, isFunc));
                var items = await _service.Sys_Permission.GetByRoleIdAsync(roleId, isFunc);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetByRoleId : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("GetMenusByRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenusByRoles([FromBody] List<string> roles)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetMenusByRoles params: (roles = {0})", roles != null && roles.Count > 0 ? string.Join(", ", roles) : ""));
                var items = await _service.Sys_Permission.GetMenusByRoles(roles);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetMenusByRoles : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
