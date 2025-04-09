using Network.API.ViewModel.Sys_Permission;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Permission
{
    public interface IService: IRepositoryBase<Model.Sys_Permission>
    {
        Task<List<Model.Sys_Permission>> SaveAsync(Guid[] resourceIds, Guid roleId, bool isFunc);        
        Task<List<Model.Sys_Permission>> GetByRoleIdAsync(Guid roleId, bool isFunc);
        Task<MenuList> GetMenusByRoles(List<string> roles);        
    }
}
