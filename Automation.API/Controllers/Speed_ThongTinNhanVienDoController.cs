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
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static Network.Core.Constant.Sys_Const;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Drawing;
using System.Drawing.Printing;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace Network.API.Controllers
{
    public class Speed_ThongTinNhanVienDoController : ApiControllerBase<Speed_ThongTinNhanVienDo>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Speed_ThongTinNhanVienDoController> _logger;
        public Speed_ThongTinNhanVienDoController(IServiceWrapper service, ILogger<Speed_ThongTinNhanVienDoController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet("ListDonVi")]
        [AuthorizeFilter]
        public async Task<IActionResult> ListDonViAsync()
        {
            try
            {
                var items = await _service.Speed_ThongTinNhanVienDo.ListDonViAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Create : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("{page}/{pageSize}/{totalLimitItems}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetListPaged(int page = 1, int pageSize = 10, int totalLimitItems = 500, string donvi = "all")
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetListPaged params: (page = {0}, pageSize = {1}, totalLimitItems = {2})", page, pageSize, totalLimitItems));
                var items = await _service.Speed_ThongTinNhanVienDo.GetPagedAsync(page, pageSize, totalLimitItems, "", donvi);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetListPaged : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("Export")]
        [AuthorizeFilter]
        public async Task<IActionResult> Export()
        {
            try
            {
                var items = await _service.Speed_ThongTinNhanVienDo.GetPagedAsync(1, 10000, 10000, "");
                if (items.Items.Count() == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "Họ và tên";
                workSheet.Cells[1, 2].Value = "Đơn vị";
                workSheet.Cells[1, 3].Value = "SĐT";
                workSheet.Cells[1, 4].Value = "Email";
                workSheet.Cells[1, 5].Value = "Id Result";
                workSheet.Cells[1, 6].Value = "Id Device";
                workSheet.Cells[1, 7].Value = "Ngày Import";
                //Body of table 
                int recordIndex = 2;
                foreach (var item in items.Items)
                {
                    workSheet.Cells[recordIndex, 1].Value = item.ho_va_ten;
                    workSheet.Cells[recordIndex, 2].Value = item.don_vi;
                    workSheet.Cells[recordIndex, 3].Value = item.so_dien_thoai;
                    workSheet.Cells[recordIndex, 4].Value = item.email;
                    workSheet.Cells[recordIndex, 5].Value = item.id_result;
                    workSheet.Cells[recordIndex, 6].Value = item.id_device;
                    workSheet.Cells[recordIndex, 7].Value = item.ngay_test.ToString("dd/MM/yyyy");
                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                return Ok(Convert.ToBase64String(excel.GetAsByteArray()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Export : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizeFilter]
        public override async Task<IActionResult> Create([FromBody] Speed_ThongTinNhanVienDo model)
        {
            try
            {
                var IsExists = await _service.Speed_ThongTinNhanVienDo.IsExistsThongTinNhanVienDoCreate(model.ho_va_ten, model.don_vi, model.so_dien_thoai);
                if (IsExists)
                {
                    throw new Exception("Trùng ho_va_ten, don_vi, so_dien_thoai");
                }
                _logger.LogInformation(string.Format("Call Create body: ({0})", JsonConvert.SerializeObject(model)));
                var item = await _service.Speed_ThongTinNhanVienDo.SaveEntityAsync(model);
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
        public override async Task<IActionResult> Update([FromBody] Speed_ThongTinNhanVienDo model)
        {
            try
            {
                var IsExists = await _service.Speed_ThongTinNhanVienDo.IsExistsThongTinNhanVienDoUpdate(model.ho_va_ten, model.don_vi, model.so_dien_thoai);
                if (IsExists)
                {
                    throw new Exception("Trùng ho_va_ten, don_vi, so_dien_thoai");
                }
                _logger.LogInformation(string.Format("Call Update body: ({0})", JsonConvert.SerializeObject(model)));
                var item = await _service.Speed_ThongTinNhanVienDo.SaveEntityAsync(model);
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Update : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        
        [HttpGet("ThongKeTheoNhanVien/{id}/{month}/{year}")]
        [AuthorizeFilter]
        public async Task<IActionResult> ThongKeTheoNhanVien(Guid id, int month = 0, int year = 0)
        {
            try
            {
                var item = await _service.Speed_ThongTinNhanVienDo.ThongKeTheoNhanVien(id, month, year);
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeTheoNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("ThongKeTheoNhanVienChuaDat/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> ThongKeTheoNhanVienChuaDat(int month = 0, int year = 0, string donvi = "all")
        {
            try
            {
                var item = await _service.Speed_ThongTinNhanVienDo.ThongKeTheoNhanVienChuaDat(month, year, donvi);
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeTheoNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("ThongKeTheoNhanVienChuaDatTuNgayDenNgay")]
        [AllowAnonymous]
        public async Task<IActionResult> ThongKeTheoNhanVienChuaDatTuNgayDenNgay(string type, string tuNgay, string deNgay, string donvi = "all")
        {
            try
            {
                if(type != "auto_send_email")
                {
                    return ResponseMessage.Success();
                }    
                var item = await _service.Speed_ThongTinNhanVienDo.ThongKeTheoNhanVienChuaDatTuNgayDenNgay(tuNgay, deNgay, donvi);
                return ResponseMessage.Success(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeTheoNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("ExportThongKeTheoNhanVienChuaDat/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportThongKeTheoNhanVienChuaDat(int month = 0, int year = 0, string donvi = "all")
        {
            try
            {
                var items = await _service.Speed_ThongTinNhanVienDo.ThongKeTheoNhanVienChuaDat(month, year, donvi);
                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "Nhân viên";
                workSheet.Cells[1, 2].Value = "Đơn vị";
                workSheet.Cells[1, 3].Value = "Số điện thoại";
                workSheet.Cells[1, 4].Value = "Email";
                workSheet.Cells[1, 5].Value = "Tốc độ Download trung vị Mbps";
                workSheet.Cells[1, 6].Value = "Tốc độ Upload trung vị Mbps";
                workSheet.Cells[1, 7].Value = "Máy hỗ trợ 5G";
                workSheet.Cells[1, 8].Value = "Máy bị root/ Jailbreak";
                workSheet.Cells[1, 9].Value = "Chưa đạt";
                //Body of table  
                int recordIndex = 2;
                foreach (var item in items)
                {
                    workSheet.Cells[recordIndex, 1].Value = item.NhanVien;
                    workSheet.Cells[recordIndex, 2].Value = item.donvi;
                    workSheet.Cells[recordIndex, 3].Value = item.SoDT;
                    workSheet.Cells[recordIndex, 4].Value = item.Email;
                    workSheet.Cells[recordIndex, 5].Value = item.Val_download_kbps == 0 ? "" : ((double)item.Val_download_kbps / 1000.0).ToString("0.000");
                    workSheet.Cells[recordIndex, 6].Value = item.Val_upload_kbps == 0 ? "" : ((double)item.Val_upload_kbps / 1000.0).ToString("0.000");
                    string is_device_5g_capable = "N/A";
                    if (!string.IsNullOrEmpty(item.is_device_5g_capable))
                    {

                        if (item.is_device_5g_capable.ToLower() == "true")
                        {
                            is_device_5g_capable = "Có";
                        }
                        else if (item.is_device_5g_capable.ToLower() == "false")
                        {
                            is_device_5g_capable = "Không";
                        }
                        else if (item.is_device_5g_capable.ToLower() == "nan")
                        {
                            is_device_5g_capable = "N/A";
                        }
                    }
                    workSheet.Cells[recordIndex, 7].Value = is_device_5g_capable;
                    //
                    string is_device_rooted = "N/A";
                    if (!string.IsNullOrEmpty(item.is_device_rooted))
                    {
                        if (item.is_device_rooted.ToLower() == "true")
                        {
                            is_device_rooted = "Rooted";
                        }
                        else if (item.is_device_rooted.ToLower() == "false")
                        {
                            is_device_rooted = "Nguyên bản";
                        }
                        else if (item.is_device_rooted.ToLower() == "nan")
                        {
                            is_device_rooted = "N/A";
                        }
                    }
                    workSheet.Cells[recordIndex, 8].Value = is_device_rooted;
                    workSheet.Cells[recordIndex, 9].Value = item.trangthai;
                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                return Ok(Convert.ToBase64String(excel.GetAsByteArray()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeTheoNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("ExportListDoKiemTungNhanVien")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportListDoKiemTungNhanVien([FromBody] RqListDoKiemTungNhanVien model)
        {
            try
            {
                var items = await _service.Speed_ThongTinNhanVienDo.ListDoKiemTungNhanVien(model);
                if (items.Count == 0)
                {
                    throw new Exception("Không có dữ liệu !");
                }
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "Nhân viên";
                workSheet.Cells[1, 2].Value = "Đơn vị";
                workSheet.Cells[1, 3].Value = "Ngày đo";
                workSheet.Cells[1, 4].Value = "Số điện thoại";
                workSheet.Cells[1, 5].Value = "Device ID";
                workSheet.Cells[1, 6].Value = "Thiết bị";
                workSheet.Cells[1, 7].Value = "Nhà mạng";
                workSheet.Cells[1, 8].Value = "Địa chỉ";
                workSheet.Cells[1, 9].Value = "Download Mbps";
                workSheet.Cells[1, 10].Value = "Upload Mbps";
                workSheet.Cells[1, 11].Value = "Mẫu hợp lệ";
                workSheet.Cells[1, 12].Value = "Máy hỗ trợ 5G";
                workSheet.Cells[1, 13].Value = "Máy bị root/ Jailbreak";
                workSheet.Cells[1, 13].Value = "Jitter (ms)";
                
                //Body of table  
                int recordIndex = 2;
                foreach (var item in items)
                {
                    workSheet.Cells[recordIndex, 1].Value = item.NhanVien;
                    workSheet.Cells[recordIndex, 2].Value = item.DonVi;
                    workSheet.Cells[recordIndex, 3].Value = item.NgayDo.ToString("dd/MM/yyyy");
                    workSheet.Cells[recordIndex, 4].Value = item.SoDT;
                    workSheet.Cells[recordIndex, 5].Value = item.Device_Id;
                    workSheet.Cells[recordIndex, 6].Value = item.attr_device_model;
                    workSheet.Cells[recordIndex, 7].Value = item.attr_isp_common_name;
                    workSheet.Cells[recordIndex, 8].Value = item.attr_place_region;
                    workSheet.Cells[recordIndex, 9].Value = item.Val_download_kbps == 0 ? "" : ((double)item.Val_download_kbps / 1000.0).ToString("0.000");
                    workSheet.Cells[recordIndex, 10].Value = item.Val_upload_kbps == 0 ? "" : ((double)item.Val_upload_kbps / 1000.0).ToString("0.000");
                    workSheet.Cells[recordIndex, 11].Value = item.is_portal_included;
                    //
                    string is_device_5g_capable = "N/A";
                    if (!string.IsNullOrEmpty(item.is_device_5g_capable))
                    {
                        if (item.is_device_5g_capable.ToLower() == "true")
                        {
                            is_device_5g_capable = "Có";
                        }
                        else if (item.is_device_5g_capable.ToLower() == "false")
                        {
                            is_device_5g_capable = "Không";
                        }
                        else if (item.is_device_5g_capable.ToLower() == "nan")
                        {
                            is_device_5g_capable = "N/A";
                        }
                    }
                    workSheet.Cells[recordIndex, 12].Value = is_device_5g_capable;
                    //
                    string is_device_rooted = "N/A";
                    if (!string.IsNullOrEmpty(item.is_device_rooted))
                    {
                        if (item.is_device_rooted.ToLower() == "true")
                        {
                            is_device_rooted = "Rooted";
                        }
                        else if (item.is_device_rooted.ToLower() == "false")
                        {
                            is_device_rooted = "Nguyên bản";
                        }
                        else if (item.is_device_rooted.ToLower() == "nan")
                        {
                            is_device_rooted = "N/A";
                        }
                    }
                    workSheet.Cells[recordIndex, 13].Value = is_device_rooted;
                    workSheet.Cells[recordIndex, 14].Value = item.val_jitter_ms;
                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                workSheet.Column(10).AutoFit();
                workSheet.Column(11).AutoFit();
                workSheet.Column(12).AutoFit();
                workSheet.Column(13).AutoFit();
                workSheet.Column(14).AutoFit();
                return Ok(Convert.ToBase64String(excel.GetAsByteArray()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ExportListDoKiemTungNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("ListDoKiemTungNhanVien")]
        [AuthorizeFilter]
        public async Task<IActionResult> ListDoKiemTungNhanVien([FromBody] RqListDoKiemTungNhanVien model)
        {
            try
            {
                _logger.LogInformation("Call ListDoKiemTungNhanVien");
                var items = await _service.Speed_ThongTinNhanVienDo.ListDoKiemTungNhanVien(model);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ListDoKiemTungNhanVien : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("FileTemplate")]
        [AuthorizeFilter]
        public async Task<IActionResult> FileTemplate()
        {
            try
            {
                return ResponseMessage.Success("StaticFiles/Template/dsnhanvien.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("FileTemplate : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("SyncDevice")]
        [AuthorizeFilter]
        public async Task<IActionResult> SyncDevice()
        {
            try
            {
                bool success = await _service.Speed_ThongTinNhanVienDo.SyncDeviceAsync();
                return ResponseMessage.Success(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("SyncDevice : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("Import")]
        [AllowAnonymous]
        public async Task<IActionResult> Import(IFormFile uploadedFile)
        {
            try
            {
                bool IsExists = false;
                foreach (var file in Request.Form.Files)
                {
                    var items = new List<Speed_ThongTinNhanVienDo>();
                    using var fileStream = file.OpenReadStream();
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var reader = ExcelReaderFactory.CreateReader(fileStream))
                    {
                        var dataSet = reader.AsDataSet();
                        for (var i = 0; i < dataSet.Tables.Count; i++)
                        {
                            var dataTable = dataSet.Tables[i];
                            for (var j = 0; j < dataTable.Rows.Count; j++)
                            {
                                try
                                {
                                    if (j >= 1)
                                    {
                                        var item = new Speed_ThongTinNhanVienDo();
                                        item.ho_va_ten = Convert.ToString(dataTable.Rows[j]["Column1"]);
                                        item.don_vi = Convert.ToString(dataTable.Rows[j]["Column2"]);
                                        item.so_dien_thoai = Convert.ToString(dataTable.Rows[j]["Column3"]);
                                        item.email = Convert.ToString(dataTable.Rows[j]["Column4"]);
                                        item.link_ket_qua = Convert.ToString(dataTable.Rows[j]["Column5"]);
                                        Regex regex = new Regex(@"/(\d+)$");
                                        Match match = regex.Match(Convert.ToString(dataTable.Rows[j]["Column5"]));
                                        item.ngay_test = DateTime.Now;
                                        item.id_result = match.Groups[1].Value;
                                        IsExists = await _service.Speed_ThongTinNhanVienDo.IsExistsThongTinNhanVienDoCreate(item.ho_va_ten, item.don_vi, item.so_dien_thoai);
                                        if(!IsExists)
                                        {
                                            items.Add(item);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    if (items.Count > 0)
                    {
                        var fillerDuplicateItems = new List<Speed_ThongTinNhanVienDo>();
                        foreach(var item in items)
                        {
                           // IsExists = items.Any(o => o.ho_va_ten == item.ho_va_ten && o.don_vi == item.don_vi && o.so_dien_thoai == item.so_dien_thoai);
                            //if (!IsExists)
                            //{
                                fillerDuplicateItems.Add(item);
                            //}
                        }    
                        await _service.Speed_ThongTinNhanVienDo.SaveEntitiesAsync(fillerDuplicateItems.ToArray());
                    }
                }
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetByProps : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
