using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_UC_TrangThai")]
    public class Net_UC_TrangThai : AuditEntity
    {
        public string Name { get; set; }
        // Có thể thêm mô tả nếu cần
    }
}