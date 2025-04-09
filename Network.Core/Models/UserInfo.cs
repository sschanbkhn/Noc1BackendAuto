using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; }       
        public string UserName { get; set; }       
        public string Email { get; set; }        
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<string> Roles { get; set; }
    }
}
