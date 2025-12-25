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
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using Network.API.ViewModel.Speed_DataOkla;
using System.Text;
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Utilities;

namespace Network.API.Controllers
{
    public class Speed_DataOklaController : ApiControllerBase<Speed_DataOkla>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Speed_DataOklaController> _logger;
        public Speed_DataOklaController(IServiceWrapper service, ILogger<Speed_DataOklaController> logger) : base(service, logger)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost("TraCuuDuLieu")]
        [AuthorizeFilter]
        public async Task<IActionResult> TraCuuDuLieu([FromBody] RqTraCuuDuLieu model)
        {
            try
            {
                _logger.LogInformation("Call TraCuuDuLieu");
                var items = await _service.Speed_DataOkla.TraCuuDuLieu(model);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("TraCuuDuLieu : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("ListKhuVuc")]
        [AuthorizeFilter]
        public async Task<IActionResult> ListKhuVuc()
        {
            try
            {
                _logger.LogInformation("Call ListKhuVuc");
                var items = await _service.Speed_DataOkla.ListKhuVucAsync();
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ListKhuVuc : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("AutomationChartDownload")]
        [AuthorizeFilter]
        public async Task<IActionResult> AutomationChartDownload([FromBody] RqAutomationChartDownload model)
        {
            try
            {
                _logger.LogInformation("Call AutomationChartDownload");
                var items = await _service.Speed_DataOkla.AutomationChartDownload(model);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("AutomationChartDownload : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("AutomationChartUpload")]
        [AuthorizeFilter]
        public async Task<IActionResult> AutomationChartUpload([FromBody] RqAutomationChartUpload model)
        {
            try
            {
                _logger.LogInformation("Call AutomationChartUpload");
                var items = await _service.Speed_DataOkla.AutomationChartUpload(model);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("AutomationChartUpload : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("ExportTraCuuDuLieu")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportTraCuuDuLieu([FromBody] RqTraCuuDuLieu model)
        {
            try
            {
                _logger.LogInformation("Call ExportTraCuuDuLieu");
                var items = await _service.Speed_DataOkla.TraCuuDuLieu(model);
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
                workSheet.Cells[1, 2].Value = "Ngày đo";
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
                return Ok(Convert.ToBase64String(excel.GetAsByteArray()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ExportTraCuuDuLieu : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("ExportMauTestCBNV")]
        [AuthorizeFilter]
        public async Task<IActionResult> ExportMauTestCBNV([FromBody] Rq_ExportMauTestCBNV model)
        {
            try
            {
                var items = await _service.Speed_DataOkla.ExportMauTestCBNV(model);
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
                workSheet.Cells[1, 1].Value = "Họ và tên";
                workSheet.Cells[1, 2].Value = "Đơn vị";
                workSheet.Cells[1, 3].Value = "SĐT";
                workSheet.Cells[1, 4].Value = "Email";
                workSheet.Cells[1, 5].Value = "Id Result";
                workSheet.Cells[1, 6].Value = "Id Device";
                workSheet.Cells[1, 7].Value = "Val Download Kbps";
                workSheet.Cells[1, 8].Value = "Val Upload Kbps";
                workSheet.Cells[1, 9].Value = "Mẫu hợp lệ";
                workSheet.Cells[1, 10].Value = "Attr Location Latitude";
                workSheet.Cells[1, 11].Value = "Attr Location intitude";
                workSheet.Cells[1, 12].Value = "Attr connection type start";
                workSheet.Cells[1, 13].Value = "Attr connection type end";
                workSheet.Cells[1, 14].Value = "Date Result";
                //Body of table  
                int recordIndex = 2;
                foreach (var item in items)
                {
                    workSheet.Cells[recordIndex, 1].Value = item.ho_va_ten;
                    workSheet.Cells[recordIndex, 2].Value = item.don_vi;
                    workSheet.Cells[recordIndex, 3].Value = item.so_dien_thoai;
                    workSheet.Cells[recordIndex, 4].Value = item.email;
                    workSheet.Cells[recordIndex, 5].Value = item.id_result;
                    workSheet.Cells[recordIndex, 6].Value = item.id_device;
                    workSheet.Cells[recordIndex, 7].Value = item.val_download_kbps;
                    workSheet.Cells[recordIndex, 8].Value = item.val_upload_kbps;
                    workSheet.Cells[recordIndex, 9].Value = item.is_portal_included;
                    workSheet.Cells[recordIndex, 10].Value = item.attr_location_latitude;
                    workSheet.Cells[recordIndex, 11].Value = item.attr_location_intitude;
                    workSheet.Cells[recordIndex, 12].Value = item.attr_connection_type_start_string;
                    workSheet.Cells[recordIndex, 13].Value = item.attr_connection_type_end_string;
                    workSheet.Cells[recordIndex, 14].Value = item.ts_result;
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
                _logger.LogError(string.Format("ExportMauTestCBNV : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("ImportFileMnpAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportFileMnpAsync(IFormFile uploadedFile)
        {
            try
            {
                string Fulltext = string.Empty;
                List<string> headers = new List<string>();
                Speed_DataOkla item = new Speed_DataOkla();
                var UserName = string.Empty;
                var CreatedDateTime = DateTime.Now;
                foreach (var file in Request.Form.Files)
                {
                    bool IsExists = await _service.Speed_FileImport.IsExistsFileImport(file.FileName);
                    if (IsExists)
                    {
                        throw new Exception("File : " + file.FileName + " đã tồn tại");
                    }
                    List<Speed_DataOkla> data = new List<Speed_DataOkla>();
                    Speed_FileImport fileImport = new Speed_FileImport();
                    fileImport.Name = file.FileName;
                    fileImport.FileLength = file.Length;
                    int i = 0;
                    using var fileStream = file.OpenReadStream();
                    using (TextFieldParser parser = new TextFieldParser(fileStream))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            //Process row
                            item = new Speed_DataOkla();
                            item.FileImport = fileImport.Name;
                            string[] rowValues = parser.ReadFields();
                            item.Id = Guid.NewGuid();
                            item.CreatedBy = UserName;
                            item.CreatedDateTime = CreatedDateTime;
                            item.id_result = rowValues[0];
                            item.guid_result = rowValues[1];
                            item.id_platform = rowValues[2];
                            DateTime.TryParse(rowValues[3], out DateTime ts_result);
                            DateTime.TryParse(rowValues[4], out DateTime ts_result_received);
                            if(ts_result != DateTime.MinValue)
                            {
                                item.ts_result = new DateTimeOffset(ts_result);
                            }
                            if (ts_result_received != DateTime.MinValue)
                            {
                                item.ts_result_received = new DateTimeOffset(ts_result_received);
                            }
                            item.attr_location_timezone = rowValues[5];
                            item.id_device = rowValues[6];
                            item.attr_device_android_fingerprint = rowValues[7];
                            item.attr_device_model = rowValues[8];
                            item.attr_device_manufacturer = rowValues[9];
                            item.attr_device_model_raw = rowValues[10];
                            item.attr_device_manufacturer_raw = rowValues[11];
                            item.attr_device_brand_raw = rowValues[12];
                            item.attr_device_chipset = rowValues[13];
                            item.attr_device_chipset_manufacturer = rowValues[14];
                            item.attr_device_hardware_name = rowValues[15];
                            item.attr_device_os_version = rowValues[16];
                            item.attr_device_build = rowValues[17];
                            item.is_device_rooted = rowValues[18];
                            item.attr_device_radio = rowValues[19];
                            item.attr_device_ram_mb = rowValues[20];
                            item.attr_device_storage_mb = rowValues[21];
                            item.is_device_world_phone = rowValues[22];
                            item.attr_device_multi_sim_support = rowValues[23];
                            item.num_device_active_modems = rowValues[24];
                            item.num_device_supported_modems = rowValues[25];
                            item.is_device_concurrent_voice_data_supported = rowValues[26];
                            item.is_device_data_connection_allowed = rowValues[27];
                            item.is_device_data_capable = rowValues[28];
                            item.is_device_data_roaming_enabled = rowValues[29];
                            item.is_device_icc_card_present = rowValues[30];
                            item.attr_device_service_state = rowValues[31];
                            item.attr_device_thermal_status = rowValues[32];
                            item.val_device_thermal_headroom = rowValues[33];
                            item.is_app_permission_phone_state = rowValues[34];
                            item.is_app_permission_fine_location = rowValues[35];
                            item.is_app_permission_coarse_location = rowValues[36];
                            item.is_app_permission_background_location = rowValues[37];
                            item.is_app_permission_wifi_state = rowValues[38];
                            item.attr_sim_operator_common_name = rowValues[39];
                            item.attr_sim_operator_name_raw = rowValues[40];
                            item.attr_sim_operator_mcc = rowValues[41];
                            item.attr_sim_operator_mnc = rowValues[42];
                            item.attr_altsim_operator_name = rowValues[43];
                            item.attr_altsim_operator_mcc = rowValues[44];
                            item.attr_altsim_operator_mnc = rowValues[45];
                            item.attr_network_operator_mcc = rowValues[46];
                            item.attr_network_operator_mnc = rowValues[47];
                            item.attr_network_operator_common_name = rowValues[48];
                            item.attr_isp_common_name = rowValues[49];
                            item.attr_isp_name_raw = rowValues[50];
                            item.attr_sim_type_allocation_code = rowValues[51];
                            item.attr_sim_state = rowValues[52];
                            item.attr_test_method = rowValues[53];
                            item.attr_test_ip_version = rowValues[54];
                            item.id_connection_type_start = rowValues[55];
                            item.id_connection_type_end = rowValues[56];
                            item.num_connections_failed = rowValues[57];
                            item.is_connection_carrier_aggregation = rowValues[58];
                            item.attr_connection_nr_state = rowValues[59];
                            item.attr_connection_apn = rowValues[60];
                            item.id_connection_net_speed = rowValues[61];
                            item.is_connection_access_technology_nr = rowValues[62];
                            item.id_connection_network_override_type = rowValues[63];
                            item.attr_connection_downstream_bandwidth_kbps = rowValues[64];
                            item.attr_connection_upstream_bandwidth_kbps = rowValues[65];
                            item.attr_connection_nat64_prefix = rowValues[66];
                            item.attr_location_latitude = rowValues[67];
                            item.attr_location_longitude = rowValues[68];
                            item.attr_location_start_latitude = rowValues[69];
                            item.attr_location_start_longitude = rowValues[70];
                            item.id_location_start_type = rowValues[71];
                            item.id_location_end_type = rowValues[72];
                            item.attr_location_accuracy_m = rowValues[73];
                            item.attr_location_age_ms = rowValues[74];
                            item.attr_location_altitude_m = rowValues[75];
                            item.attr_location_vertical_accuracy_m = rowValues[76];
                            item.attr_location_speed_mps = rowValues[77];
                            item.attr_place_formatted_address = rowValues[78];
                            item.attr_place_name = rowValues[79];
                            item.attr_place_locality_type = rowValues[80];
                            item.attr_place_country = rowValues[81];
                            item.attr_place_country_code = rowValues[82];
                            item.attr_place_region = rowValues[83];
                            item.attr_place_subregion = rowValues[84];
                            item.attr_place_subsubregion = rowValues[85];
                            item.attr_place_postal_code = rowValues[86];
                            item.num_packet_loss_sent = rowValues[87];
                            item.num_packet_loss_received = rowValues[88];
                            item.metric_packet_loss_percent = rowValues[89];
                            item.is_download_stopped = rowValues[90];
                            item.val_latency_min_ms = rowValues[91];
                            item.val_latency_iqm_ms = rowValues[92];
                            item.val_latency_max_ms = rowValues[93];
                            item.val_multiserver_latency_ms = rowValues[94];
                            item.val_download_latency_min_ms = rowValues[95];
                            item.val_download_latency_iqm_ms = rowValues[96];
                            item.val_download_latency_max_ms = rowValues[97];
                            item.val_upload_latency_min_ms = rowValues[98];
                            item.val_upload_latency_iqm_ms = rowValues[99];
                            item.val_upload_latency_max_ms = rowValues[100];
                            item.num_traceroute_hops = rowValues[101];
                            item.attr_traceroute0_ip_address = rowValues[102];
                            item.val_traceroute0_latency_ms = rowValues[103];
                            item.attr_traceroute1_ip_address = rowValues[104];
                            item.val_traceroute1_latency_ms = rowValues[105];
                            item.val_jitter_ms = rowValues[106];
                            item.val_multiserver_jitter_ms = rowValues[107];
                            int.TryParse(rowValues[108], out int val_download_kbps);
                            item.val_download_kbps = val_download_kbps;
                            item.val_test_download_kb = rowValues[109];
                            item.num_test_download_threads = rowValues[110];
                            item.val_test_download_duration_ms = rowValues[111];
                            int.TryParse(rowValues[112], out int val_upload_kbps);
                            item.val_upload_kbps = val_upload_kbps;
                            item.val_test_upload_kb = rowValues[113];
                            item.num_test_upload_threads = rowValues[114];
                            item.val_test_upload_duration_ms = rowValues[115];
                            item.attr_network_ipv4_address = rowValues[116];
                            item.attr_network_ipv6_address = rowValues[117];
                            item.attr_network_asn = rowValues[118];
                            item.attr_app_version = rowValues[119];
                            item.attr_app_store = rowValues[120];
                            item.attr_server_name = rowValues[121];
                            item.attr_server_sponsor_name = rowValues[122];
                            item.attr_server_latitude = rowValues[123];
                            item.attr_server_longitude = rowValues[124];
                            item.val_server_distance_km = rowValues[125];
                            item.attr_server_country = rowValues[126];
                            item.attr_server_country_code = rowValues[127];
                            item.is_server_auto_selected = rowValues[128];
                            item.is_server_on_network = rowValues[129];
                            item.attr_server_asn = rowValues[130];
                            item.num_server_download = rowValues[131];
                            item.val_signal_rsrp_dbm = rowValues[132];
                            item.val_signal_csi_rsrp_dbm = rowValues[133];
                            item.val_signal_ss_rsrp_dbm = rowValues[134];
                            item.val_signal_rsrq_db = rowValues[135];
                            item.val_signal_csi_rsrq_db = rowValues[136];
                            item.val_signal_ss_rsrq_db = rowValues[137];
                            item.val_signal_rssnr_db = rowValues[138];
                            item.val_signal_csi_snr_db = rowValues[139];
                            item.val_signal_ss_snr_db = rowValues[140];
                            item.val_signal_wcdma_ecno_db = rowValues[141];
                            item.val_signal_rssi_dbm = rowValues[142];
                            item.val_signal_gsm_rssi_dbm = rowValues[143];
                            item.val_signal_timing_advance_ts = rowValues[144];
                            item.val_signal_cqi = rowValues[145];
                            item.attr_cell_nr_frequency_range = rowValues[146];
                            item.attr_cell_bandwidth_khz = rowValues[147];
                            item.attr_cell_bandwidths_khz = rowValues[148];
                            item.id_cell_primary = rowValues[149];
                            item.id_cell_lte_enodeb = rowValues[150];
                            item.attr_cell_pci = rowValues[151];
                            item.attr_cell_nr_pci = rowValues[152];
                            item.attr_cell_tac = rowValues[153];
                            item.attr_cell_lac = rowValues[154];
                            item.attr_cell_psc = rowValues[155];
                            item.attr_cell_frequency_channel = rowValues[156];
                            item.attr_cell_frequency_channel_type = rowValues[157];
                            item.attr_cell_nr_arfcn = rowValues[158];
                            item.attr_cell_lte_bands = rowValues[159];
                            item.attr_cell_nr_bands = rowValues[160];
                            item.is_network_roaming = rowValues[161];
                            item.is_network_international_roaming = rowValues[162];
                            item.is_network_vpn = rowValues[163];
                            item.is_device_5g_capable = rowValues[164];
                            item.is_portal_included = rowValues[165];
                            item.attr_portal_categories = rowValues[166];
                            item.attr_connection_type_start_string = rowValues[167];
                            item.attr_connection_type_end_string = rowValues[168];
                            item.attr_device_esim_embedded = rowValues[169];
                            item.id_cell_nr = rowValues[170];
                            item.id_cell_start = rowValues[171];
                            item.attr_network_operator_mcc_nr = rowValues[172];
                            item.attr_network_operator_mnc_nr = rowValues[173];
                            if (i != 0)
                            {
                                data.Add(item);
                            }
                            i++;
                        }
                    }
                    var issuccess = await _service.Speed_DataOkla.BulkInsertAsync(data);
                    if (issuccess)
                    {
                        await _service.Speed_FileImport.SaveEntityAsync(fileImport);
                    }                
                }
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Import : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
