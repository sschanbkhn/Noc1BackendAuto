using System;

namespace Network.API.ViewModel.Net_HistoryCurenAlarm
{
    public class HistoryCurenAlarmList
    {
        public Guid Id { get; set; }

        public string AlarmType { get; set; }

        public string Device { get; set; }

        public string Level { get; set; }

        public string Status { get; set; }

        public string IncidentTime { get; set; }

        public string RecoveryTime { get; set; }

        public string AlarmDetail { get; set; }

        public string AlarmCode { get; set; }

        public string Reason { get; set; }

        public string Note { get; set; }

        public string ProcessedContent { get; set; }
    }
}
