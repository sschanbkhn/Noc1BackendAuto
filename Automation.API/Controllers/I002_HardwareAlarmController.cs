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
    public class I002_HardwareAlarmController : ApiControllerBase<I002_HardwareAlarm>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<I002_HardwareAlarmController> _logger;
        
        public I002_HardwareAlarmController(IServiceWrapper service, ILogger<I002_HardwareAlarmController> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet("GetList")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetList()
        {
            try
            {
                _logger.LogInformation("Call GetList for Hardware Alarm");
                var items = await _service.I002_HardwareAlarm.GetHardwareAlarmListAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetList Hardware Alarm: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("Check")]
        [AuthorizeFilter]
        public async Task<IActionResult> Check([FromBody] CheckAlarmRequest request)
        {
            try
            {
                _logger.LogInformation($"Call Check alarm for ID: {request.alarmId}");
                
                if (request.alarmId <= 0)
                {
                    return ResponseMessage.Error("Alarm ID is required");
                }
                
                if (string.IsNullOrEmpty(request.username))
                {
                    return ResponseMessage.Error("Username is required");
                }
                
                var result = await _service.I002_HardwareAlarm.CheckAlarmAsync(request.alarmId, request.username);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Check alarm {request?.alarmId}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("AutoReboot")]
        [AuthorizeFilter]
        public async Task<IActionResult> AutoReboot([FromBody] AutoRebootRequest request)
        {
            try
            {
                _logger.LogInformation($"Call AutoReboot for alarm ID: {request.alarmId}, device: {request.deviceName}");
                
                if (request.alarmId <= 0)
                {
                    return ResponseMessage.Error("Alarm ID is required");
                }
                
                if (string.IsNullOrEmpty(request.deviceName))
                {
                    return ResponseMessage.Error("Device name is required");
                }
                
                if (string.IsNullOrEmpty(request.username))
                {
                    return ResponseMessage.Error("Username is required");
                }
                
                var result = await _service.I002_HardwareAlarm.AutoRebootAsync(
                    request.alarmId, 
                    request.deviceName, 
                    request.fpcSlot, 
                    request.keyword,
                    request.username
                );
                
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AutoReboot for alarm {request?.alarmId}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("ManualHandle")]
        [AuthorizeFilter]
        public async Task<IActionResult> ManualHandle([FromBody] ManualHandleRequest request)
        {
            try
            {
                _logger.LogInformation($"Call ManualHandle for alarm ID: {request.alarmId}");
                
                if (request.alarmId <= 0)
                {
                    return ResponseMessage.Error("Alarm ID is required");
                }
                
                if (string.IsNullOrEmpty(request.username))
                {
                    return ResponseMessage.Error("Username is required");
                }
                
                var result = await _service.I002_HardwareAlarm.ManualHandleAsync(
                    request.alarmId, 
                    request.username, 
                    request.causeName ?? "Manual Handle"
                );
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ManualHandle for alarm {request?.alarmId}: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetErrorLinksStatus")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetErrorLinksStatus()
        {
            try
            {
                _logger.LogInformation("Call GetErrorLinksStatus");
                var items = await _service.I002_HardwareAlarm.GetErrorLinksStatusAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetErrorLinksStatus: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("GetHardwareAlarmHistory")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetHardwareAlarmHistory()
        {
            try
            {
                _logger.LogInformation("Call GetHardwareAlarmHistory");
                var items = await _service.I002_HardwareAlarm.GetHardwareAlarmHistoryAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetHardwareAlarmHistory: {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
    
    public class CheckAlarmRequest
    {
        public int alarmId { get; set; }
        public string username { get; set; }
    }
    
    public class AutoRebootRequest
    {
        public int alarmId { get; set; }
        public string deviceName { get; set; }
        public string fpcSlot { get; set; }
        public string keyword { get; set; }
        public string username { get; set; }
    }
    
    public class ManualHandleRequest
    {
        public int alarmId { get; set; }
        public string username { get; set; }
        public string causeName { get; set; }
    }
}
