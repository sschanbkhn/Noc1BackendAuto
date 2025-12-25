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
    public class I003_BNGController : ApiControllerBase<I003_BNG>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<I003_BNGController> _logger;
        
        public I003_BNGController(IServiceWrapper service, ILogger<I003_BNGController> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet("GetBNGData")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetBNGData()
        {
            try
            {
                _logger.LogInformation("Call GetBNGData");
                var items = await _service.I003_BNG.GetBNGDataAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetBNGData : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("ClearOverLimitSession")]
        [AuthorizeFilter]
        public async Task<IActionResult> ClearOverLimitSession([FromBody] ClearSessionRequest request)
        {
            try
            {
                _logger.LogInformation($"Call ClearOverLimitSession for IP: {request.ip}");
                
                if (string.IsNullOrEmpty(request.ip))
                {
                    return ResponseMessage.Error("IP address is required");
                }
                
                var result = await _service.I003_BNG.ClearOverLimitSessionAsync(request.ip);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClearOverLimitSession for IP {request?.ip}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("CheckOneUser")]
        [AuthorizeFilter]
        public async Task<IActionResult> CheckOneUser([FromBody] UserActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Call CheckOneUser for Username: {request.username}, IP: {request.ip}");
                
                if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.ip))
                {
                    return ResponseMessage.Error("Username and IP address are required");
                }
                
                var result = await _service.I003_BNG.CheckOneUserAsync(request.username, request.ip);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CheckOneUser for Username {request?.username}, IP {request?.ip}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("ClearOverLimitOneUser")]
        [AuthorizeFilter]
        public async Task<IActionResult> ClearOverLimitOneUser([FromBody] UserActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Call ClearOverLimitOneUser for Username: {request.username}, IP: {request.ip}");
                
                if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.ip))
                {
                    return ResponseMessage.Error("Username and IP address are required");
                }
                
                var result = await _service.I003_BNG.ClearOverLimitOneUserAsync(request.username, request.ip);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClearOverLimitOneUser for Username {request?.username}, IP {request?.ip}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("ClearAllOneUser")]
        [AuthorizeFilter]
        public async Task<IActionResult> ClearAllOneUser([FromBody] UserActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Call ClearAllOneUser for Username: {request.username}, IP: {request.ip}");
                
                if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.ip))
                {
                    return ResponseMessage.Error("Username and IP address are required");
                }
                
                var result = await _service.I003_BNG.ClearAllOneUserAsync(request.username, request.ip);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClearAllOneUser for Username {request?.username}, IP {request?.ip}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetDashboardData")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboardData([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                _logger.LogInformation($"Call GetDashboardData from {fromDate} to {toDate}");
                
                var result = await _service.I003_BNG.GetDashboardDataAsync(fromDate, toDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDashboardData: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetLocationList")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetLocationList()
        {
            try
            {
                _logger.LogInformation("Call GetLocationList");
                var items = await _service.I003_BNG.GetLocationListAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLocationList : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetBNGDataByLocation")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetBNGDataByLocation([FromQuery] string location, [FromQuery] DateTime? reportDate = null)
        {
            try
            {
                _logger.LogInformation($"Call GetBNGDataByLocation for location: {location}, date: {reportDate}");
                
                if (string.IsNullOrEmpty(location))
                {
                    return ResponseMessage.Error("Location is required");
                }
                
                var result = await _service.I003_BNG.GetBNGDataByLocationAsync(location, reportDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetBNGDataByLocation for location {location}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetSessionUserDashboardData")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetSessionUserDashboardData([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                _logger.LogInformation($"Call GetSessionUserDashboardData from {fromDate} to {toDate}");
                
                var result = await _service.I003_BNG.GetSessionUserDashboardDataAsync(fromDate, toDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetSessionUserDashboardData: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
    
    public class ClearSessionRequest
    {
        public string ip { get; set; }
    }
    
    public class UserActionRequest
    {
        public string username { get; set; }
        public string ip { get; set; }
    }
}