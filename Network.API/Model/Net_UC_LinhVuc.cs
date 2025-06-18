using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_UC_LinhVuc")]
    public class Net_UC_LinhVuc : AuditEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        // Có thể thêm mô tả nếu cần
    }
}