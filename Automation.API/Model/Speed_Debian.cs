using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    //[Table("Speed_SmsEmail")]
    public class Speed_Debian : AuditEntity
    {
        public string CommonName { get; set; }        // Thông tin về tên nhà mạng
        public string Address { get; set; }            // Địa chỉ
        public string Latitude { get; set; }           // Vĩ độ (latitude)
        public string Longitude { get; set; }          // Kinh độ (longitude)
        public decimal MedianDownloadKbps { get; set; } // Tốc độ tải xuống trung bình (kbps)
        public decimal MedianUploadKbps { get; set; }   // Tốc độ tải lên trung bình (kbps)
        public DateTime TsResultReceived { get; set; }  // Thời gian kết quả nhận được (timestamp with time zone)

    }
}
