using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Network.API.Service;

namespace Network.API.Infrastructure.Authorization
{
    public class AuthorizeFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userName = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            //if (!string.IsNullOrEmpty(userId) && userName != "admin")
            //{
            //    string controller = (string)context.RouteData.Values["controller"];
            //    string action = (string)context.RouteData.Values["action"];
            //    var dbContext = context.HttpContext.RequestServices.GetService(typeof(DomainDbContext)) as DomainDbContext;
            //    var controller_Resource = dbContext.Sys_Resources.Where(o => o.Code == controller+"Controller" && o.Type == Core.Enums.ResourceType.Function).FirstOrDefault();
            //    var action_Resource = dbContext.Sys_Resources.Where(o => o.Code == action && o.ParentId == controller_Resource.Id && o.Type == Core.Enums.ResourceType.Function).FirstOrDefault();
            //    var isApiOK = (from x in dbContext.Sys_Users_Roles
            //                join y in dbContext.Sys_Users on x.UserId equals y.Id
            //                join z in dbContext.Sys_Roles on x.RoleId equals z.Id
            //                join p in dbContext.Sys_Permissions on z.Id equals p.RoleId
            //                where p.IsFunc == true && (p.ResourceId == controller_Resource.Id || p.ResourceId == action_Resource.Id) && y.Id.ToString() == userId
            //                select x.UserId).Any();
                //if(!isApiOK)
                //{
                //    context.Result = new UnauthorizedResult();
                //}    
            //}    
        }
    }
}
