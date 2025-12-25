using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_HistoryCurenAlarm")]
    public class Net_HistoryCurenAlarm : AuditEntity
    {
        public Guid? AlarmTypeId { get; set; }

        public Guid? DeviceId { get; set; }

        public Guid? LevelId { get; set; }

        public Guid? Status { get; set; }

        public DateTimeOffset? IncidentTime { get; set; }

        public DateTimeOffset? RecoveryTime { get; }

        [StringLength(100)]
        public string AlarmDetail { get; set; }

        [StringLength(55)]
        public string AlarmCode { get; set; }

        [StringLength(255)]
        public string Reason { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(255)]
        public string ProcessedContent { get; set; }

        bool Cleared { get; set; }
    }
}
