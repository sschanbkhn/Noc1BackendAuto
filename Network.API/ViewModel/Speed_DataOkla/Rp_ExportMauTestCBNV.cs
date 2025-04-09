using System;
using System.ComponentModel.DataAnnotations;

namespace Network.API.ViewModel.Speed_DataOkla
{
    public class Rp_ExportMauTestCBNV
    {
        public string ho_va_ten { get; set; }
        public string so_dien_thoai { get; set; }
        public string email { get; set; }
        public string id_result { get; set; }
        public string id_device { get; set; }
        public string val_download_kbps { get; set; }
        public string val_test_upload_kb { get; set; }
        public string attr_location_latitude { get; set; }
        public string attr_location_longitude { get; set; }
    }
}
