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
    public class Sys_RoleController : ApiControllerBase<Sys_Role>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_RoleController> _logger;
        public Sys_RoleController(IServiceWrapper service, ILogger<Sys_RoleController> logger) :base(service, logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("CheckDuplicateAttributes")]
        [AuthorizeFilter]
        public async Task<IActionResult> CheckDuplicateAttributes(Guid? id, string code)
        {
            try
            {
                _logger.LogInformation(string.Format("Call CheckDuplicateAttributes params: (id = {0}, code = {1})", id, code));
                var result = await _service.Sys_Role.IsDupicateAttributesAsync(id, code);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("CheckDuplicateAttributes : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpDelete("DeleteById/{Id}")]
        [AuthorizeFilter]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            try
            {
                _logger.LogInformation(string.Format("Call DeleteById params: (id = {0})", Id));
                await _service.Sys_Role.DeleteById(Id);
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("DeleteById : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
