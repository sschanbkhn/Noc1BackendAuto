using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Core;

namespace Network.API.Model
{
    [Table("Sys_Roles")]
    public class Sys_Role : AuditEntity
    {
        [StringLength(55)]
        [ColumnNameAttr("category")]
        public string Code { get; set; }
        [StringLength(55)]
        [ColumnNameAttr("category")]
        public string Name { get; set; }      
    }
}
