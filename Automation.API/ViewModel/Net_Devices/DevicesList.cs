using System;

namespace Network.API.ViewModel.Net_Devices
{
    public class DevicesList
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Lon_Lat { get; set; }
        public string Description { get; set; }
        public string DeviceType { get; set; }
        public string Manufacturer { get; set; }
        public string FirmwareVersion { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string SerialNumber { get; set; }
        public string Organ { get; set; }
        public int NumberOfNetPorts { get; set; }
    }
}
