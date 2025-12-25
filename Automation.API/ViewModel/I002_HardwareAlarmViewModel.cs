using System;

namespace Network.API.ViewModel
{
    public class I002_HardwareAlarmViewModel
    {
        public int Id { get; set; }
        public string Device { get; set; }
        public string IpLoopback { get; set; }
        public string Keyword { get; set; }
        public string Severity { get; set; }
        public string RawLog { get; set; }
        public string FpcSlot { get; set; }
        public string FpcSn { get; set; }
        public string FpcPn { get; set; }
        public string FpcDesc { get; set; }
        public string FpcVer { get; set; }
        public string FpcModel { get; set; }
        public string IntfList { get; set; }
        public string RestartStatus { get; set; }
        public string RawResult { get; set; }
        public int? CanRestart { get; set; }
        public int? IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? AlarmId { get; set; }
        
        // From hardware_alarm_reset_postcheck_data table
        public string SummaryStatus { get; set; }
        public string FpcStatus { get; set; }
        
        // From tracking table
        public DateTime? CheckTime { get; set; }
        public string CheckUser { get; set; }
        public string StatusProcess { get; set; }
    }
}
