using Network.Core.Enums;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Model
{
    [Table("Sys_Categories")]
    public class Sys_Category : AuditEntity
    {
        [StringLength(55)]
        public string Code { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public CategoryType Type { get; set; }
        public Guid? ParentId { get; set; }
    }
}
