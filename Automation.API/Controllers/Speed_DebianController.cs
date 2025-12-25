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
using Newtonsoft.Json;
using Network.API.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Network.API.Controllers
{
    public class Speed_DebianController : ApiControllerBase<Speed_Debian>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Speed_DebianController> _logger;        
        public Speed_DebianController(IServiceWrapper service, ILogger<Speed_DebianController> logger) :base(service, logger)
        {
            _service = service;
            _logger = logger;            
        }
        [HttpGet("GetSpeedDebianByDate")]
        [AuthorizeFilter]
        public async Task<ActionResult<List<Speed_Debian>>> GetSpeedDebianByDate([FromQuery] DateTime resultReceivedDate)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetSpeedDebianByDate params: (id = {0}", resultReceivedDate));
                var result = await _service.Speed_Debian.GetSpeedDebianByDateAsync(resultReceivedDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("CheckDuplicateAttributes : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
       
        //[HttpDelete("DeleteById/{Id}")]
        //[AuthorizeFilter]
        //public async Task<IActionResult> DeleteById(Guid Id)
        //{
        //    try
        //    {
        //        _logger.LogInformation(string.Format("Call DeleteById params: (id = {0})", Id));
        //        await _service.Sys_EmailSms.DeleteById(Id);
        //        return ResponseMessage.Success();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(string.Format("DeleteById : {0}", ex.Message));
        //        return ResponseMessage.Error(ex.Message);
        //    }
        //}

    }
}
