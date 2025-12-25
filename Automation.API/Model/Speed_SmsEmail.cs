using Network.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    //[Table("Speed_SmsEmail")]
    public class Speed_SmsEmail : AuditEntity
    {
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? Time_create { get; set; }
        public DateTimeOffset? Time_send { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Titleemail { get; set; }
        public string Contentemail { get; set; }
        public string Statusmail { get; set; }

    }
}
