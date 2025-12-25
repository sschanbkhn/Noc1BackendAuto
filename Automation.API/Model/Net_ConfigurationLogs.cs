using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_ConfigurationLogs")]
    public class Net_ConfigurationLogs : AuditEntity
    {
        public Guid? DeviceId { get; set; }

        [StringLength(55)] 
        public string BeforeConfig { get; set; }

        [StringLength(55)]
        public string AfterConfig { get; set; }

        public DateTimeOffset? Time { get; set; }
    }
}
