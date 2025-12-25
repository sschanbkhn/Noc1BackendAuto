using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.API.Service;
using Network.API.ViewModel.Net_NetworkLinks;
using Network.API.ViewModel.Net_CableManagement;

namespace Network.API.Service.Net_CableManagement
{
    public class Service : RepositoryBase<Model.Net_CableManagement>, Net_CableManagement.IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService) : base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }

        public async Task<List<CableManagementList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_CableManagement
                        join y1 in _dbContext.Net_Devices on x.HeadDeviceId equals y1.Id
                        into sub_Net_Devices1
                        from yy1 in sub_Net_Devices1.DefaultIfEmpty()
                        join y2 in _dbContext.Net_Devices on x.LastDeviceId equals y2.Id
                        into sub_Net_Devices2
                        from yy2 in sub_Net_Devices2.DefaultIfEmpty()
                        join z in _dbContext.Net_NetworkLinks on x.LineId equals z.Id
                        into sub_Net_NetworkLinks
                        from zz in sub_Net_NetworkLinks.DefaultIfEmpty()

                        select new CableManagementList
                        {
                            Id = x.Id,
                            CableCode = x.CableCode,
                            CableType = x.CableType,
                            HeadDevice = yy1 != null ? yy1.Name : null,
                            LastDevice = yy2 != null ? yy1.Name : null,
                            Line = zz != null ? zz.SerialNumber : null,
                            SetPoint = x.SetPoint,
                            ManageOrgan = x.ManageOrgan,
                            ManagerName = x.ManagerName,
                            ManagerTel = x.ManagerTel,
                            ManagerEmail = x.ManagerEmail
                        };
            return await query.ToListAsync();
        }

        public async Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code)
        {
            bool result = false;
            if (string.IsNullOrEmpty(Code))
            {
                throw new Exception(Sys_Const.Message.SERVICE_CODE_NOT_EMPTY);
            }
            if (GuidHelpers.IsNullOrEmpty(Id))
            {
                result = await _dbContext.Net_CableManagement.Where(o => o.CableCode == Code).AnyAsync();
            }
            else
            {
                var count = await _dbContext.Net_CableManagement.Where(o => o.Id == Id && o.CableCode == Code).CountAsync();
                if (count <= 1)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return await Task.FromResult(result);
        }
    }
}
