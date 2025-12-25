using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Speed_FileImport
{
    public class Service:RepositoryBase<Model.Speed_FileImport>, Speed_FileImport.IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService):base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }
        public async Task<bool> IsExistsFileImport(string fileName)
        {
            return await _dbContext.Speed_FileImport.AnyAsync(o => o.Name == fileName);
        }
        public async Task<string> LastFileImportAsync()
        {
            var items = await _dbContext.Speed_FileImport
            .OrderByDescending(o => o.CreatedDateTime) // Sắp xếp theo CreatedDateTime giảm dần
            .FirstOrDefaultAsync(); // Lấy bản ghi đầu tiên (nếu có)
            return items?.Name ?? "No data";
        }
    }
}
