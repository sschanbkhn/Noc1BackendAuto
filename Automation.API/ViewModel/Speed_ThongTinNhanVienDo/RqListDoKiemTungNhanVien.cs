using System;

namespace Network.API.ViewModel.Speed_ThongTinNhanVienDo
{
    public class RqListDoKiemTungNhanVien
    {
        public string DonVi { get; set; }
        public string TenNhanVien { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int? Down { get; set; }
        public int? Up { get; set; }
    }
}
