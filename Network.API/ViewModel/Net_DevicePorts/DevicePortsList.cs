using System;

namespace Network.API.ViewModel.Net_DevicePorts
{
    public class DevicePortsList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Device {  get; set; }
        public string SerialPort { get; set; }
        public string PortFormat { get; set; }
        public string Type { get; set; }
        public int MaxSpeed { get; set; }
        public string Status { get; set; }
    }
}
