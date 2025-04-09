using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_AlarmType")]
    public class Net_AlarmType : AuditEntity
    {
        [StringLength(55)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public Guid? LevelId { get; set; }
    }
}
