using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Network.API.Model;
using Network.Core.Interfaces;

namespace Network.API.Service.I004_1_LSP
{
    public interface IService : IRepositoryBase<Model.I004_1_LSP>
    {
        Task<List<Model.I004_1_LSP>> GetLSPDataAsync(DateTime fromDate, DateTime toDate);
    }
}
