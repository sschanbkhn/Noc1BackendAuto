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
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static Network.Core.Constant.Sys_Const;

namespace Network.API.Controllers
{
    public class Speed_FileImportController : ApiControllerBase<Speed_FileImport>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Speed_FileImportController> _logger;
        public Speed_FileImportController(IServiceWrapper service, ILogger<Speed_FileImportController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet("LastFileImport")]
        [AuthorizeFilter]
        public async Task<IActionResult> LastFileImportAsync()
        {
            try
            {
                _logger.LogInformation("Call LastFileImportAsync");
                var result = await _service.Speed_FileImport.LastFileImportAsync();
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("LastFileImportAsync : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
