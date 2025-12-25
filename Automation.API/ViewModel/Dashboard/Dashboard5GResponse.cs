using System;
using System.Collections.Generic;

namespace Network.API.ViewModel.Dashboard
{
    public class Dashboard5GResponse
    {
        public DateTime ReportDate { get; set; }
        public int TotalSites { get; set; }
        public int TotalCells { get; set; }
        public VendorBreakdown5G VendorBreakdown { get; set; }
        public TechnologyBreakdown5G TechnologyBreakdown { get; set; }
        public BandBreakdown5G BandBreakdown { get; set; }
        public List<ProvincialData5G> ProvincialData { get; set; }
        public ProvincialTotals5G ProvincialTotals { get; set; }
        public List<DailyTrend5G> DailyTrend { get; set; }
    }

    public class VendorBreakdown5G
    {
        public int Nokia { get; set; }
    }

    public class TechnologyBreakdown5G
    {
        public int G5 { get; set; }
    }

    public class BandBreakdown5G
    {
        public int Chbw100Mhz { get; set; }
        public int Chbw80Mhz { get; set; }
        public int Chbw60Mhz { get; set; }
        public int Chbw40Mhz { get; set; }
        public int Chbw20Mhz { get; set; }
    }

    public class ProvincialData5G
    {
        public string Province { get; set; }
        public int Nokia5GSites { get; set; }
        public int Total5GCells { get; set; }
        public int Chbw100Mhz { get; set; }
        public int Chbw80Mhz { get; set; }
        public int Chbw60Mhz { get; set; }
        public int Chbw40Mhz { get; set; }
        public int Chbw20Mhz { get; set; }
        public int TxRx4812 { get; set; }
        public int TxRx328 { get; set; }
    }

    public class ProvincialTotals5G
    {
        public int Nokia5GSites { get; set; }
        public int Total5GCells { get; set; }
        public int Chbw100Mhz { get; set; }
        public int Chbw80Mhz { get; set; }
        public int Chbw60Mhz { get; set; }
        public int Chbw40Mhz { get; set; }
        public int Chbw20Mhz { get; set; }
        public int TxRx4812 { get; set; }
        public int TxRx328 { get; set; }
    }

    public class DailyTrend5G
    {
        public string Date { get; set; }
        public int Sites { get; set; }
        public int Cells { get; set; }
    }
} 