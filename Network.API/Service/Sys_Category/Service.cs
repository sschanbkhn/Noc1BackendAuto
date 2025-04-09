using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Category
{
    public class Service:RepositoryBase<Model.Sys_Category>, Sys_Category.IService
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
        public async Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code, int Type)
        {
            bool result = false;
            if (string.IsNullOrEmpty(Code))
            {
                throw new Exception(Sys_Const.Message.SERVICE_CODE_NOT_EMPTY);
            }
            if (GuidHelpers.IsNullOrEmpty(Id))
            {
                result = await _dbContext.Sys_Categories.Where(o => o.Code == Code && o.Type == (Core.Enums.CategoryType)Type).AnyAsync();
            }
            else
            {
                var count = await _dbContext.Sys_Categories.Where(o => o.Id == Id && o.Code == Code && o.Type == (Core.Enums.CategoryType)Type).CountAsync();
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
        public async Task<Model.Sys_Category> GetItemByCode(Core.Enums.CategoryType type, string code)
        {
            return await _dbContext.Sys_Categories.Where(o => o.Type == type && o.Code == code).FirstOrDefaultAsync();
        }
        public async Task<Model.Sys_Category> GetItemById(Core.Enums.CategoryType type, Guid id)
        {
            return await _dbContext.Sys_Categories.Where(o => o.Type == type && o.Id == id).FirstOrDefaultAsync();
        }
    }
}
