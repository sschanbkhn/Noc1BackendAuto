using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public string PasswordOld { get; set; }
        [Required]
        public string PasswordNew { get; set; }
    }
}
