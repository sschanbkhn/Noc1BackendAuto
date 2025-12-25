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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Network.API.Controllers
{
    public class Rnoc_R009Controller : ApiControllerBase<Hw_BtsData>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Rnoc_R009Controller> _logger;
        
        public Rnoc_R009Controller(IServiceWrapper service, ILogger<Rnoc_R009Controller> logger) 
            : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        
        // Huawei endpoints
        [HttpGet("hw_GetBtsDataByDate")]
        [AuthorizeFilter]
        public async Task<IActionResult> hw_GetBtsDataByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call hw_GetBtsDataByDate params: (date = {date:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetBtsDataByDateAsync(date);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"hw_GetBtsDataByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("hw_GetBtsDataByDateRange")]
        [AuthorizeFilter]
        public async Task<IActionResult> hw_GetBtsDataByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call hw_GetBtsDataByDateRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetBtsDataByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"hw_GetBtsDataByDateRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("hw_ExportBtsDataToExcel")]
        [AuthorizeFilter]
        public async Task<IActionResult> hw_ExportBtsDataToExcel([FromBody] ExportBtsDataRequest request)
        {
            try
            {
                _logger.LogInformation($"Call hw_ExportBtsDataToExcel params: (fromDate = {request.FromDate}, toDate = {request.ToDate}, vendor = {request.Vendor})");
                
                // Parse ngày
                DateTime fromDate = DateTime.Parse(request.FromDate);
                DateTime toDate = DateTime.Parse(request.ToDate);

                // Truy vấn dữ liệu
                var items = await _service.Rnoc_R009.GetBtsDataByDateRangeAsync(fromDate, toDate);
                
                // Lọc vendor nếu cần
                if (request.Vendor != "huawei")
                {
                    string vendor = request.Vendor.ToLower();
                    items = items.Where(item =>
                    {
                        var nename = item.Nename?.ToLower() ?? "";
                        return (vendor == "huawei" && (nename.Contains("huawei") || nename.Contains("hw")))
                            || (vendor == "nokia" && (nename.Contains("nokia") || nename.Contains("nk")))
                            || (vendor == "ericsson" && (nename.Contains("ericsson") || nename.Contains("er")));
                    }).ToList();
                }

                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }

                // Tạo CSV thay vì Excel để tránh lỗi Gdip
                var csvContent = new System.Text.StringBuilder();
                
                // Header for Huawei
                csvContent.AppendLine("TT,Cell Name,ID Cell,EnodeB ID,Local Cell ID,UL EARFCN,DL EARFCN,Root Sequence Index,TxRx Mode,UL Bandwidth,DL Bandwidth,Frequency Band,NE Name,TAC,Physical Cell ID,Create Date");
                
                // Data
                int stt = 1;
                foreach (var item in items)
                {
                    csvContent.AppendLine($"{stt}," +
                        $"\"{item.CellName}\"," +
                        $"{item.IdCell}," +
                        $"\"{item.EnodebId}\"," +
                        $"\"{item.LocalCellId}\"," +
                        $"\"{item.Ulearfcncfgind}\"," +
                        $"\"{item.Dlearfcn}\"," +
                        $"\"{item.RootSequenceIdx}\"," +
                        $"\"{item.Txrxmode}\"," +
                        $"\"{item.Ulbandwidth}\"," +
                        $"\"{item.Dlbandwidth}\"," +
                        $"\"{item.Freqband}\"," +
                        $"\"{item.Nename}\"," +
                        $"\"{item.Tac}\"," +
                        $"\"{item.Phycellid}\"," +
                        $"\"{item.CreateDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                    stt++;
                }
                
                var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent.ToString());
                return ResponseMessage.Success(Convert.ToBase64String(csvBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("hw_ExportBtsDataToExcel : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        // Nokia 4G endpoints
        [HttpGet("nk_GetBtsDataByDate")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk_GetBtsDataByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call nk_GetBtsDataByDate params: (date = {date:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetNokiaBtsDataByDateAsync(date);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"nk_GetBtsDataByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("nk_GetBtsDataByDateRange")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk_GetBtsDataByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call nk_GetBtsDataByDateRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetNokiaBtsDataByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"nk_GetBtsDataByDateRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("nk_ExportBtsDataToExcel")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk_ExportBtsDataToExcel([FromBody] ExportBtsDataRequest request)
        {
            try
            {
                _logger.LogInformation($"Call nk_ExportBtsDataToExcel params: (fromDate = {request.FromDate}, toDate = {request.ToDate}, vendor = {request.Vendor})");
                
                // Parse ngày
                DateTime fromDate = DateTime.Parse(request.FromDate);
                DateTime toDate = DateTime.Parse(request.ToDate);

                // Truy vấn dữ liệu Nokia 4G
                var items = await _service.Rnoc_R009.GetNokiaBtsDataByDateRangeAsync(fromDate, toDate);

                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }

                // Tạo CSV thay vì Excel để tránh lỗi Gdip
                var csvContent = new System.Text.StringBuilder();
                
                // Header for Nokia 4G
                csvContent.AppendLine("TT,ID BTS,LN Cells MO ID,Administrative State,Physical Cell ID,TAC,Cell Name,LCR ID,ENB Name,MR BTS Name,EARFCN UL,EARFCN DL,Root Sequence Index,DL Channel BW,UL Channel BW,Channel BW,Direction,Create Date");
                
                // Data
                int stt = 1;
                foreach (var item in items)
                {
                    csvContent.AppendLine($"{stt}," +
                        $"\"{item.IdBts}\"," +
                        $"\"{item.LncellsMoId}\"," +
                        $"\"{item.AdministrativeState}\"," +
                        $"\"{item.Phycellid}\"," +
                        $"\"{item.Tac}\"," +
                        $"\"{item.CellName}\"," +
                        $"\"{item.Lcrid}\"," +
                        $"\"{item.EnbName}\"," +
                        $"\"{item.MrbtsName}\"," +
                        $"\"{item.EarfcnUl}\"," +
                        $"\"{item.EarfcnDl}\"," +
                        $"\"{item.RootSeqIndex}\"," +
                        $"\"{item.DlChBw}\"," +
                        $"\"{item.UlChBw}\"," +
                        $"\"{item.ChBw}\"," +
                        $"\"{item.Direction}\"," +
                        $"\"{item.CreateDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                    stt++;
                }
                
                var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent.ToString());
                return ResponseMessage.Success(Convert.ToBase64String(csvBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("nk_ExportBtsDataToExcel : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        // Nokia 5G endpoints
        [HttpGet("nk5G_GetBtsDataByDate")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk5G_GetBtsDataByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call nk5G_GetBtsDataByDate params: (date = {date:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetNokiaBtsData5GByDateAsync(date);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"nk5G_GetBtsDataByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("nk5G_GetBtsDataByDateRange")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk5G_GetBtsDataByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call nk5G_GetBtsDataByDateRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetNokiaBtsData5GByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"nk5G_GetBtsDataByDateRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("nk5G_ExportBtsDataToExcel")]
        [AuthorizeFilter]
        public async Task<IActionResult> nk5G_ExportBtsDataToExcel([FromBody] ExportBtsDataRequest request)
        {
            try
            {
                _logger.LogInformation($"Call nk5G_ExportBtsDataToExcel params: (fromDate = {request.FromDate}, toDate = {request.ToDate}, vendor = {request.Vendor})");
                
                // Parse ngày
                DateTime fromDate = DateTime.Parse(request.FromDate);
                DateTime toDate = DateTime.Parse(request.ToDate);

                // Truy vấn dữ liệu Nokia 5G
                var items = await _service.Rnoc_R009.GetNokiaBtsData5GByDateRangeAsync(fromDate, toDate);

                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }

                // Tạo CSV thay vì Excel để tránh lỗi Gdip
                var csvContent = new System.Text.StringBuilder();
                
                // Header for Nokia 5G
                csvContent.AppendLine("TT,ID BTS,NR Cell MO ID,Cell Technology,Cell Dep Type,Cell Name,Physical Cell ID,LCR ID,PRACH Root Sequence Index,Channel BW,NR ARFCN,Administrative State,Basic Beam Set,NR BTS MO ID,NR BTS Name,MR BTS MO ID,MR BTS Name,Direction,Create Date");
                
                // Data
                int stt = 1;
                foreach (var item in items)
                {
                    csvContent.AppendLine($"{stt}," +
                        $"\"{item.IdBts}\"," +
                        $"\"{item.NrCellMoId}\"," +
                        $"\"{item.CellTechnology}\"," +
                        $"\"{item.CellDepType}\"," +
                        $"\"{item.CellName}\"," +
                        $"\"{item.PhysCellId}\"," +
                        $"\"{item.Lcrid}\"," +
                        $"\"{item.PrachRootSequenceIndex}\"," +
                        $"\"{item.ChBw}\"," +
                        $"\"{item.NrArfcn}\"," +
                        $"\"{item.AdministrativeState}\"," +
                        $"\"{item.BasicBeamSet}\"," +
                        $"\"{item.NrBtsMoId}\"," +
                        $"\"{item.NrBtsName}\"," +
                        $"\"{item.MrBtsMoId}\"," +
                        $"\"{item.MrBtsName}\"," +
                        $"\"{item.Direction}\"," +
                        $"\"{item.CreateDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                    stt++;
                }
                
                var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent.ToString());
                return ResponseMessage.Success(Convert.ToBase64String(csvBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("nk5G_ExportBtsDataToExcel : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        // ZTE endpoints
        [HttpGet("zte_GetBtsDataByDate")]
        [AuthorizeFilter]
        public async Task<IActionResult> zte_GetBtsDataByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call zte_GetBtsDataByDate params: (date = {date:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetZteBtsDataByDateAsync(date);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"zte_GetBtsDataByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("zte_GetBtsDataByDateRange")]
        [AuthorizeFilter]
        public async Task<IActionResult> zte_GetBtsDataByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call zte_GetBtsDataByDateRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetZteBtsDataByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"zte_GetBtsDataByDateRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("zte_ExportBtsDataToExcel")]
        [AuthorizeFilter]
        public async Task<IActionResult> zte_ExportBtsDataToExcel([FromBody] ExportBtsDataRequest request)
        {
            try
            {
                _logger.LogInformation($"Call zte_ExportBtsDataToExcel params: (fromDate = {request.FromDate}, toDate = {request.ToDate}, vendor = {request.Vendor})");
                
                // Parse ngày
                DateTime fromDate = DateTime.Parse(request.FromDate);
                DateTime toDate = DateTime.Parse(request.ToDate);

                // Truy vấn dữ liệu ZTE
                var items = await _service.Rnoc_R009.GetZteBtsDataByDateRangeAsync(fromDate, toDate);

                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }

                // Tạo CSV thay vì Excel để tránh lỗi Gdip
                var csvContent = new System.Text.StringBuilder();
                
                // Header for ZTE
                csvContent.AppendLine("TT,Technology,Cell Name,TAC,Physical Cell ID,LCR ID,UL EARFCN,DL EARFCN,Cell Type,Cell Remote,RSI Decimal,Bandwidth,MIMO,eNodeB ID,Province Code,District Code,NET,eNodeB Name,NE Name,Admin State,Device Type,Created Date");
                
                // Data
                int stt = 1;
                foreach (var item in items)
                {
                    csvContent.AppendLine($"{stt}," +
                        $"\"{item.Technology}\"," +
                        $"\"{item.Cellname}\"," +
                        $"{item.TAC}," +
                        $"{item.PhyCellId}," +
                        $"{item.LcrId}," +
                        $"{item.ULEARFCN}," +
                        $"{item.DLEARFCN}," +
                        $"\"{item.CellType}\"," +
                        $"\"{item.Cellremote}\"," +
                        $"{item.RSI_Decimal}," +
                        $"\"{item.Bandwidth}\"," +
                        $"\"{item.MIMO}\"," +
                        $"\"{item.ENodeBID}\"," +
                        $"\"{item.Provincecode}\"," +
                        $"\"{item.Districtcode}\"," +
                        $"\"{item.NET}\"," +
                        $"\"{item.ENodeB_Name}\"," +
                        $"\"{item.NE_Name}\"," +
                        $"\"{item.AdminState}\"," +
                        $"\"{item.DeviceType}\"," +
                        $"\"{item.CreatedDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"");
                    stt++;
                }
                
                var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent.ToString());
                return ResponseMessage.Success(Convert.ToBase64String(csvBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("zte_ExportBtsDataToExcel : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Ericsson endpoints
        [HttpGet("ericsson_GetBtsDataByDate")]
        [AuthorizeFilter]
        public async Task<IActionResult> ericsson_GetBtsDataByDate([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call ericsson_GetBtsDataByDate params: (date = {date:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetEricssonBtsDataByDateAsync(date);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ericsson_GetBtsDataByDate : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("ericsson_GetBtsDataByDateRange")]
        [AuthorizeFilter]
        public async Task<IActionResult> ericsson_GetBtsDataByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Call ericsson_GetBtsDataByDateRange params: (startDate = {startDate:yyyy-MM-dd}, endDate = {endDate:yyyy-MM-dd})");
                var items = await _service.Rnoc_R009.GetEricssonBtsDataByDateRangeAsync(startDate, endDate);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ericsson_GetBtsDataByDateRange : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("ericsson_ExportBtsDataToExcel")]
        [AuthorizeFilter]
        public async Task<IActionResult> ericsson_ExportBtsDataToExcel([FromBody] ExportBtsDataRequest request)
        {
            try
            {
                _logger.LogInformation($"Call ericsson_ExportBtsDataToExcel params: (fromDate = {request.FromDate}, toDate = {request.ToDate}, vendor = {request.Vendor})");
                
                // Parse ngày
                DateTime fromDate = DateTime.Parse(request.FromDate);
                DateTime toDate = DateTime.Parse(request.ToDate);

                // Truy vấn dữ liệu Ericsson
                var items = await _service.Rnoc_R009.GetEricssonBtsDataByDateRangeAsync(fromDate, toDate);

                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }

                // Tạo CSV thay vì Excel để tránh lỗi Gdip
                var csvContent = new System.Text.StringBuilder();
                
                // Header for Ericsson
                csvContent.AppendLine("TT,Cell Name,eNodeB ID,eNodeB Name,LCR ID,TAC,UL EARFCN,DL EARFCN,Physical Cell ID,Bandwidth DL,Cell Type,Admin State,Technology,Province Code,District Code,Created Date,RSI,MIMO,Bandwidth UL");
                
                // Data
                int stt = 1;
                foreach (var item in items)
                {
                    csvContent.AppendLine($"{stt}," +
                        $"\"{item.Cellname}\"," +
                        $"\"{item.ENodeBID}\"," +
                        $"\"{item.ENodeB_Name}\"," +
                        $"{item.LcrId}," +
                        $"{item.TAC}," +
                        $"{item.ULEARFCN}," +
                        $"{item.DLEARFCN}," +
                        $"{item.PhyCellId}," +
                        $"\"{item.BandwidthDL}\"," +
                        $"\"{item.CellType}\"," +
                        $"\"{item.AdminState}\"," +
                        $"\"{item.Technology}\"," +
                        $"\"{item.Provincecode}\"," +
                        $"\"{item.Districtcode}\"," +
                        $"\"{item.CreatedDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"," +
                        $"{item.RSI}," +
                        $"\"{item.MIMO}\"," +
                        $"\"{item.BandwidthUL}\"");
                    stt++;
                }
                
                var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent.ToString());
                return ResponseMessage.Success(Convert.ToBase64String(csvBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ericsson_ExportBtsDataToExcel : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Dashboard 4G API
        [HttpGet("dashboard4g")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboard4GData([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call GetDashboard4GData params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R009.GetDashboard4GDataAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDashboard4GData : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Dashboard 5G API
        [HttpGet("dashboard5g")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetDashboard5GData([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call GetDashboard5GData params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R009.GetDashboard5GDataAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDashboard5GData : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Provincial Report 4G API
        [HttpGet("provincial4g")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetProvincialReport4G([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call GetProvincialReport4G params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R009.GetProvincialReport4GAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProvincialReport4G : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Provincial Report 5G API
        [HttpGet("provincial5g")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetProvincialReport5G([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call GetProvincialReport5G params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R009.GetProvincialReport5GAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProvincialReport5G : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }

        // Provincial Report All API
        [HttpGet("provincialAll")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetProvincialReportAll([FromQuery] DateTime date)
        {
            try
            {
                _logger.LogInformation($"Call GetProvincialReportAll params: (date = {date:yyyy-MM-dd})");
                var result = await _service.Rnoc_R009.GetProvincialReportAllAsync(date);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProvincialReportAll : {ex.Message}");
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
} 