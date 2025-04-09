using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Network.API.Infrastructure;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Network.Core.Core;
using Network.Core.Constant;

namespace Network.API.Service
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AuditEntity
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _dbContext;
            }
        }

        public RepositoryBase(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;            
            _userProvider = userService;
        }
        public async Task AddOrUpdateAsync(T entity)
        {
            var existingItem = await _dbContext.Set<T>().FirstOrDefaultAsync(o => o.Id == entity.Id);
            DateTimeOffset now = _dateTimeProvider.OffsetNow;
            string userName = string.IsNullOrEmpty(_userProvider.UserName) ? "Guest" : _userProvider.UserName;
            if (existingItem != null)
            {
                entity.UpdatedDateTime = now.AddHours(Sys_Const.TimeZone);
                entity.UpdatedBy = userName;
                _dbContext.Entry(existingItem).CurrentValues.SetValues(entity);
            }
            else
            {
                entity.Id = Guid.Empty;
                entity.CreatedDateTime = now.AddHours(Sys_Const.TimeZone);
                entity.CreatedBy = userName;
                await _dbContext.Set<T>().AddAsync(entity);
            }
        }
        public async Task<T> SaveEntityAsync(T entity)
        {
            var existingItem = await _dbContext.Set<T>().FirstOrDefaultAsync(o => o.Id == entity.Id);
            DateTimeOffset now = _dateTimeProvider.OffsetNow;
            string userName = string.IsNullOrEmpty(_userProvider.UserName) ? "Guest" : _userProvider.UserName;
            if (existingItem != null)
            {
                entity.UpdatedDateTime = now.AddHours(Sys_Const.TimeZone);
                entity.UpdatedBy = userName;
                _dbContext.Entry(existingItem).CurrentValues.SetValues(entity);
            }
            else
            {
                entity.Id = Guid.NewGuid();
                entity.CreatedDateTime = now.AddHours(Sys_Const.TimeZone);
                entity.CreatedBy = userName;                
                await _dbContext.Set<T>().AddAsync(entity);
            }
            await UnitOfWork.SaveAsync();
            return entity;
        }
        public async Task<T[]> SaveEntitiesAsync(T[] entities)
        {
            foreach (var entity in entities)
            {
                var existingItem = await _dbContext.Set<T>().FirstOrDefaultAsync(o => o.Id == entity.Id);
                DateTimeOffset now = _dateTimeProvider.OffsetNow;
                string userName = string.IsNullOrEmpty(_userProvider.UserName) ? "Guest" : _userProvider.UserName;
                if (existingItem != null)
                {
                    entity.UpdatedDateTime = now.AddHours(Sys_Const.TimeZone);
                    entity.UpdatedBy = userName;
                    _dbContext.Entry(existingItem).CurrentValues.SetValues(entity);
                }
                else
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDateTime = now.AddHours(Sys_Const.TimeZone);
                    entity.CreatedBy = userName;
                    await _dbContext.Set<T>().AddAsync(entity);
                }
            }
            await UnitOfWork.SaveAsync();
            return entities;
        }
        public async Task<Paged<T>> GetPagedAsync(int page, int pageSize, int totalLimitItems, string search)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(search);
            }
            Paged<T> result = new Paged<T>(query, page, pageSize, totalLimitItems);
            result.Items = await query.Paged(page, pageSize, totalLimitItems).ToListAsync();
            return result;
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(o => o.Id == id);
        }
        public void Delete(List<T> entity)
        {
            DbSet.RemoveRange(DbSet.Where(o => entity.Select(o => o.Id).Contains(o.Id)));            
        }
        public async Task DeleteSave(List<T> entity)
        {
            DbSet.RemoveRange(DbSet.Where(o => entity.Select(o => o.Id).Contains(o.Id)));
            await UnitOfWork.SaveAsync();
        }
        public List<T> GetCategories()
        {
            List<string> columnNames = ReflectionUtil.GetColumnNameAttr<T>("category");
            if (columnNames.Count == 0)
                return null;
            string strColumns = ListHelpers.ConcatStrings(columnNames);
            var result = _dbContext.Set<T>().Select(LinQHelpers.DynamicSelectGenerator<T>(strColumns)).ToList();
            return result;
        }
    }
}
