using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class SocialLoginRequest
    {
        [Required]
        public string Email { get; set; }       
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Token_Google { get; set; }
        public string Token_Facebook { get; set; }
    }
}
