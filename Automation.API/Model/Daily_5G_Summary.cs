using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("daily_5g_summary")]
    public class Daily_5G_Summary
    {
        [Key]
        [Column("report_date")]
        public DateTime ReportDate { get; set; }

        [Column("province")]
        public string Province { get; set; }

        [Column("nokia_5g_sites")]
        public int Nokia5GSites { get; set; }

        [Column("total_5g_cells")]
        public int Total5GCells { get; set; }

        [Column("chbw_100_mhz")]
        public int Chbw100Mhz { get; set; }

        [Column("chbw_80_mhz")]
        public int Chbw80Mhz { get; set; }

        [Column("chbw_60_mhz")]
        public int Chbw60Mhz { get; set; }

        [Column("chbw_40_mhz")]
        public int Chbw40Mhz { get; set; }

        [Column("chbw_20_mhz")]
        public int Chbw20Mhz { get; set; }

        [Column("txrx_48_12")]
        public int TxRx4812 { get; set; }

        [Column("txrx_32_8")]
        public int TxRx328 { get; set; }
    }
} 