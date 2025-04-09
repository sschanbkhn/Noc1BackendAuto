using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class ClientRequestInfo
    {        
        public string Address { get; set; }
        public string MediaType { get; set; } = "application/json";
        public string Bearer { get; set; } = "bearer";
        public string Token { get; set; }
    }
}
