using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Category
{
    public interface IService: IRepositoryBase<Model.Sys_Category>
    {
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code, int Type);
        Task<Model.Sys_Category> GetItemByCode(Core.Enums.CategoryType Type, string Code);
        Task<Model.Sys_Category> GetItemById(Core.Enums.CategoryType Type, Guid Id);
    }
}
