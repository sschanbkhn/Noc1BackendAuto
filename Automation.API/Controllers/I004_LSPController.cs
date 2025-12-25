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
    public class I004_LSPController : ApiControllerBase<Model.I004_LSP>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<I004_LSPController> _logger;

        public I004_LSPController(IServiceWrapper service, ILogger<I004_LSPController> logger)
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Lấy dữ liệu LSP quốc tế
        /// </summary>
        [HttpGet("GetLSPInternationalData")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetLSPInternationalData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                _logger.LogInformation($"Call GetLSPInternationalData params: (fromDate = {fromDate}, toDate = {toDate})");
                var items = await _service.I004_LSP.GetLSPInternationalDataAsync(fromDate, toDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLSPInternationalData : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách P-Data (router có pcep_address)
        /// </summary>
        [HttpGet("GetPDataList")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetPDataList()
        {
            try
            {
                _logger.LogInformation("Call GetPDataList");
                var items = await _service.I004_LSP.GetPDataListAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetPDataList : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách POP-Data (router có pcep_address)
        /// </summary>
        [HttpGet("GetPOPDataList")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetPOPDataList()
        {
            try
            {
                _logger.LogInformation("Call GetPOPDataList");
                var items = await _service.I004_LSP.GetPOPDataListAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetPOPDataList : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lấy trạng thái Route PCEP
        /// </summary>
        [HttpGet("GetRoutePCEPStatus")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetRoutePCEPStatus()
        {
            try
            {
                _logger.LogInformation("Call GetRoutePCEPStatus");
                var result = await _service.I004_LSP.GetRoutePCEPStatusAsync();
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRoutePCEPStatus : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lấy trạng thái LSP Delegated
        /// </summary>
        [HttpGet("GetLSPDelegatedStatus")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetLSPDelegatedStatus()
        {
            try
            {
                _logger.LogInformation("Call GetLSPDelegatedStatus");
                var result = await _service.I004_LSP.GetLSPDelegatedStatusAsync();
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLSPDelegatedStatus : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lấy thống kê các hoạt động LSP (Add, Update, Remove)
        /// </summary>

        /// <summary>
        /// Lấy dữ liệu bandwidth LSP giữa các node (có thể nhiều node)
        /// </summary>
        [HttpGet("GetLSPBandwidthData")]
        public async Task<IActionResult> GetLSPBandwidthData([FromQuery] string[] fromIdNodes, [FromQuery] string[] toIdNodes, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            try
            {
                // Validate inputs
                if (fromIdNodes == null || fromIdNodes.Length == 0)
                {
                    return BadRequest(new { Success = false, Message = "fromIdNodes parameter is required" });
                }

                if (toIdNodes == null || toIdNodes.Length == 0)
                {
                    return BadRequest(new { Success = false, Message = "toIdNodes parameter is required" });
                }

                // Validate date range
                if (fromDate >= toDate)
                {
                    return BadRequest(new { Success = false, Message = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc" });
                }

                // Limit date range to prevent performance issues (max 30 days)
                var maxDays = 30;
                if ((toDate - fromDate).TotalDays > maxDays)
                {
                    return BadRequest(new { Success = false, Message = $"Khoảng thời gian không được vượt quá {maxDays} ngày" });
                }

                var data = await _service.I004_LSP.GetLSPBandwidthDataAsync(fromIdNodes, toIdNodes, fromDate, toDate);

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    Message = $"Retrieved {data.Count} bandwidth records from {fromDate:yyyy-MM-dd HH:mm} to {toDate:yyyy-MM-dd HH:mm}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLSPBandwidthData");
                return StatusCode(500, new { Success = false, Message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        /// <summary>
        /// Lấy dữ liệu bandwidth LSP theo path - API mới với parameters linh hoạt
        /// </summary>
        [HttpGet("bandwidthbypath")]
        public async Task<IActionResult> GetBandwidthByPath([FromQuery] string[] fromData, [FromQuery] string[] toData, [FromQuery] string timeRange, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                // Validate inputs
                if (fromData == null || fromData.Length == 0)
                {
                    return BadRequest(new { Success = false, Message = "Vui lòng cung cấp fromData" });
                }

                if (toData == null || toData.Length == 0)
                {
                    return BadRequest(new { Success = false, Message = "Vui lòng cung cấp toData" });
                }

                if (string.IsNullOrEmpty(timeRange))
                {
                    return BadRequest(new { Success = false, Message = "Vui lòng cung cấp timeRange" });
                }

                var data = await _service.I004_LSP.GetBandwidthByPathAsync(fromData, toData, timeRange, fromDate, toDate);

                // Calculate actual time range used for response
                DateTime calculatedFromDate;
                DateTime calculatedToDate = DateTime.Now;

                switch (timeRange?.ToLower())
                {
                    case "3h":
                        calculatedFromDate = calculatedToDate.AddHours(-3);
                        break;
                    case "12h":
                        calculatedFromDate = calculatedToDate.AddHours(-12);
                        break;
                    case "24h":
                        calculatedFromDate = calculatedToDate.AddDays(-1);
                        break;
                    case "1w":
                        calculatedFromDate = calculatedToDate.AddDays(-7);
                        break;
                    case "1m":
                        calculatedFromDate = calculatedToDate.AddDays(-30);
                        break;
                    case "manual":
                        calculatedFromDate = fromDate ?? calculatedToDate.AddDays(-1);
                        calculatedToDate = toDate ?? calculatedToDate;
                        break;
                    default:
                        calculatedFromDate = calculatedToDate.AddDays(-1);
                        break;
                }

                return Ok(new
                {
                    Success = true,
                    Data = data,
                    TimeRange = new
                    {
                        From = calculatedFromDate,
                        To = calculatedToDate,
                        Range = timeRange,
                        DurationHours = Math.Round((calculatedToDate - calculatedFromDate).TotalHours, 2)
                    },
                    Message = $"Retrieved {data.Count} bandwidth records for {timeRange} range"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBandwidthByPath");
                return StatusCode(500, new { Success = false, Message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [HttpGet("GetLSPActionStats")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetLSPActionStats([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                // Default to last 24 hours if no dates provided
                var calculatedToDate = toDate ?? DateTime.Now;
                var calculatedFromDate = fromDate ?? calculatedToDate.AddDays(-1);

                // Validate date range
                if (calculatedFromDate >= calculatedToDate)
                {
                    return BadRequest(new { Success = false, Message = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc" });
                }

                // Limit date range to prevent performance issues (max 90 days)
                var maxDays = 90;
                if ((calculatedToDate - calculatedFromDate).TotalDays > maxDays)
                {
                    return BadRequest(new { Success = false, Message = $"Khoảng thời gian không được vượt quá {maxDays} ngày" });
                }

                _logger.LogInformation($"Call GetLSPActionStats params: (fromDate = {calculatedFromDate}, toDate = {calculatedToDate})");
                var result = await _service.I004_LSP.GetLSPActionStatsAsync(calculatedFromDate, calculatedToDate);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLSPActionStats : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        /// <summary>
        /// Debug database - check data availability
        /// </summary>
        [HttpGet("debugdatabase")]
        [AuthorizeFilter]
        public async Task<IActionResult> DebugDatabase()
        {
            try
            {
                var result = await _service.I004_LSP.DebugDatabaseAsync();
                return Ok(new { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError($"DebugDatabase : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
