using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Network.API.Infrastructure.Authorization;
using Network.API.Service;
using Network.Core.Helpers;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Network.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase<TEntity> : ControllerBase
    {        
        private ServiceDecorator<TEntity> _serviceDecorator;
        private readonly ILogger _logger;        
        public ApiControllerBase(IServiceWrapper service, ILogger logger)
        {            
            _logger = logger;
            _serviceDecorator = new ServiceDecorator<TEntity>(service);
        }        
        [HttpGet("{page}/{pageSize}/{totalLimitItems}")]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> GetListPaged(int page = 1, int pageSize = 10, int totalLimitItems = 500)
        {
            try
            {                
                _logger.LogInformation(string.Format("Call GetListPaged params: (page = {0}, pageSize = {1}, totalLimitItems = {2})", page, pageSize, totalLimitItems));
                var items = await _serviceDecorator.GetPagedAsync(page, pageSize, totalLimitItems, "");                
                return ResponseMessage.Success(items);                               
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("GetListPaged : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("Categories")]
        [AuthorizeFilter]
        public virtual IActionResult GetCategories()
        {
            try
            {
                _logger.LogInformation("Call GetCategories");
                var items = _serviceDecorator.GetCategories();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetCategories : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetById params: (id = {0})", id));
                var item = await _serviceDecorator.GetByIdAsync(id);                
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetById : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> Create([FromBody] TEntity model)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Create body: ({0})", JsonConvert.SerializeObject(model)));
                var item = await _serviceDecorator.SaveEntityAsync(model);                
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Create : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPut]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> Update([FromBody] TEntity model)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Update body: ({0})", JsonConvert.SerializeObject(model)));
                var item = await _serviceDecorator.SaveEntityAsync(model);                
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Update : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("SaveMultiple")]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> SaveMultiple([FromBody] TEntity[] models)
        {
            try
            {
                _logger.LogInformation("Call Save multiple");
                var items = await _serviceDecorator.SaveEntitiesAsync(models);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Save : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpDelete]
        [AuthorizeFilter]
        public virtual async Task<IActionResult> DeleteMultiple([FromBody] List<TEntity> models)
        {
            try
            {
                _logger.LogInformation("Call DeleteMultiple");
                await _serviceDecorator.Delete(models);                
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("DeleteMultiple : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
