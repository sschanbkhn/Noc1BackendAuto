using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Infrastructure.Authorization;
using Network.API.Service;
using Network.API.Controllers;
using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Controllers
{
    public class Rnoc_R008Controller : ApiControllerBase<R008_RunScheduler>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Rnoc_R008Controller> _logger;
        
        public Rnoc_R008Controller(IServiceWrapper service, ILogger<Rnoc_R008Controller> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        /// <summary>
        /// Get dashboard statistics by day (with hourly filter)
        /// </summary>
        [HttpGet("dashboard/day")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboardByDay([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetDashboardByDay params: (startDate = {startDate:yyyy-MM-dd HH:mm}, endDate = {endDate:yyyy-MM-dd HH:mm})");
                var result = await _service.Rnoc_R008.GetDashboardByDayAsync(startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetDashboardByDay : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Get dashboard statistics by week
        /// </summary>
        [HttpGet("dashboard/week")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboardByWeek([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetDashboardByWeek params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var result = await _service.Rnoc_R008.GetDashboardByWeekAsync(startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetDashboardByWeek : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Get dashboard statistics by month
        /// </summary>
        [HttpGet("dashboard/month")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboardByMonth([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetDashboardByMonth params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var result = await _service.Rnoc_R008.GetDashboardByMonthAsync(startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetDashboardByMonth : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Get scheduler records with pagination
        /// </summary>
        [HttpGet("records/paged")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetSchedulerRecordsPaged(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetSchedulerRecordsPaged params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd}, page = {page}, pageSize = {pageSize})");
                var result = await _service.Rnoc_R008.GetSchedulerRecordsPagedAsync(startDate, endDate, page, pageSize);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetSchedulerRecordsPaged : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Get statistics by specific date
        /// </summary>
        [HttpGet("statistics")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetStatisticsByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetStatisticsByDate params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R008.GetStatisticsByDateAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetStatisticsByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Get records by cell name
        /// </summary>
        [HttpGet("records/by-cell")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetRecordsByCellName(
            [FromQuery] string cellName,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R008 GetRecordsByCellName params: (cellName = {cellName}, startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var result = await _service.Rnoc_R008.GetRecordsByCellNameAsync(cellName, startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 GetRecordsByCellName : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        /// <summary>
        /// Export scheduler records to CSV
        /// </summary>
        [HttpGet("export/csv")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportToCsv([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R008 ExportToCsv params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var csvContent = await _service.Rnoc_R008.ExportSchedulerRecordsToCsvAsync(startDate, endDate);
                var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
                return File(bytes, "text/csv", $"R008_PowerSaving_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError($"R008 ExportToCsv : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
