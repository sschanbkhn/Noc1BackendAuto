using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Organization
{
    public interface IService: IRepositoryBase<Model.Sys_Organization>
    {
        Task<List<ViewModel.Sys_Organization.OrganTree>> GetTreeAsync();
        Task<List<ViewModel.Sys_Organization.OrganTree>> GetTreeListAsync();
        Task<List<Model.Sys_Organization>> GetByParentIdAsync(Guid ParentId);
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code, Guid ParentId);
        Task DeleteById(Guid Id);
    }
}
