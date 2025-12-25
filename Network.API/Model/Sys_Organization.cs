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
    [Table("Sys_Organizations")]
    public class Sys_Organization : AuditEntity
    {
        [StringLength(55)]
        public string Code { get; set; }
        [StringLength(55)]
        public string Name { get; set; }
        public OrganizationType Type { get; set; }
        public Guid ParentId { get; set; } = Guid.Empty;
    }
}
