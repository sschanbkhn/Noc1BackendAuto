using Network.API.ViewModel.Net_Devices;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Network.API.ViewModel.Net_NetworkLinks;


namespace Network.API.Service.Net_NetworkLinks
{
    public interface IService: IRepositoryBase<Model.Net_NetworkLinks>
    {
        Task<List<NetworkLinkList>> GetListAsync(int page, int pageSize, int totalLimitItems);
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code);
        Task<List<ConnectionsResult>> GetConnectionsAsync();
    }
}
