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
using Network.Core.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Linq.Dynamic.Core;
using SysAccountVNPT.API.ViewModel.DsAccountDevice;

namespace Network.API.Service.Net_AlarmType
{
    public class Service : RepositoryBase<Model.Net_AlarmType>, Net_AlarmType.IService
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

        public async Task<List<AlarmTypeList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_AlarmType
                        join y in _dbContext.Sys_Categories on x.LevelId equals y.Id
                        into sub_Sys_Categories
                        from yy in sub_Sys_Categories.DefaultIfEmpty()
                        select new AlarmTypeList { Id = x.Id, Code = x.Code, Name = x.Name, LevelName = yy != null ? yy.Name : null };
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
                result = await _dbContext.Net_AlarmType.Where(o => o.Code == Code).AnyAsync();
            }
            else
            {
                var count = await _dbContext.Net_AlarmType.Where(o => o.Id == Id && o.Code == Code).CountAsync();
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
