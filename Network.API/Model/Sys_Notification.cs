using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Network.Core.Models;
using static Network.Core.Enumeration.Sys_Enum;

namespace Network.API.Model
{
    [Table("Sys_Notification")]
    public class Sys_Notification : AuditEntity
    {
        public NotificationType Type { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Content { get; set; }        
        [StringLength(55)]
        public string Receiver { get; set; }
        [StringLength(255)]
        public string DetailsURL { get; set; }
        [StringLength(100)]
        public string ObjectId { get; set; }
        [StringLength(100)]
        public string ObjectType { get; set; }
    }
}
