using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Models;

namespace Network.API.Model
{
    [Table("hardware_alarm_detail", Schema = "public")]
    public class I002_HardwareAlarm : AuditEntity
    {
        [Column("device")]
        [StringLength(255)]
        public string Device { get; set; }
        
        [Column("iploopback")]
        [StringLength(255)]
        public string IpLoopback { get; set; }
        
        [Column("keyword")]
        [StringLength(500)]
        public string Keyword { get; set; }
        
        [Column("severity")]
        [StringLength(100)]
        public string Severity { get; set; }
        
        [Column("raw_log")]
        public string RawLog { get; set; }
        
        [Column("fpc_slot")]
        [StringLength(50)]
        public string FpcSlot { get; set; }
        
        [Column("fpc_sn")]
        [StringLength(255)]
        public string FpcSn { get; set; }
        
        [Column("fpc_pn")]
        [StringLength(255)]
        public string FpcPn { get; set; }
        
        [Column("fpc_desc")]
        [StringLength(500)]
        public string FpcDesc { get; set; }
        
        [Column("fpc_ver")]
        [StringLength(100)]
        public string FpcVer { get; set; }
        
        [Column("fpc_model")]
        [StringLength(255)]
        public string FpcModel { get; set; }
        
        [Column("intf_list")]
        public string IntfList { get; set; } // JSONB stored as string
        
        [Column("can_restart")]
        public int? CanRestart { get; set; }
        
        [Column("is_active")]
        public int? IsActive { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [Column("alarm_id")]
        public int? AlarmId { get; set; }
        
        [Column("time_check")]
        public DateTime? TimeCheck { get; set; }
        
        [Column("user_check")]
        [StringLength(255)]
        public string UserCheck { get; set; }
    }
    
    [Table("hardware_alarm_history", Schema = "public")]
    public class I002_HardwareAlarmHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("device")]
        [StringLength(255)]
        public string Device { get; set; }
        
        [Column("iploopback")]
        [StringLength(255)]
        public string IpLoopback { get; set; }
        
        [Column("keyword")]
        [StringLength(500)]
        public string Keyword { get; set; }
        
        [Column("severity")]
        [StringLength(100)]
        public string Severity { get; set; }
        
        [Column("raw_log")]
        public string RawLog { get; set; }
        
        [Column("fpc_slot")]
        [StringLength(50)]
        public string FpcSlot { get; set; }
        
        [Column("fpc_sn")]
        [StringLength(255)]
        public string FpcSn { get; set; }
        
        [Column("fpc_pn")]
        [StringLength(255)]
        public string FpcPn { get; set; }
        
        [Column("fpc_desc")]
        [StringLength(500)]
        public string FpcDesc { get; set; }
        
        [Column("fpc_ver")]
        [StringLength(100)]
        public string FpcVer { get; set; }
        
        [Column("fpc_model")]
        [StringLength(255)]
        public string FpcModel { get; set; }
        
        [Column("intf_list")]
        public string IntfList { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [Column("alarm_id")]
        public int? AlarmId { get; set; }
        
        [Column("time_check")]
        public DateTime? TimeCheck { get; set; }
        
        [Column("user_check")]
        [StringLength(255)]
        public string UserCheck { get; set; }
        
        [Column("cause_name")]
        public string CauseName { get; set; }
        
        [Column("cause_create")]
        public DateTime? CauseCreate { get; set; }
    }
    
    [Table("hardware_alarm_reset_postcheck_data", Schema = "public")]
    public class I002_HardwareAlarmResetData : AuditEntity
    {
        [Column("alarm_id")]
        public int? AlarmId { get; set; }
        
        [Column("restart_status")]
        [StringLength(255)]
        public string RestartStatus { get; set; }
        
        [Column("fpc_status")]
        public string FpcStatus { get; set; } // JSONB stored as string
        
        [Column("intf_status")]
        public string IntfStatus { get; set; } // JSONB stored as string
        
        [Column("alarms")]
        public string Alarms { get; set; } // JSONB stored as string
        
        [Column("summary_status")]
        [StringLength(500)]
        public string SummaryStatus { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
    
    // Bảng tracking để lưu thông tin check và status (tạo bảng này trong DB)
    [Table("hardware_alarm_tracking", Schema = "public")]
    public class I002_HardwareAlarmTracking : AuditEntity
    {
        [Column("alarm_db_id")]
        public int AlarmDbId { get; set; } // ID từ bảng hardware_alarm_detail
        
        [Column("check_time")]
        public DateTime? CheckTime { get; set; }
        
        [Column("check_user")]
        [StringLength(255)]
        public string CheckUser { get; set; }
        
        [Column("status_process")]
        [StringLength(100)]
        public string StatusProcess { get; set; }
        
        [Column("notes")]
        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
