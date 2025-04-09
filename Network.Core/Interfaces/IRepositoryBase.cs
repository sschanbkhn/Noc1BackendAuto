using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.Core.Interfaces
{
    public interface IRepositoryBase<TEntity>
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TEntity> SaveEntityAsync(TEntity entity);
        Task<TEntity[]> SaveEntitiesAsync(TEntity[] entity);        
        Task AddOrUpdateAsync(TEntity entity);
        Task<Paged<TEntity>> GetPagedAsync(int page, int pageSize, int totalLimitItems, string search);
        Task<TEntity> GetByIdAsync(Guid id);
        Task DeleteSave(List<TEntity> entity);
        void Delete(List<TEntity> entity);
        List<TEntity> GetCategories();
    }
}
