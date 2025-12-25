using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Models;

namespace Network.API.Model
{
    [Table("bng")]
    public class I003_BNG : AuditEntity
    {
        [StringLength(255)]
        public string location { get; set; }
        
        [StringLength(255)]
        public string province_name { get; set; }
        
        [StringLength(255)]
        public string bng_name { get; set; }
        
        [StringLength(255)]
        public string bng_ip { get; set; }
        
        public int? bng_over_session { get; set; }
        public int? bng_cleared_session { get; set; }
        
        public int? bng_clear_frequency { get; set; }
    }
}