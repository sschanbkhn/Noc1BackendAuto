using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_AuthToken
{
    public class Service : RepositoryBase<Model.Sys_AuthToken>, Sys_AuthToken.IService
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
        public async Task SaveByUserNameAsync(string UserName, Model.Sys_AuthToken authToken)
        {
            var tokenOld = await _dbContext.Sys_AuthTokens.Where(o => o.UserName == UserName).FirstOrDefaultAsync();
            if (tokenOld == null)
            {
                Model.Sys_AuthToken authTokenNew = new Model.Sys_AuthToken();                
                authTokenNew.Id = Guid.NewGuid();
                authTokenNew.AccessToken = authToken.AccessToken;
                authTokenNew.RefeshToken = authToken.RefeshToken;
                authTokenNew.UserName = UserName;
                authTokenNew.CreatedBy = UserName;
                authToken.CreatedDateTime = _dateTimeProvider.OffsetNow;
                await _dbContext.Sys_AuthTokens.AddAsync(authTokenNew);
            }
            else
            {
                ObjectHelpers.Mapping<Model.Sys_AuthToken, Model.Sys_AuthToken>(authToken, tokenOld, new string[] { "id" });
                tokenOld.UserName = UserName;
                tokenOld.UpdatedBy = UserName;
                tokenOld.UpdatedDateTime = _dateTimeProvider.OffsetNow;
                //_dbContext.Sys_AuthTokens.Update(token);
                _dbContext.Entry(tokenOld).CurrentValues.SetValues(tokenOld);
            }
            await UnitOfWork.SaveAsync();
        }
        public async Task<Model.Sys_AuthToken> GetByUserNameAsync(string UserName)
        {
            return await _dbContext.Sys_AuthTokens.Where(o => o.UserName == UserName).FirstOrDefaultAsync();
        }
    }
}
