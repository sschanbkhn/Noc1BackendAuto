using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Role
{
    public interface IService: IRepositoryBase<Model.Sys_Role>
    {
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
        Task DeleteById(Guid Id);
    }
}
