using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Core.Interfaces;
using Network.Core.Models;
using Xunit;
using Network.API.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using Network.API.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Network.API.Model;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Network.Core.Helpers;
using Network.Core.Constant;

namespace Homes.UnitTest.Services
{
    public class Test_Sys_User
    {               
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Mock<IUserProvider> _userProvider;        
        public Test_Sys_User()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();                     
            _userProvider = new Mock<IUserProvider>();
            _dateTimeProvider.Setup(o => o.OffsetNow).Returns(DateTimeOffset.Now);
            _userProvider.Setup(o => o.Id).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            _userProvider.Setup(o => o.UserName).Returns("admin");
            _userProvider.Setup(o => o.IsAuthenticated).Returns(true);
         
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "admin", "admin@gmail.com")]
        public async Task IsDupicateAttributesAsync(string Id, string UserName, string Email)
        {
            var service = await CreateServiceAsync();
            var result = await service.IsDupicateAttributesAsync(Guid.Parse(Id), UserName, Email);
            Assert.IsType<bool>(result);
        }

        [Theory]
        [InlineData("22222222-2222-2222-2222-222222222221")]
        [InlineData("22222222-2222-2222-2222-222222222222")]
        public async Task DeleteById(string Id)
        {
            var service = await CreateServiceAsync();
            var ex = Record.ExceptionAsync(async () => await service.DeleteById(Guid.Parse(Id)));
            var substrings = new[] { "", Sys_Const.Message.SERVICE_USERNAME_UNEXISTS.ToString()};
            Assert.True(ex.Result.Message.ContainsAny(substrings, StringComparison.CurrentCultureIgnoreCase));
        }

        [Theory]
        [InlineData("22222222-2222-2222-2222-222222222222")]
        public async Task GetDetailByIdAsync(string Id)
        {
            
            var service = await CreateServiceAsync();            
            var result = await service.GetDetailByIdAsync(Guid.Parse(Id));
            Assert.IsType< Network.API.ViewModel.Sys_User.Detail>(result);            
        }

        [Theory]
        [InlineData("vhbao", "vhbao", "1", "vhbao@gmail.com", "0123456789", "hn", true, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111")]
        public async Task CreateAsync(string fullName, string UserName, string passWord, string email, string phone, string address, bool isActive, string organId, string roleId)
        {
            var service = await CreateServiceAsync();
            var user = new Sys_User()
            {
                Id = Guid.Empty,
                FullName = fullName,
                UserName = UserName,
                PassWord = passWord,
                Email = email,
                Phone = phone,
                Address = address,
                IsActive = isActive,
            };
            var result = await service.CreateAsync(user, Guid.Parse(organId), Guid.Parse(roleId));
            Assert.IsType<Network.API.ViewModel.Sys_User.Detail>(result);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "vhbao", "vhbao", "1", "vhbao@gmail.com", "0123456789", "hn", true, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111")]
        public async Task UpdateAsync(string id, string fullName, string UserName, string passWord, string email, string phone, string address, bool isActive, string organId, string roleId)
        {
            var service = await CreateServiceAsync();
            var user = new Sys_User()
            {
                Id = Guid.Parse(id),
                FullName = fullName,
                UserName = UserName,
                PassWord = passWord,
                Email = email,
                Phone = phone,
                Address = address,
                IsActive = isActive,
            };
            var result = await service.UpdateAsync(user, Guid.Parse(organId), Guid.Parse(roleId));
            Assert.IsType<Network.API.ViewModel.Sys_User.Detail>(result);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111")]
        [InlineData("11111111-1111-1111-1111-111111111110")]
        public async Task GetByOrganIdAsync(string organId)
        {
            var service = await CreateServiceAsync();
            var result = await service.GetByOrganIdAsync(Guid.Parse(organId));
            Assert.IsType<List<Network.API.ViewModel.Sys_User.ListByOrganId>>(result);
        }

        [Theory]
        [InlineData("admin", "1")]
        public async Task CheckUserLogin(string userName, string password)
        {
            var service = await CreateServiceAsync();
            var result = await service.CheckUserLogin(userName, password);
            Assert.IsType<LoginResult>(result);
        }

        [Theory]
        [InlineData("admin")]
        public async Task CheckUserExisted(string userName)
        {
            var service = await CreateServiceAsync();
            var result = await service.CheckUserNameExists(userName);
            Assert.IsType<LoginResult>(result);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "2")]
        public async Task UserChangePassword(string userId, string passWord)
        {
            var service = await CreateServiceAsync();
            await service.UserChangePassword(Guid.Parse(userId), passWord);
            Assert.True(true);
        }

        [Theory]
        [InlineData("admin")]
        public async Task GetUserInfo(string userName)
        {
            var service = await CreateServiceAsync();
            var result = await service.GetUserInfo(userName);
            Assert.IsType<UserInfo>(result);
        }

        [Theory]
        [InlineData("admin")]
        public async Task CheckUserRefreshToken(string userName)
        {
            var service = await CreateServiceAsync();
            var result = await service.CheckUserRefreshToken(userName);
            Assert.IsType<LoginResult>(result);
        }

        #region Init Service
        private async Task<Network.API.Service.Sys_User.Service> CreateServiceAsync()
        {            
            var _contextOptions = new DbContextOptionsBuilder<DomainDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))                                
                                .Options;
            DomainDbContext _context = new DomainDbContext(_contextOptions);            
            await PopulateDataAsync(_context);
            return new Network.API.Service.Sys_User.Service(_context, _dateTimeProvider.Object, _userProvider.Object);
        }
        private async Task PopulateDataAsync(DomainDbContext context)
        {         
            //User
            var items_Sys_User = new List<Sys_User>();
            items_Sys_User.Add(new Sys_User() {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FullName = "admin",
                UserName = "admin",
                PassWord = "6tz2HBlFEwTPKDINnAGA4Q==",
                Email = "admin@gmail.com",
                Phone = "012356789",
                Address = "hn",
                IsActive = true,
                IsSystem = true,
                CreatedDateTime = _dateTimeProvider.Object.OffsetNow,
                CreatedBy = _userProvider.Object.UserName
            });
            items_Sys_User.Add(new Sys_User() {                
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                FullName = "vhbao",
                UserName = "vhbao",
                PassWord = "6tz2HBlFEwTPKDINnAGA4Q==",
                Email = "vhbao@gmail.com",
                Phone = "012356789",
                Address = "hn",
                IsActive = true,
                IsSystem = false,
                CreatedDateTime = _dateTimeProvider.Object.OffsetNow,
                CreatedBy = _userProvider.Object.UserName,
            });
            await context.Sys_Users.AddRangeAsync(items_Sys_User);
            //Role
            var items_Sys_Role = new List<Sys_Role>();
            items_Sys_Role.Add(new Sys_Role()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Code = "admin",
                Name = "admin",
                CreatedDateTime = _dateTimeProvider.Object.OffsetNow,
                CreatedBy = _userProvider.Object.UserName
            });
            await context.Sys_Roles.AddRangeAsync(items_Sys_Role);
            //Organization
            var items_Sys_Organization = new List<Sys_Organization>();
            items_Sys_Organization.Add(new Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ParentId = Guid.Empty,
                Type = Network.Core.Enums.OrganizationType.Organization,
                Code = "organ",
                Name = "organ",
                CreatedDateTime = _dateTimeProvider.Object.OffsetNow,
                CreatedBy = _userProvider.Object.UserName
            });
            await context.Sys_Organizations.AddRangeAsync(items_Sys_Organization);
            //User_Role
            var items_Sys_User_Role = new List<Sys_User_Role>();
            items_Sys_User_Role.Add(new Sys_User_Role()
            {
                Id = Guid.NewGuid(),
                IsDefault = true,
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                OrganId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            });
            items_Sys_User_Role.Add(new Sys_User_Role()
            {
                Id = Guid.NewGuid(),
                IsDefault = true,
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                OrganId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            });
            await context.Sys_Users_Roles.AddRangeAsync(items_Sys_User_Role);
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
