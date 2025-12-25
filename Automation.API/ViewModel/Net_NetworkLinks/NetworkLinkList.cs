using System;

namespace Network.API.ViewModel.Net_NetworkLinks
{
    public class NetworkLinkList
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public int Distance { get; set; }
        public string HeadDevice { get; set; }
        public string HeadDevicePort { get; set; }
        public string LastDevice { get; set; }
        public string LastDevicePort { get; set; }
        public string Type { get; set; }
        public string ConnectType { get; set; }
        public int Speed { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
