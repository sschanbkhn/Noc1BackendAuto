using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Models;

namespace Network.API.Model
{
    [Table("lsps")]
    public class I004_LSP : AuditEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("from_address")]
        public string FromAddress { get; set; }

        [Column("to_address")]
        public string ToAddress { get; set; }

        [Column("action")]
        public string Action { get; set; }

        [Column("operational_status")]
        public string OperationalStatus { get; set; }

        [Column("bandwidth")]
        public long Bandwidth { get; set; }

        [Column("path_lsp")]
        public string PathLsp { get; set; }

        [Column("last_update")]
        public DateTime LastUpdate { get; set; }
    }
    
    // DTOs for response models
    public class LSPInternationalDataDto
    {
        public string Name { get; set; }
        public string FromAddress { get; set; }
        public string HostNameFrom { get; set; }
        public string ToAddress { get; set; }
        public string HostNameTo { get; set; }
        public string Action { get; set; }
        public string OperationalStatus { get; set; }
        public decimal Bandwidth { get; set; }
        public string PathLsp { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    
    public class RouterNodeDto
    {
        public string HostName { get; set; }
        public string IdNode { get; set; }
    }
    
    public class RoutePCEPStatusDto
    {
        public int UpCount { get; set; }
        public int DownCount { get; set; }
    }
    
    public class LSPDelegatedStatusDto
    {
        public int ActiveCount { get; set; }
        public int DownCount { get; set; }
        public int UnknownCount { get; set; }
    }
    
    public class LSPActionStatsDto
    {
        public int AddCount { get; set; }
        public int UpdateCount { get; set; }
        public int RemoveCount { get; set; }
    }
    
    public class LSPBandwidthDto
    {
        public DateTime Ts { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string PathLsp { get; set; }
        public decimal Bandwidth { get; set; }
    }
}
