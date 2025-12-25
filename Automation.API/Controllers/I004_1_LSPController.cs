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
    public class I004_1_LSPController : ApiControllerBase<I004_1_LSP>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<I004_1_LSPController> _logger;
        
        public I004_1_LSPController(IServiceWrapper service, ILogger<I004_1_LSPController> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet("GetLSPData")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetLSPData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                _logger.LogInformation($"Call GetLSPData params: (fromDate = {fromDate}, toDate = {toDate})");
                var items = await _service.I004_1_LSP.GetLSPDataAsync(fromDate, toDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLSPData : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
