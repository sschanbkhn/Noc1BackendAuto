using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class LoginResult
    {
        public Guid UserId { get; set; }
        public string[] Roles { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string AccessToken { get; set; }
    }
}
