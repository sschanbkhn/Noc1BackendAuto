using System;

namespace Network.API.Model
{
    public class ExportBtsDataRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Vendor { get; set; }
    }
} 