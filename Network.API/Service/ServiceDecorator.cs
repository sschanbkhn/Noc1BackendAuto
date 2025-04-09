using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Network.API.Service
{
    class ServiceDecorator<TEntity>
    {
        private IRepositoryBase<TEntity> _serviceBase;
        public ServiceDecorator(IServiceWrapper service)
        {
            #region add service
            if (typeof(TEntity) == typeof(Model.Sys_Category))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Category;
            }
            else if(typeof(TEntity) == typeof(Model.Sys_User))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_User;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_File))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_File;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Organization))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Organization;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Role))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Role;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Config))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Config;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Permission))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Permission;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Resource))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Resource;
            }
            else if (typeof(TEntity) == typeof(Model.Sys_Notification))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Sys_Notification;
            }
            else if (typeof(TEntity) == typeof(Model.Net_AlarmType))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_AlarmType;
            }
            else if (typeof(TEntity) == typeof(Model.Net_CableManagement))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_CableManagement;
            }
            else if (typeof(TEntity) == typeof(Model.Net_ConfigurationLogs))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_ConfigurationLogs;
            }
            else if (typeof(TEntity) == typeof(Model.Net_CurenAlarm))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_CurenAlarm;
            }
            else if (typeof(TEntity) == typeof(Model.Net_DevicePorts))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_DevicePorts;
            }
            else if (typeof(TEntity) == typeof(Model.Net_Devices))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_Devices;
            }
            else if (typeof(TEntity) == typeof(Model.Net_DeviceTypes))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_DeviceTypes;
            }
            else if (typeof(TEntity) == typeof(Model.Net_HistoryCurenAlarm))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_HistoryCurenAlarm;
            }
            else if (typeof(TEntity) == typeof(Model.Net_Manufacturers))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_Manufacturers;
            }
            else if (typeof(TEntity) == typeof(Model.Net_NetworkLinks))
            {
                _serviceBase = (IRepositoryBase<TEntity>)service.Net_NetworkLinks;
            }
            #endregion
        }
        public async Task<TEntity> SaveEntityAsync(TEntity entity)
        {
            return await _serviceBase.SaveEntityAsync(entity);
        }
        public async Task<TEntity[]> SaveEntitiesAsync(TEntity[] entities)
        {
            return await _serviceBase.SaveEntitiesAsync(entities);
        }        
        public async Task<Paged<TEntity>> GetPagedAsync(int page, int pageSize, int totalLimitItems, string search)
        {
            return await _serviceBase.GetPagedAsync(page, pageSize, totalLimitItems, search);
        }
        public List<TEntity> GetCategories()
        {
            return _serviceBase.GetCategories();
        }
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _serviceBase.GetByIdAsync(id);
        }
        public async Task Delete(List<TEntity> entity)
        {
            await _serviceBase.DeleteSave(entity);
        }
    }
}
