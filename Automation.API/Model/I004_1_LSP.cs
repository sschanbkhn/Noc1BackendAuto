using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Models;

namespace Network.API.Model
{
    [Table("lsps")]
    public class I004_1_LSP : AuditEntity
    {
        [StringLength(255)]
        public string name { get; set; }
        
        [StringLength(255)]
        public string from_address { get; set; }
        
        [StringLength(255)]
        public string host_name_from { get; set; }
        
        [StringLength(255)]
        public string to_address { get; set; }
        
        [StringLength(255)]
        public string host_name_to { get; set; }
        
        [StringLength(255)]
        public string action { get; set; }
        
        [StringLength(255)]
        public string operational_status { get; set; }
        
        public decimal? bandwidth { get; set; }
        
        [StringLength(500)]
        public string path_lsp { get; set; }
        
        public DateTime? last_update { get; set; }
    }
}
