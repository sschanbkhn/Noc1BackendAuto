using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Core;
using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Network.Core.Helpers;

namespace Network.API.Service.Sys_Resource
{
    public class Service:RepositoryBase<Model.Sys_Resource>, Sys_Resource.IService
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
        public async Task<List<ViewModel.Sys_Resource.ResourceTree>> GetMenuTreeAsync()
        {
            List<ViewModel.Sys_Resource.ResourceTree> items = await _dbContext.Sys_Resources.Where(o => o.Type == Core.Enums.ResourceType.Menu || o.Type == Core.Enums.ResourceType.SubMenu).Select(o =>
                new ViewModel.Sys_Resource.ResourceTree() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToListAsync();
            List<ViewModel.Sys_Resource.ResourceTree> trees = TreeHelpers<ViewModel.Sys_Resource.ResourceTree>.ListToTrees(items);
            return trees;
        }
        public async Task<List<ViewModel.Sys_Resource.ResourceTree>> GetFunctionTreeAsync()
        {
            List<ViewModel.Sys_Resource.ResourceTree> items = await _dbContext.Sys_Resources.Where(o => o.Type == Core.Enums.ResourceType.Function).Select(o =>
                new ViewModel.Sys_Resource.ResourceTree() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToListAsync();
            List<ViewModel.Sys_Resource.ResourceTree> trees = TreeHelpers<ViewModel.Sys_Resource.ResourceTree>.ListToTrees(items);
            return trees;
        }
        public async Task<List<Model.Sys_Resource>> InitFunctionAsync()
        {            
            Model.Sys_Resource resourceParent;
            Model.Sys_Resource resourceChild;
            List<Model.Sys_Resource> resources = new List<Model.Sys_Resource>();
            string assemblyFile = Assembly.GetExecutingAssembly().Location; 
            IEnumerable<string> actions = ReflectionUtil.GetActionsWithController(assemblyFile);
            IEnumerable<string> controllers = ReflectionUtil.GetControllers(assemblyFile);
            int countControllers = controllers.Count();
            int countActions = actions.Count();
            for (int i = 0;i < countControllers; i++)
            {
                resourceParent = new Model.Sys_Resource();
                resourceParent.Id = Guid.NewGuid();
                resourceParent.Code = controllers.ElementAt(i);
                resourceParent.Name = controllers.ElementAt(i);
                resourceParent.Type = Core.Enums.ResourceType.Function;
                resourceParent.ParentId = Guid.Empty;
                resourceParent.CreatedBy = _userProvider.UserName;
                resourceParent.CreatedDateTime = _dateTimeProvider.OffsetNow;
                resources.Add(resourceParent);
                for (int j = 0;j < countActions; j++)
                {                    
                    if (actions.ElementAt(j).Contains(controllers.ElementAt(i)))
                    {
                        string[] splitAction = actions.ElementAt(j).Split(".");
                        resourceChild = new Model.Sys_Resource();
                        resourceChild.Id = Guid.NewGuid();
                        resourceChild.Code = splitAction[1];
                        resourceChild.Name = splitAction[1];
                        resourceChild.Type = Core.Enums.ResourceType.Function;
                        resourceChild.ParentId = resourceParent.Id;
                        resourceChild.CreatedBy = _userProvider.UserName;
                        resourceChild.CreatedDateTime = _dateTimeProvider.OffsetNow;
                        resources.Add(resourceChild);
                    }
                }
            }
            await _dbContext.Sys_Resources.AddRangeAsync(resources);
            await UnitOfWork.SaveAsync();
            return resources;
        }
        public async Task<List<Model.Sys_Resource>> InitMenuAsync(List<MenuConfig> menu)
        {
            if (menu == null)            
                return null;            
            List<Model.Sys_Resource> resources = new List<Model.Sys_Resource>();
            Model.Sys_Resource resourceParent;
            Model.Sys_Resource resourceChild;            
            for (int i = 0; i < menu.Count; i++)
            {
                resourceParent = new Model.Sys_Resource();
                resourceParent.Id = Guid.NewGuid();
                resourceParent.Code = menu[i].Code;
                resourceParent.Name = menu[i].Name;
                resourceParent.Url = menu[i].Url;
                resourceParent.Icon = menu[i].Icon;
                resourceParent.Type = Core.Enums.ResourceType.Menu;
                resourceParent.ParentId = Guid.Empty;
                resourceParent.CreatedBy = _userProvider.UserName;
                resourceParent.CreatedDateTime = _dateTimeProvider.OffsetNow;
                resources.Add(resourceParent);
                var childrensBig = menu[i].SubMenu;
                if (childrensBig != null)
                {
                    for (int j = 0; j < childrensBig.Length; j++)
                    {
                        resourceChild = new Model.Sys_Resource();
                        resourceChild.Id = Guid.NewGuid();
                        resourceChild.Code = childrensBig[j].Code;
                        resourceChild.Name = childrensBig[j].Name;
                        resourceChild.Url = childrensBig[j].Url;
                        resourceChild.Icon = childrensBig[j].Icon;
                        resourceChild.Type = Core.Enums.ResourceType.SubMenu;
                        resourceChild.ParentId = resourceParent.Id;
                        resourceChild.CreatedBy = _userProvider.UserName;
                        resourceChild.CreatedDateTime = _dateTimeProvider.OffsetNow;
                        resources.Add(resourceChild);
                    }
                }
            }
            await _dbContext.Sys_Resources.AddRangeAsync(resources);
            await UnitOfWork.SaveAsync();
            return resources;
        }
        public async Task DeleteAllMenu()
        {
            _dbContext.Sys_Resources.RemoveRange(_dbContext.Sys_Resources.Where(o => o.Type == Core.Enums.ResourceType.Menu || o.Type == Core.Enums.ResourceType.SubMenu).ToList());
            await UnitOfWork.SaveAsync();
        }
        public async Task DeleteAllFunction()
        {
            _dbContext.Sys_Resources.RemoveRange(_dbContext.Sys_Resources.Where(o => o.Type == Core.Enums.ResourceType.Function));
            await UnitOfWork.SaveAsync();
        }
    }
}
