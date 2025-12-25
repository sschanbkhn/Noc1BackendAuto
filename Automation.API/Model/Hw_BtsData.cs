using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("hw_bts_data")]
    public class Hw_BtsData
    {
        [Column("cell_name")]
        [StringLength(255)]
        public string CellName { get; set; }
        
        [Column("id_cell")]
        [StringLength(100)]
        public Int32 IdCell { get; set; }
        
        [Column("enodeb_id")]
        [StringLength(100)]
        public string EnodebId { get; set; }
        
        [Column("localcellid")]
        [StringLength(100)]
        public string LocalCellId { get; set; }
        
        [Column("ulearfcncfgind")]
        [StringLength(100)]
        public string Ulearfcncfgind { get; set; }
        
        [Column("dlearfcn")]
        [StringLength(100)]
        public string Dlearfcn { get; set; }
        
        [Column("rootsequenceidx")]
        [StringLength(100)]
        public string RootSequenceIdx { get; set; }
        
        [Column("txrxmode")]
        [StringLength(100)]
        public string Txrxmode { get; set; }
        
        [Column("ulbandwidth")]
        [StringLength(100)]
        public string Ulbandwidth { get; set; }
        
        [Column("dlbandwidth")]
        [StringLength(100)]
        public string Dlbandwidth { get; set; }
        
        [Column("freqband")]
        [StringLength(100)]
        public string Freqband { get; set; }
        
        [Column("nename")]
        [StringLength(255)]
        public string Nename { get; set; }
        
        [Column("tac")]
        [StringLength(100)]
        public string Tac { get; set; }
        
        [Column("phycellid")]
        [StringLength(100)]
        public string Phycellid { get; set; }
        
        [Column("createdate")]
        public DateTime? CreateDate { get; set; }
    }
} 