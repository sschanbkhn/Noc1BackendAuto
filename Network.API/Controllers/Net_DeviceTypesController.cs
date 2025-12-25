using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Service;
using Network.Core.Interfaces;
using Network.API.Model;

namespace Network.API.Controllers
{
    public class Net_DeviceTypesController : ApiControllerBase<Net_DeviceTypes>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Net_DeviceTypesController> _logger;
        private readonly IUserProvider _userProvider;
        public Net_DeviceTypesController(IServiceWrapper service, IUserProvider userProvider, ILogger<Net_DeviceTypesController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
            _userProvider = userProvider;
        }
    }
}
