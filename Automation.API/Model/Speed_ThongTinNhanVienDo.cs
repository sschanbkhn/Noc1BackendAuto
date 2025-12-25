using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Network.API.Model
{
    public class Speed_ThongTinNhanVienDo : AuditEntity
    {
        [StringLength(255)]
        public string ho_va_ten { get; set; }
        [StringLength(255)]
        public string don_vi { get; set; }
        [StringLength(255)]
        public string so_dien_thoai { get; set; }
        [StringLength(255)]
        public string email { get; set; }
        [StringLength(255)]
        public string link_ket_qua { get; set; }
        public DateTimeOffset ngay_test { get; set; }

        [StringLength(255)]
        public string id_result { get; set; }
        [StringLength(255)]
        public string id_device { get; set; }
        [StringLength(500)] 
        public string attr_device_model { get; set; }
        [StringLength(500)]
        public string is_device_5g_capable { get; set; }
        [StringLength(500)]
        public string is_device_rooted { get; set; }
    }
}
