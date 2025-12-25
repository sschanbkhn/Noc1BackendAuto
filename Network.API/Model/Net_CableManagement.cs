using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_CableManagement")]
    public class Net_CableManagement : AuditEntity
    {
        [StringLength(55)]
        public string CableCode { get; set; }

        public Guid? LineId { get; set; }

        [StringLength(55)]
        public string CableType {  get; set; }

        public Guid? HeadDeviceId { get; set; }

        public Guid? LastDeviceId { get; set; }

        [StringLength(55)]  
        public string SetPoint {  get; set; }

        [StringLength(255)]
        public string ManageOrgan { get; set; }

        [StringLength(100)]
        public string ManagerName { get; set; }

        [StringLength(20)]
        public string ManagerTel { get; set; }

        [StringLength(55)]
        public string ManagerEmail { get; set; }
    }
}
