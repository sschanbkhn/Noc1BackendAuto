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
    [Table("Sys_Configs")]
    public class Sys_Config : AuditEntity
    {
        [StringLength(55)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Value { get; set; }
        public ConfigType Type { get; set; }        
    }
}
