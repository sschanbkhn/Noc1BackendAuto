using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Network.API.Model
{
    [Table("NetUsecase_Run")]
    public class NetUsecase_Run : AuditEntity
    {
        public string UsecaseName { get; set; }
        public Guid LinhVucId { get; set; }
        public Net_UC_LinhVuc Field { get; set; }
        public Guid TrangThaiId { get; set; }
        public Net_UC_TrangThai Status { get; set; }
        public string Result { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? TotalSeconds { get; set; }
    }
}