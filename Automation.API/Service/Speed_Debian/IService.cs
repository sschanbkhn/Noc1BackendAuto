using Microsoft.AspNetCore.Http;
using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Speed_Debian
{
    public interface IService: IRepositoryBase<Model.Speed_Debian>
    {
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
        Task DeleteById(Guid Id);
        Task<List<Model.Speed_Debian>> GetSpeedDebianByDateAsync(DateTime resultReceivedDate);
    }
}
