using Network.API.ViewModel.Net_DevicePorts;
using Network.API.ViewModel.Net_Devices;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Network.API.Service.Net_DevicePorts
{
    public interface IService: IRepositoryBase<Model.Net_DevicePorts>
    {
        Task<List<DevicePortsList>> GetListAsync(int page, int pageSize, int totalLimitItems);
        Task<List<Model.Net_DevicePorts>> GetListByDeviceIdAsync(Guid DeviceId);
    }
}
