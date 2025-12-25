using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("nokia_bts_data")]
    public class Nokia_BtsData
    {
        [Column("id_bts")]
        [StringLength(100)]
        public string IdBts { get; set; }
        
        [Column("lncells_mo_id")]
        [StringLength(100)]
        public string LncellsMoId { get; set; }
        
        [Column("administrativestate")]
        [StringLength(100)]
        public string AdministrativeState { get; set; }
        
        [Column("phycellid")]
        [StringLength(100)]
        public string Phycellid { get; set; }
        
        [Column("tac")]
        [StringLength(100)]
        public string Tac { get; set; }
        
        [Column("cellname")]
        [StringLength(255)]
        public string CellName { get; set; }
        
        [Column("lcrid")]
        [StringLength(100)]
        public string Lcrid { get; set; }
        
        [Column("enbname")]
        [StringLength(255)]
        public string EnbName { get; set; }
        
        [Column("mrbts_name")]
        [StringLength(255)]
        public string MrbtsName { get; set; }
        
        [Column("earfcnul")]
        [StringLength(100)]
        public string EarfcnUl { get; set; }
        
        [Column("earfcndl")]
        [StringLength(100)]
        public string EarfcnDl { get; set; }
        
        [Column("rootseqindex")]
        [StringLength(100)]
        public string RootSeqIndex { get; set; }
        
        [Column("dlchbw")]
        [StringLength(100)]
        public string DlChBw { get; set; }
        
        [Column("ulchbw")]
        [StringLength(100)]
        public string UlChBw { get; set; }
        
        [Column("chbw")]
        [StringLength(100)]
        public string ChBw { get; set; }
        
        [Column("direction")]
        [StringLength(100)]
        public string Direction { get; set; }
        
        [Column("createdate")]
        public DateTime? CreateDate { get; set; }
    }
} 