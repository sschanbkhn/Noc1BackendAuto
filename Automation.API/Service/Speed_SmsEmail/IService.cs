using Microsoft.AspNetCore.Http;
using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Speed_SmsEmail
{
    public interface IService: IRepositoryBase<Model.Speed_SmsEmail>
    {
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
        Task DeleteById(Guid Id);
    }
}
