using Network.Core.Interfaces;
using System.Threading.Tasks;
using System;
using Network.Core.Models;
using SysAccountVNPT.API.ViewModel.DsAccountDevice;
using System.Collections.Generic;


namespace Network.API.Service.Net_AlarmType
{
    public interface IService: IRepositoryBase<Model.Net_AlarmType>
    {
        Task<List<AlarmTypeList>> GetListAsync(int page, int pageSize, int totalLimitItems);
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
    }
}
