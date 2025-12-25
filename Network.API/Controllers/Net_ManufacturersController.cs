using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Service;
using Network.Core.Interfaces;
using Network.API.Model;

namespace Network.API.Controllers
{
    public class Net_ManufacturersController : ApiControllerBase<Net_Manufacturers>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Net_ManufacturersController> _logger;
        private readonly IUserProvider _userProvider;
        public Net_ManufacturersController(IServiceWrapper service, IUserProvider userProvider, ILogger<Net_ManufacturersController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
            _userProvider = userProvider;
        }
    }
}
