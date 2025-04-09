using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Model
{
    [Table("Sys_Authtokens")]
    public class Sys_AuthToken : AuditEntity
    {
        [StringLength(55)]
        public string UserName { get; set; }
        [StringLength(2000)]
        public string AccessToken { get; set; }
        [StringLength(2000)]
        public string RefeshToken { get; set; }
    }
}
