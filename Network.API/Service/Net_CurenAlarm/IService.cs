using Network.API.ViewModel.Net_CurenAlarm;
using Network.API.ViewModel.Net_Devices;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Network.API.Service.Net_CurenAlarm
{
    public interface IService: IRepositoryBase<Model.Net_CurenAlarm>
    {
        Task<List<CurenAlarmList>> GetListAsync(int page, int pageSize, int totalLimitItems);
    }
}
