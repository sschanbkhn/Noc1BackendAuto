using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("error_links_status")]
    public class I002_ErrorLinksStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("host")]
        public string Host { get; set; }

        [Column("interface")]
        public string Interface { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("bandwidth")]
        public string Bandwidth { get; set; }

        [Column("ae")]
        public string Ae { get; set; }

        [Column("input_rate")]
        public long? InputRate { get; set; }

        [Column("output_rate")]
        public long? OutputRate { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("ae_bandwidth")]
        public string AeBandwidth { get; set; }

        [Column("shut_link")]
        public bool? ShutLink { get; set; }
    }
}
