using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("zte_bts_data")]
    public class Zte_BtsData
    {
        [Column("technology")]
        [StringLength(100)]
        public string Technology { get; set; }
        
        [Column("Cellname")]
        [StringLength(255)]
        public string Cellname { get; set; }
        
        [Column("TAC")]
        public int? TAC { get; set; }
        
        [Column("phyCellId")]
        public int? PhyCellId { get; set; }
        
        [Column("lcrId")]
        public int? LcrId { get; set; }
        
        [Column("ULEARFCN")]
        public int? ULEARFCN { get; set; }
        
        [Column("DLEARFCN")]
        public int? DLEARFCN { get; set; }
        
        [Column("CellType")]
        [StringLength(100)]
        public string CellType { get; set; }
        
        [Column("Cellremote")]
        [StringLength(100)]
        public string Cellremote { get; set; }
        
        [Column("RSI_Decimal")]
        public int? RSI_Decimal { get; set; }
        
        [Column("Bandwidth")]
        [StringLength(100)]
        public string Bandwidth { get; set; }
        
        [Column("MIMO")]
        [StringLength(100)]
        public string MIMO { get; set; }
        
        [Column("eNodeBID")]
        [StringLength(100)]
        public string ENodeBID { get; set; }
        
        [Column("Provincecode")]
        [StringLength(100)]
        public string Provincecode { get; set; }
        
        [Column("Districtcode")]
        [StringLength(100)]
        public string Districtcode { get; set; }
        
        [Column("NET")]
        [StringLength(100)]
        public string NET { get; set; }
        
        [Column("eNodeB_Name")]
        [StringLength(255)]
        public string ENodeB_Name { get; set; }
        
        [Column("NE_Name")]
        [StringLength(255)]
        public string NE_Name { get; set; }
        
        [Column("AdminState")]
        [StringLength(100)]
        public string AdminState { get; set; }
        
        [Column("DeviceType")]
        [StringLength(100)]
        public string DeviceType { get; set; }
        
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
