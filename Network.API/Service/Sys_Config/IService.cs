using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Config
{
    public interface IService: IRepositoryBase<Model.Sys_Config>
    {
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code, int Type);
    }
}
