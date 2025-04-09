using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_User
{
    public interface IService: IRepositoryBase<Model.Sys_User>
    {        
        Task<LoginResult> CheckUserLogin(string UserName, string Password);
        Task<LoginResult> CheckUserLogin(string Email);
        Task<bool> CheckUserNameExists(string UserName);
        Task<bool> CheckEmailExists(string Email);
        Task UserChangePassword(Guid UserId, string PassWord);
        Task UserChangePasswordNew(string Code, string PassWordNew);
        Task<LoginResult> CheckUserRefreshToken(string UserName);
        Task<UserInfo> GetUserInfo(string UserName);
        Task EditUserInfo(EditUserInfo UserInfo);
        Task<string> GetCodeByEmailAsync(string Email);
        Task<List<ViewModel.Sys_User.ListByOrganId>> GetByOrganIdAsync(Guid OrganId);
        Task<ViewModel.Sys_User.Detail> GetDetailByIdAsync(Guid Id);
        Task<List<ViewModel.Sys_User.UsersWithRoleOrgan>> GetUsersWithRoleOrgan();
        Task<bool> IsDupicateAttributesAsync(Guid? Id, string UserName, string Email);
        Task<ViewModel.Sys_User.Detail> CreateAsync(Model.Sys_User user, Guid? OrganId, Guid? RoleId);
        Task<ViewModel.Sys_User.Detail> UpdateAsync(Model.Sys_User user, Guid? OrganId, Guid? RoleId);
        Task DeleteById(Guid Id);
    }
}
