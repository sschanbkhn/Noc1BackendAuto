using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Service.I002_HardwareAlarm
{
    public interface IService : IRepositoryBase<Model.I002_HardwareAlarm>
    {
        Task<List<ViewModel.I002_HardwareAlarmViewModel>> GetHardwareAlarmListAsync();
        Task<dynamic> CheckAlarmAsync(int alarmId, string username);
        Task<dynamic> AutoRebootAsync(int alarmId, string deviceName, string fpcSlot, string keyword, string username);
        Task<dynamic> ManualHandleAsync(int alarmId, string username, string causeName = "Manual Handle");
        Task<List<Model.I002_ErrorLinksStatus>> GetErrorLinksStatusAsync();
        Task<List<Model.I002_HardwareAlarmHistory>> GetHardwareAlarmHistoryAsync();
    }
}
