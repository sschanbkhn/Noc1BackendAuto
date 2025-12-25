using System;
using System.Collections.Generic;

namespace Network.API.ViewModel.Dashboard
{
    public class Dashboard4GResponse
    {
        public DateTime ReportDate { get; set; }
        public int TotalSites { get; set; }
        public int TotalCells { get; set; }
        public VendorBreakdown VendorBreakdown { get; set; }
        public TechnologyBreakdown TechnologyBreakdown { get; set; }
        public BandBreakdown BandBreakdown { get; set; }
        public List<ProvincialData> ProvincialData { get; set; }
        public ProvincialTotals ProvincialTotals { get; set; }
        public List<DailyTrend> DailyTrend { get; set; }
    }

    public class VendorBreakdown
    {
        public int Huawei { get; set; }
        public int Nokia { get; set; }
        public int Ericsson { get; set; }
        public int ZTE { get; set; }
    }

    public class TechnologyBreakdown
    {
        public int G4 { get; set; }
        public int G5 { get; set; }
    }

    public class BandBreakdown
    {
        public int Band900MHz { get; set; }
        public int Band1800MHz { get; set; }
        public int Band2100MHz { get; set; }
        public int Band2600MHz { get; set; }
    }

    public class ProvincialData
    {
        public string Province { get; set; }
        public int NokiaSites { get; set; }
        public int HuaweiSites { get; set; }
        public int ZTESites { get; set; }
        public int EricssonSites { get; set; }
        public int Total4GCells { get; set; }
        public int MoranCells { get; set; }
        public int IoTCells { get; set; }
        public int Band900 { get; set; }
        public int Band1800 { get; set; }
        public int Band2100 { get; set; }
        public int Config4T4R { get; set; }
        public int Config2T4R { get; set; }
        public int Config2T2R { get; set; }
        public int Config1T2R { get; set; }
        public int Config1T1R { get; set; }
        public int Huawei4GCells { get; set; }
        public int Nokia4GCells { get; set; }
        public int ZTE4GCells { get; set; }
        public int Ericsson4GCells { get; set; }
    }

    public class ProvincialTotals
    {
        public int NokiaSites { get; set; }
        public int HuaweiSites { get; set; }
        public int ZTESites { get; set; }
        public int EricssonSites { get; set; }
        public int Total4GCells { get; set; }
        public int MoranCells { get; set; }
        public int IoTCells { get; set; }
        public int Band900 { get; set; }
        public int Band1800 { get; set; }
        public int Band2100 { get; set; }
        public int Config4T4R { get; set; }
        public int Config2T4R { get; set; }
        public int Config2T2R { get; set; }
        public int Config1T2R { get; set; }
        public int Config1T1R { get; set; }
        public int Huawei4GCells { get; set; }
        public int Nokia4GCells { get; set; }
        public int ZTE4GCells { get; set; }
        public int Ericsson4GCells { get; set; }
    }

    public class DailyTrend
    {
        public string Date { get; set; }
        public int Sites { get; set; }
        public int Cells { get; set; }
        public int HuaweiCells { get; set; }
        public int NokiaCells { get; set; }
        public int ZTECells { get; set; }
        public int EricssonCells { get; set; }
        public int HuaweiSites { get; set; }
        public int NokiaSites { get; set; }
        public int ZTESites { get; set; }
        public int EricssonSites { get; set; }
    }
} 