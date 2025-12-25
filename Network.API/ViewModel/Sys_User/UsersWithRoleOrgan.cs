using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.ViewModel.Sys_User
{
    public class UsersWithRoleOrgan
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string OrganName { get; set; }
    }
}
