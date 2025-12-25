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
    [Table("Sys_EmailSms")]
    public class Sys_EmailSms : AuditEntity
    {
        [StringLength(50)]
        public string EmailFrom{ get; set; } = "";
        [StringLength(20)]
        public string EmailPort { get; set; } = "";
        [StringLength(50)]
        public string EmailHost{ get; set; } = "";
        [StringLength(100)]
        public string EmailPass { get; set; }
        [StringLength(100)]
        public string SmSUser { get; set; }
        [StringLength(100)]
        public string SmSPass { get; set; }
        [StringLength(100)]
        public string SmSUrl { get; set; }
    }
}
