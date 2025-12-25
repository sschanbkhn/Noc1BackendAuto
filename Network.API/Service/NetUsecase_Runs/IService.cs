using Network.API.Model;
using Network.Core.Interfaces;
using Network.Core.Models;
using SysAccountVNPT.API.ViewModel.DsAccountDevice;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Network.API.Service.NetUsecase_Runs
{
    public interface IService: IRepositoryBase<Model.NetUsecase_Run>
    {
        Task<List<NetUsecase_View>> GetListAsync(int page, int pageSize, int totalLimitItems);
    }
}
