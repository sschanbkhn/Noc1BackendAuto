using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Network.Core.Models;
using static Network.Core.Enumeration.Sys_Enum;

namespace Core.Infrastructure.Models.Logs
{
    public class AuditLogEntry
    {
        public AuditLogEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> ObjectId { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldObject { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewObject { get; set; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            audit.UserId = UserId;
            audit.UserName = UserName;
            audit.TableName = TableName;
            audit.Type = AuditType.ToString();
            audit.DateTime = DateTimeOffset.Now;
            audit.ObjectId = JsonConvert.SerializeObject(ObjectId);
            audit.OldObject = OldObject.Count == 0 ? null : JsonConvert.SerializeObject(OldObject);
            audit.NewObject = NewObject.Count == 0 ? null : JsonConvert.SerializeObject(NewObject);
            return audit;
        }
    }
}
