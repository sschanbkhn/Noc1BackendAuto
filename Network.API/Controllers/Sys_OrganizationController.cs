using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network.API.Infrastructure.Authorization;
using Network.API.Service;
using Network.API.Controllers;
using Network.API.Model;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.Core.Interfaces;
using Network.Core.Constant;

namespace Network.API.Controllers
{
    public class Sys_OrganizationController : ApiControllerBase<Sys_Organization>
    {
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_CategoryController> _logger;
        private readonly IUserProvider _userProvider;
        public Sys_OrganizationController(IServiceWrapper service, IUserProvider userProvider, ILogger<Sys_CategoryController> logger) :base(service, logger)
        {
            _logger = logger;
            _service = service;
            _userProvider = userProvider;
        }
        [HttpGet("CheckDuplicateAttributes")]
        [AuthorizeFilter]
        public async Task<IActionResult> CheckDuplicateAttributes(Guid? id, string code, Guid parentId)
        {
            try
            {
                _logger.LogInformation(string.Format("Call CheckDuplicateAttributes params: (id = {0}, code = {1}, parentId = {2})", id, code, parentId));
                var result = await _service.Sys_Organization.IsDupicateAttributesAsync(id, code, parentId);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("CheckDuplicateAttributes : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("Tree")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetTree()
        {
            try
            {
                var userid = _userProvider.Id;
                var organ = _service.Sys_User.GetDetailByIdAsync(userid).Result.OrganId.ToString();
                _logger.LogInformation("Call GetTree");
                List<ViewModel.Sys_Organization.OrganTree> treeOrgan = null;
                if (userid.ToString().Equals(Sys_Const.GuidAdmin))
                { treeOrgan = await _service.Sys_Organization.GetTreeAsync(); }
                else { treeOrgan = await _service.Sys_Organization.GetTreeAsyncUser(organ); }
                return ResponseMessage.Success(treeOrgan);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetTree : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("TreeList")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetTreeList()
        {
            try
            {
                _logger.LogInformation("Call GetTreeList");
                var treeOrgan = await _service.Sys_Organization.GetTreeListAsync();
                return ResponseMessage.Success(treeOrgan);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetTreeList : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("List/{ParentId}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetByPerentId(Guid parentId)
        {
            try
            {
                _logger.LogInformation(string.Format("Call GetByPerentId params: (parentId = {0})", parentId));
                var items = await _service.Sys_Organization.GetByParentIdAsync(parentId);
                return ResponseMessage.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetByPerentId : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpDelete("DeleteById/{Id}")]
        [AuthorizeFilter]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            try
            {
                _logger.LogInformation(string.Format("Call DeleteById params: (Id = {0})", Id));
                await _service.Sys_Organization.DeleteById(Id);
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("DeleteById : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
