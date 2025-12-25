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

namespace Network.API.Service.Net_UC_TrangThais
{
    public class Service : RepositoryBase<Model.Net_UC_TrangThai>, Net_UC_TrangThais.IService
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

        
    }
}
