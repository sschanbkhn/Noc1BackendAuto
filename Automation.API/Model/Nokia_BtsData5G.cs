using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("nokia_bts_data5g")]
    public class Nokia_BtsData5G
    {
        [Column("id_bts")]
        [StringLength(100)]
        public string IdBts { get; set; }
        
        [Column("nrcell_mo_id")]
        [StringLength(100)]
        public string NrCellMoId { get; set; }
        
        [Column("celltechnology")]
        [StringLength(100)]
        public string CellTechnology { get; set; }
        
        [Column("celldeptype")]
        [StringLength(100)]
        public string CellDepType { get; set; }
        
        [Column("cellname")]
        [StringLength(255)]
        public string CellName { get; set; }
        
        [Column("physcellid")]
        [StringLength(100)]
        public string PhysCellId { get; set; }
        
        [Column("lcrid")]
        [StringLength(100)]
        public string Lcrid { get; set; }
        
        [Column("prachrootsequenceindex")]
        [StringLength(100)]
        public string PrachRootSequenceIndex { get; set; }
        
        [Column("chbw")]
        [StringLength(100)]
        public string ChBw { get; set; }
        
        [Column("nrarfcn")]
        [StringLength(100)]
        public string NrArfcn { get; set; }
        
        [Column("administrativestate")]
        [StringLength(100)]
        public string AdministrativeState { get; set; }
        
        [Column("basicbeamset")]
        [StringLength(100)]
        public string BasicBeamSet { get; set; }
        
        [Column("nrbts_mo_id")]
        [StringLength(100)]
        public string NrBtsMoId { get; set; }
        
        [Column("nrbts_name")]
        [StringLength(255)]
        public string NrBtsName { get; set; }
        
        [Column("mrbts_mo_id")]
        [StringLength(100)]
        public string MrBtsMoId { get; set; }
        
        [Column("mrbts_name")]
        [StringLength(255)]
        public string MrBtsName { get; set; }
        
        [Column("direction")]
        [StringLength(100)]
        public string Direction { get; set; }
        
        [Column("createdate")]
        public DateTime? CreateDate { get; set; }
    }
} 