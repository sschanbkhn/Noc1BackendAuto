using System;

namespace Network.API.ViewModel.Speed_ThongTinNhanVienDo
{
    public class RpThongKeTheoNhanVienChuaDat
    {
        public Guid Id { get; set; }
        public string NhanVien { get; set; }
        public string donvi { get; set; }
        public string SoDT { get; set; }
        public string Email { get; set; }
        public string attr_device_model { get; set; }
        public int Val_download_kbps { get; set; }
        public int Val_upload_kbps { get; set; }
        public string is_device_5g_capable { get; set; }
        public string is_device_rooted { get; set; }
        public string is_portal_included { get; set; }
        public string trangthai { get; set; }
    }
}
