using Network.API.ViewModel.Net_NetworkLinks;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Network.API.ViewModel.Net_CableManagement;


namespace Network.API.Service.Net_CableManagement
{
    public interface IService: IRepositoryBase<Model.Net_CableManagement>
    {
        Task<List<CableManagementList>> GetListAsync(int page, int pageSize, int totalLimitItems);
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
    }
}
