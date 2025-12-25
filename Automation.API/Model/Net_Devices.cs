using Network.Core.Core;
using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_Devices")]
    public class Net_Devices : AuditEntity
    {
        [StringLength(55)]
        public string Code { get; set; }

        [StringLength(255)]
        [ColumnNameAttr("category")]
        public string Name { get; set; }

        [StringLength(10)]
        public string Lon { get; set; }

        [StringLength(10)]
        public string Lat { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Guid? DeviceTypeId { get; set; }

        public Guid? ManufacturerId { get; set; }

        [StringLength(10)]
        public string FirmwareVersion { get; set; }

        [StringLength(20)]
        public string IPAddress { get; set; }

        [StringLength(50)]
        public string MACAddress { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        public Guid? OrganId { get; set; }

    }
}
