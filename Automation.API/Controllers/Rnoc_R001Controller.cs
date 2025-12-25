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
    public class Rnoc_R001Controller : ApiControllerBase<R001_DataRuntime>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Rnoc_R001Controller> _logger;
        
        public Rnoc_R001Controller(IServiceWrapper service, ILogger<Rnoc_R001Controller> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet("dashboard")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetDashboard oteparams: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetDashboardDataAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetDashboard : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("configured-sites")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetConfiguredSites([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetConfiguredSites params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetConfiguredSitesByDateAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetConfiguredSites : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("configured-sites-range")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetConfiguredSitesRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetConfiguredSitesRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetConfiguredSitesByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetConfiguredSitesRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("bad-configurations")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetBadConfigurations([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetBadConfigurations params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetBadConfigurationsByDateAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetBadConfigurations : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("bad-configurations-range")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetBadConfigurationsRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetBadConfigurationsRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetBadConfigurationsByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetBadConfigurationsRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        // ⚡ Server-side pagination endpoint for configured sites
        [HttpGet("configured-sites-paged")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetConfiguredSitesPaged([FromQuery] DateTime date, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetConfiguredSitesPaged params: (date = {date:yyyy-MM-dd}, page = {page}, pageSize = {pageSize})");
                var (data, totalCount) = await _service.Rnoc_R001.GetConfiguredSitesByDatePagedAsync(date, page, pageSize);
                
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var response = new
                {
                    Data = data,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                };
                
                return ResponseMessage.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetConfiguredSitesPaged : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        // ⚡ Server-side pagination endpoint for bad configurations
        [HttpGet("bad-configurations-paged")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetBadConfigurationsPaged([FromQuery] DateTime date, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetBadConfigurationsPaged params: (date = {date:yyyy-MM-dd}, page = {page}, pageSize = {pageSize})");
                var (data, totalCount) = await _service.Rnoc_R001.GetBadConfigurationsByDatePagedAsync(date, page, pageSize);
                
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var response = new
                {
                    Data = data,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                };
                
                return ResponseMessage.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetBadConfigurationsPaged : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("correct-configurations")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetCorrectConfigurations([FromBody] R001DetailRequest request)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetCorrectConfigurations params: (date = {request.Date}, page = {request.Page}, pageSize = {request.PageSize})");
                var result = await _service.Rnoc_R001.GetCorrectConfigurationsAsync(request);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetCorrectConfigurations : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("incorrect-configurations")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetIncorrectConfigurations([FromBody] R001DetailRequest request)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetIncorrectConfigurations params: (date = {request.Date}, page = {request.Page}, pageSize = {request.PageSize})");
                var result = await _service.Rnoc_R001.GetIncorrectConfigurationsAsync(request);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetIncorrectConfigurations : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("parameter-details")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetParameterDetails([FromBody] R001DetailRequest request)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetParameterDetails params: (date = {request.Date}, parameter = {request.ParameterName}, isCorrect = {request.IsCorrect})");
                var result = await _service.Rnoc_R001.GetParameterDetailsAsync(request);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetParameterDetails : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("statistics")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetStatistics params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetStatisticsAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetStatistics : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("parameter-summaries")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetParameterSummaries([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetParameterSummaries params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetParameterSummariesAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetParameterSummaries : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("total-unique-ne")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetTotalUniqueNE([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetTotalUniqueNE params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R001.GetTotalUniqueNECountAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetTotalUniqueNE : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("export-configured-sites")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportConfiguredSites([FromBody] ExportDateRangeRequest request)
        {
            try
            {
                _logger.LogInformation($"Call R001 ExportConfiguredSites params: (startDate = {request.StartDate}, endDate = {request.EndDate})");
                var result = await _service.Rnoc_R001.ExportConfiguredSitesToCsvAsync(DateTime.Parse(request.StartDate), DateTime.Parse(request.EndDate));
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 ExportConfiguredSites : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("export-bad-configurations")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportBadConfigurations([FromBody] ExportDateRangeRequest request)
        {
            try
            {
                _logger.LogInformation($"Call R001 ExportBadConfigurations params: (startDate = {request.StartDate}, endDate = {request.EndDate})");
                var result = await _service.Rnoc_R001.ExportBadConfigurationsToCsvAsync(DateTime.Parse(request.StartDate), DateTime.Parse(request.EndDate));
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 ExportBadConfigurations : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("fix-single-configuration")]
        [AuthorizeFilter]
        public async Task<IActionResult> FixSingleConfiguration([FromBody] R001_SchedulerFixParameter fixRequest)
        {
            try
            {
                _logger.LogInformation($"Call R001 FixSingleConfiguration params: (NeName = {fixRequest.NeName}, CellId = {fixRequest.CellId})");
                var result = await _service.Rnoc_R001.FixSingleConfigurationAsync(fixRequest);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 FixSingleConfiguration : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpPost("fix-all-configurations")]
        [AuthorizeFilter]
        public async Task<IActionResult> FixAllConfigurations([FromBody] List<R001_SchedulerFixParameter> fixRequests)
        {
            try
            {
                _logger.LogInformation($"Call R001 FixAllConfigurations params: (count = {fixRequests.Count})");
                var result = await _service.Rnoc_R001.FixAllConfigurationsAsync(fixRequests);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 FixAllConfigurations : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("fix-parameters-paged")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetFixParametersPaged([FromQuery] DateTime date, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                _logger.LogInformation($"Call R001 GetFixParametersPaged params: (date = {date:yyyy-MM-dd}, page = {page}, pageSize = {pageSize})");
                var (data, totalCount) = await _service.Rnoc_R001.GetFixParametersByDatePagedAsync(date, page, pageSize);
                return ResponseMessage.Success(new { Data = data, TotalCount = totalCount });
            }
            catch (Exception ex)
            {
                _logger.LogError($"R001 GetFixParametersPaged : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
    
    public class ExportDateRangeRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}