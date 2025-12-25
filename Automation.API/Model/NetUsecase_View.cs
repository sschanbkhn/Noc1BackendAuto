using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Network.API.Model
{
    public class NetUsecase_View 
    {
         public string SystemName { get; set; }         // Hệ thống
        public string UsecaseType { get; set; }        // Loại Usecase
        public string StatusName { get; set; }         // Trạng thái (từ Net_UC_TrangThai)
        public string Result { get; set; }             // Kết quả
        public DateTime StartTime { get; set; }        // Ngày giờ chạy
        public DateTime? EndTime { get; set; }         // Ngày giờ kết thúc
        public string FieldName { get; set; }   
    }
}