using System.ComponentModel.DataAnnotations;
using System;

namespace Network.API.ViewModel.Net_ConfigurationLogs
{
    public class ConfigurationLogsList
    {
        public Guid Id { get; set; }

        public string Device { get; set; }

        public string BeforeConfig { get; set; }

        public string AfterConfig { get; set; }

        public string Time { get; set; }
    }
}
