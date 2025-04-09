using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_DevicePorts")]
    public class Net_DevicePorts : AuditEntity
    {
        public Guid? DeviceId { get; set; }

        [StringLength(55)]
        [ColumnNameAttr("category")]
        public string Name { get; set; }

        [StringLength(20)]
        public string SerialPort { get; set; }

        [StringLength(20)]
        public string PortFormat { get; set; }

        [StringLength(55)]
        public string Type { get; set; }

        public int MaxSpeed { get; set; }

        [StringLength(55)]
        public string Status { get; set; }
    }
}
