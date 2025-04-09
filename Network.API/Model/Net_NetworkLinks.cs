using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_NetworkLinks")]
    public class Net_NetworkLinks : AuditEntity
    {
        [StringLength(50)]
        [ColumnNameAttr("category")]
        public string SerialNumber { get; set; }

        public int Distance { get; set; }

        public Guid? HeadDeviceId { get; set; }

        public Guid? LastDeviceId { get; set; }

        public Guid? HeadDevicePortId { get; set; }

        public Guid? LastDevicePortId { get; set; }

        [StringLength(50)]
        public string ConnectType { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int Speed { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(55)]
        public string Status { get; set; }
    }
}
