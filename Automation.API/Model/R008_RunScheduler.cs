using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("r008_run_scheduler")]
    public class R008_RunScheduler
    {
        [Key]
        public long Id { get; set; }
        
        [Column("time")]
        public DateTime? Time { get; set; }
        
        [Column("cell_name")]
        public string CellName { get; set; }
        
        [Column("enodeb_name")]
        public string EnodebName { get; set; }
        
        [Column("srn")]
        public int? Srn { get; set; }
        
        [Column("cn")]
        public int? Cn { get; set; }
        
        [Column("sn")]
        public int? Sn { get; set; }
        
        [Column("localcellid")]
        public string LocalCellId { get; set; }
        
        [Column("run_off")]
        public int? RunOff { get; set; }
        
        [Column("run_on")]
        public int? RunOn { get; set; }
        
        [Column("time_run_off")]
        public DateTime? TimeRunOff { get; set; }
        
        [Column("time_run_on")]
        public DateTime? TimeRunOn { get; set; }
        
        // Calculated property for duration in hours
        [NotMapped]
        public double? DurationHours
        {
            get
            {
                if (TimeRunOff.HasValue && TimeRunOn.HasValue)
                {
                    return (TimeRunOn.Value - TimeRunOff.Value).TotalHours;
                }
                return null;
            }
        }
    }
}
