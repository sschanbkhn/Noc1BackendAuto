using Network.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.Core.Enums;

namespace Network.API.ViewModel.Sys_Organization
{
    public class OrganTree:absTree<OrganTree>
    {
        public OrganizationType Type { get; set; }
    }
}
