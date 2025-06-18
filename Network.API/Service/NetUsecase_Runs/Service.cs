using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.API.Model;
using Network.API.Service;
using Network.API.ViewModel.Net_CurenAlarm;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using Network.Core.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SysAccountVNPT.API.ViewModel.DsAccountDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Network.API.Service.NetUsecase_Runs
{
    public class Service : RepositoryBase<Model.NetUsecase_Run>, NetUsecase_Runs.IService
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

        public async Task<List<NetUsecase_View>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from run in _dbContext.NetUsecase_Run
                        join field in _dbContext.Net_UC_LinhVuc on run.LinhVucId equals field.Id
                        join status in _dbContext.Net_UC_TrangThai on run.TrangThaiId equals status.Id
                        select new Model.NetUsecase_View
                        {
                            SystemName = run.UsecaseName,
                            StatusName = status.Name,
                            Result = run.Result,
                            StartTime = run.StartTime,
                            EndTime = run.EndTime,
                            FieldName = field.Name
                        };
            return await query.ToListAsync();
        }
    }
}
