using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Service;
using Network.Core.Interfaces;
using Network.API.Model;
using Network.API.Infrastructure.Authorization;
using Network.Core.Models;
using System.Threading.Tasks;
using System;

namespace Network.API.Controllers
{
    public class Net_HistoryCurenAlarmController : ApiControllerBase<Net_HistoryCurenAlarm>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Net_HistoryCurenAlarmController> _logger;
        private readonly IUserProvider _userProvider;
        public Net_HistoryCurenAlarmController(IServiceWrapper service, IUserProvider userProvider, ILogger<Net_HistoryCurenAlarmController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
            _userProvider = userProvider;
        }

        [HttpGet("GetList/{page}/{pageSize}/{totalLimitItems}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetListAsync(int page = 1, int pageSize = 10, int totalLimitItems = 500)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetListPaged params: (page = {0}, pageSize = {1}, totalLimitItems = {2})", page, pageSize, totalLimitItems));
                var items = await _service.Net_HistoryCurenAlarm.GetListAsync(page, pageSize, totalLimitItems);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetListPaged : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
