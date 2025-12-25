using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Network.API.Model
{
    public class Speed_FileImport : AuditEntity
    {
        [StringLength(255)]
        public string Name { get; set; }
        public long FileLength { get;set; }
    }
}
