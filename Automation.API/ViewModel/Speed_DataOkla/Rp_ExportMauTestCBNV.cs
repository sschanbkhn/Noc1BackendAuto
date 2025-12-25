using System;
using System.ComponentModel.DataAnnotations;

namespace Network.API.ViewModel.Speed_DataOkla
{
    public class Rp_ExportMauTestCBNV
    {
        public string ho_va_ten { get; set; }
        public string don_vi { get;set; }
        public string so_dien_thoai { get; set; }
        public string email { get; set; }
        public string id_result { get; set; }
        public string id_device { get; set; }
        public int val_download_kbps { get; set; }
        public int val_upload_kbps { get; set; }
        public string attr_location_latitude { get; set; }
        public string attr_location_intitude { get; set; }
        public DateTimeOffset ts_result { get; set; }
        public string is_portal_included { get; set; }
        public string attr_connection_type_start_string { get; set; }
        public string attr_connection_type_end_string { get; set; }
    }
}
