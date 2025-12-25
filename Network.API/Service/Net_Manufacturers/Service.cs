using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Interfaces;

namespace Network.API.Service.Net_Manufacturers
{
    public class Service : RepositoryBase<Model.Net_Manufacturers>, Net_Manufacturers.IService
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
