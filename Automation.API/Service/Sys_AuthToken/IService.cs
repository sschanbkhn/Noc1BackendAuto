using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_AuthToken
{
    public interface IService: IRepositoryBase<Model.Sys_AuthToken>
    {
        Task SaveByUserNameAsync(string UserName, Model.Sys_AuthToken authToken);
        Task<Model.Sys_AuthToken> GetByUserNameAsync(string UserName);
    }
}
