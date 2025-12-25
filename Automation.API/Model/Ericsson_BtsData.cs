using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("ericsson_bts_data")]
    public class Ericsson_BtsData
    {
        [Column("Cellname")]
        public string Cellname { get; set; }

        [Column("eNodeB_ID")]
        public string ENodeBID { get; set; }

        [Column("eNodeB_name")]
        public string ENodeB_Name { get; set; }

        [Column("lcrId")]
        public int? LcrId { get; set; }

        [Column("TAC")]
        public int TAC { get; set; }

        [Column("ULEARFCN")]
        public int? ULEARFCN { get; set; }

        [Column("DLEARFCN")]
        public int? DLEARFCN { get; set; }

        [Column("phyCellId")]
        public int PhyCellId { get; set; }

        [Column("BandwidthDL")]
        public string BandwidthDL { get; set; }

        [Column("Celltype")]
        public string CellType { get; set; }

        [Column("AdminState")]
        public string AdminState { get; set; }

        [Column("technology_type")]
        public string Technology { get; set; }

        [Column("province_code")]
        public string Provincecode { get; set; }

        [Column("district_code")]
        public string Districtcode { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("RSI")]
        public int? RSI { get; set; }

        [Column("MIMO")]
        public string MIMO { get; set; }

        [Column("BandwidthUL")]
        public string BandwidthUL { get; set; }
    }
}
