using Network.API.ViewModel.Net_ConfigurationLogs;
using Network.API.ViewModel.Net_DevicePorts;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Network.API.Service.Net_ConfigurationLogs
{
    public interface IService: IRepositoryBase<Model.Net_ConfigurationLogs>
    {
        Task<List<ConfigurationLogsList>> GetListAsync(int page, int pageSize, int totalLimitItems);
    }
}
