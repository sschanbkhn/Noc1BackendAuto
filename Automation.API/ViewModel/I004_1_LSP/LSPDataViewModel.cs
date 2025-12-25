using System;

namespace Network.API.ViewModel.I004_1_LSP
{
    public class LSPDataViewModel
    {
        public string Name { get; set; }
        public string FromAddress { get; set; }
        public string HostNameFrom { get; set; }
        public string ToAddress { get; set; }
        public string HostNameTo { get; set; }
        public string Action { get; set; }
        public string OperationalStatus { get; set; }
        public decimal? Bandwidth { get; set; }
        public string PathLSP { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
