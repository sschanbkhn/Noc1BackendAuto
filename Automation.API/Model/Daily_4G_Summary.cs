using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("daily_4g_summary")]
    public class Daily_4G_Summary
    {
        [Key]
        [Column("report_date")]
        public DateTime ReportDate { get; set; }

        [Column("province")]
        public string Province { get; set; }

        [Column("nokia_sites")]
        public int NokiaSites { get; set; }

        [Column("huawei_sites")]
        public int HuaweiSites { get; set; }

        [Column("total_4g_cells")]
        public int Total4GCells { get; set; }

        [Column("moran_cells")]
        public int MoranCells { get; set; }

        [Column("iot_cells")]
        public int IoTCells { get; set; }

        [Column("band_900")]
        public int Band900 { get; set; }

        [Column("band_1800")]
        public int Band1800 { get; set; }

        [Column("band_2100")]
        public int Band2100 { get; set; }

        [Column("txrxmode_4t4r")]
        public int TxRxMode4T4R { get; set; }

        [Column("txrxmode_2t4r")]
        public int TxRxMode2T4R { get; set; }

        [Column("txrxmode_2t2r")]
        public int TxRxMode2T2R { get; set; }

        [Column("txrxmode_1t2r")]
        public int TxRxMode1T2R { get; set; }

        [Column("txrxmode_1t1r")]
        public int TxRxMode1T1R { get; set; }

        [Column("huawei_4g_cells")]
        public int Huawei4GCells { get; set; }

        [Column("nokia_4g_cells")]
        public int Nokia4GCells { get; set; }
    }
} 