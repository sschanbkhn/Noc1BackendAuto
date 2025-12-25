using Network.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.API.Model
{
    [Table("Net_Manufacturers")]
    public class Net_Manufacturers : AuditEntity
    {
        [StringLength(55)]
        public string Name { get; set; }

        [StringLength(55)]
        public string Nation { get; set; }
    }
}
