using System;

namespace Network.API.ViewModel.Speed_ThongTinNhanVienDo
{
    public class RpListDoKiemTungNhanVien
    {
        public Guid Id { get; set; }
        public string NhanVien { get; set; }
        public string DonVi { get; set; }
        public string SoDT { get; set; }
        public string Device_Id { get; set; }
        public string attr_device_model { get; set; }
        public string attr_isp_common_name { get; set; }
        public string attr_place_region { get; set; }
        public int Val_download_kbps { get; set; }
        public int Val_upload_kbps { get; set; }
        public DateTimeOffset NgayDo { get; set; }
        public string is_device_5g_capable { get; set; }
        public string is_device_rooted { get; set; }
        public string is_portal_included { get; set; }
        public string val_jitter_ms { get; set; }
    }
}
