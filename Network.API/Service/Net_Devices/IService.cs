using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Network.API.ViewModel.Net_Devices;


namespace Network.API.Service.Net_Devices
{
    public interface IService: IRepositoryBase<Model.Net_Devices>
    {
        Task<List<DevicesList>> GetListAsync(Guid organId, int page, int pageSize, int totalLimitItems);
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
        Task<List<SwitchesResult>> GetSwitchesAsync();
    }
}
