using Network.API.ViewModel.Net_CurenAlarm;
using Network.API.ViewModel.Net_HistoryCurenAlarm;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Network.API.Service.Net_HistoryCurenAlarm
{
    public interface IService: IRepositoryBase<Model.Net_HistoryCurenAlarm>
    {
        Task<List<HistoryCurenAlarmList>> GetListAsync(int page, int pageSize, int totalLimitItems);
    }
}
