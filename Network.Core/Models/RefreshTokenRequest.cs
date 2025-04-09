using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class RefreshTokenRequest
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
