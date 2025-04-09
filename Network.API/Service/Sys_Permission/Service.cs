using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.API.ViewModel.Sys_Permission;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_Permission
{
    public class Service:RepositoryBase<Model.Sys_Permission>, Sys_Permission.IService
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
        public async Task<List<Model.Sys_Permission>> SaveAsync(Guid[] resourceIds, Guid roleId, bool isFunc)
        {
            try
            {
                if (resourceIds == null) return null;
                UnitOfWork.CreateTransaction();
                List<Model.Sys_Permission> permissions = new List<Model.Sys_Permission>();
                Model.Sys_Permission permission = null;
                for (int i = 0; i < resourceIds.Length; i++)
                {
                    permission = new Model.Sys_Permission();
                    permission.Id = Guid.NewGuid();
                    permission.IsFunc = isFunc;
                    permission.ResourceId = resourceIds[i];
                    permission.RoleId = roleId;
                    permission.CreatedBy = _userProvider.UserName;
                    permission.CreatedDateTime = _dateTimeProvider.OffsetNow;
                    permissions.Add(permission);
                }
                _dbContext.Sys_Permissions.RemoveRange(_dbContext.Sys_Permissions.Where(o => o.RoleId == roleId && o.IsFunc == isFunc).ToList());
                await _dbContext.Sys_Permissions.AddRangeAsync(permissions);
                await UnitOfWork.SaveAsync();
                UnitOfWork.Commit();
                return permissions;
            }
            catch(Exception ex)
            {
                UnitOfWork.Roolback();
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<List<Model.Sys_Permission>> GetByRoleIdAsync(Guid roleId, bool isFunc)
        {
            return await _dbContext.Sys_Permissions.Where(o => o.RoleId == roleId && o.IsFunc == isFunc).ToListAsync();
        }
        public async Task<MenuList> GetMenusByRoles(List<string> rolesCode)
        {
            var roles = await _dbContext.Sys_Roles.Where(o => rolesCode.Contains(o.Code)).ToListAsync();
            var menus = await _dbContext.Sys_Resources.Where(o => o.Type == Core.Enums.ResourceType.Menu).ToListAsync();
            var permMenus = await (from x in _dbContext.Sys_Permissions
                                  join y in _dbContext.Sys_Resources on x.ResourceId equals y.Id
                                  where roles.Select(o => o.Id).Contains(x.RoleId)
                                  select new { y.Code, y.ParentId }).ToListAsync();
            var parentMenus = await _dbContext.Sys_Resources.Where(o => permMenus.Select(e => e.ParentId).Contains(o.Id)).ToListAsync();
            HashSet<string> rs = new HashSet<string>();
            foreach(var o in permMenus)
            {
                rs.Add(o.Code);
            }
            foreach (var o in parentMenus)
            {
                rs.Add(o.Code);
            }
            MenuList menuList = new MenuList();
            menuList.Roles = roles.Select(o => o.Code).ToArray();
            menuList.Menus = rs.ToList();
            return menuList;
        }
    }
}
