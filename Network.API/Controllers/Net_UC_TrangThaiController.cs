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
    public class Net_UC_TrangThaiController : ApiControllerBase<Net_UC_TrangThai>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Net_UC_TrangThaiController> _logger;
        private readonly IUserProvider _userProvider;
        public Net_UC_TrangThaiController(IServiceWrapper service, IUserProvider userProvider, ILogger<Net_UC_TrangThaiController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
            _userProvider = userProvider;
        }

        [HttpGet("{page}/{pageSize}/{totalLimitItems}/{type}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetListPagedByType(int page = 1, int pageSize = 10, int totalLimitItems = 500, int type = 0)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetListPagedByType params: (page = {0}, pageSize = {1}, totalLimitItems = {2}, type = {3})", page, pageSize, totalLimitItems, type));
                string search = $"type = {type}";
                var items = await _service.Net_UC_TrangThais.GetPagedAsync(page, pageSize, totalLimitItems, search);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetListPagedByType : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

    }
}
