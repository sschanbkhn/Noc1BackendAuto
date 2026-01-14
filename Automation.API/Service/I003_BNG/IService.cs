using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Service.I003_BNG
{
    public interface IService : IRepositoryBase<Model.I003_BNG>
    {
        Task<List<Model.I003_BNG>> GetBNGDataAsync();
        Task<dynamic> ClearOverLimitSessionAsync(string ip);
        Task<dynamic> CheckOneUserAsync(string username, string ip);
        Task<dynamic> ClearOverLimitOneUserAsync(string username, string ip);
        Task<dynamic> ClearAllOneUserAsync(string username, string ip);
        Task<dynamic> GetDashboardDataAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<List<dynamic>> GetLocationListAsync();
        Task<List<dynamic>> GetBNGDataByLocationAsync(string location, DateTime? reportDate = null);
        Task<dynamic> GetSessionUserDashboardDataAsync(DateTime? reportDate = null, string location = null);
    }
}