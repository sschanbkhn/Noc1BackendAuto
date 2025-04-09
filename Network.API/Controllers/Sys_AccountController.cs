using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Network.API.Infrastructure.Authentication;
using Network.Core.Interfaces;
using Network.API.Service;
using Network.Core.Models;
using Network.Core.Constant;
using System.Security.Claims;
using Network.Core.Helpers;
using Network.Core.Core;
using Microsoft.Extensions.Configuration;
using Network.API.Infrastructure.Authorization;
using Network.API.Model;
using Newtonsoft.Json;
using Google.Apis.Auth;
using System.Collections.Generic;
using Network.API.Infrastructure;
using FacebookCore;
using Network.InfraCore.Email;

namespace Network.API.Controllers
{
    [ApiController]
    [AuthorizeFilter]
    [Route("api/[controller]")]
    public class Sys_AccountController : ControllerBase
    {        
        private readonly IUserProvider _userProvider;        
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_AccountController> _logger;
        private readonly AppSettings appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly HubConnection connection;
        public Sys_AccountController(IServiceWrapper service, IUserProvider userService, IJwtAuthManager jwtAuthManager, ILogger<Sys_AccountController> logger, IConfiguration rootConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            appSettings = new AppSettings();
            rootConfiguration.Bind(appSettings);
            _userProvider = userService;            
            _jwtAuthManager = jwtAuthManager;
            _service = service;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //string myHostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            //connection = new HubConnectionBuilder()
            //    .WithUrl(myHostUrl + "/hubs/notification")
            //    .Build();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Login body: ({0})", JsonConvert.SerializeObject(request)));
                if (!UserHelpers.IsValidUserLogin(request.UserName, request.Password, out string message))
                {
                    throw new Exception(message);                    
                }
                var result = await _service.Sys_User.CheckUserLogin(request.UserName, request.Password);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, result.UserName),                    
                };

                var jwtResult = _jwtAuthManager.GenerateTokens(result.UserName, claims, DateTime.Now);                
                result.AccessToken = jwtResult.AccessToken;
                Sys_AuthToken authToken = new Sys_AuthToken();                
                authToken.AccessToken = jwtResult.AccessToken;
                authToken.RefeshToken = jwtResult.RefreshToken.TokenString;
                await _service.Sys_AuthToken.SaveByUserNameAsync(result.UserName, authToken);                
                return ResponseMessage.Success(result, Sys_Const.Message.SERVICE_LOGIN_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Login : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("SocialLogin")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format("Call SocialLogin body: ({0})", JsonConvert.SerializeObject(request)));
                var isExists = await _service.Sys_User.CheckEmailExists(request.Email);
                if(isExists == false)
                {
                    return ResponseMessage.Success(false);
                }
                var isLoggin = false;
                if (!string.IsNullOrEmpty(request.Token_Google))
                {
                    var verifySuccess = VerifyGoogleToken(request.Token_Google, request.ClientId);
                    if (verifySuccess != null)
                    {
                        isLoggin = true;
                    }
                }
                else if (!string.IsNullOrEmpty(request.Token_Facebook))
                {
                    var verifySuccess = VerifyFacebookToken(request.Token_Facebook, request.ClientId, request.ClientSecret);
                    if (verifySuccess != null)
                    {
                        isLoggin = true;
                    }
                }
                if(isLoggin)
                {
                    var result = await _service.Sys_User.CheckUserLogin(request.Email);
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, result.UserName),
                };

                    var jwtResult = _jwtAuthManager.GenerateTokens(result.UserName, claims, DateTime.Now);
                    result.AccessToken = jwtResult.AccessToken;
                    Sys_AuthToken authToken = new Sys_AuthToken();
                    authToken.AccessToken = jwtResult.AccessToken;
                    authToken.RefeshToken = jwtResult.RefreshToken.TokenString;
                    await _service.Sys_AuthToken.SaveByUserNameAsync(result.UserName, authToken);
                    return ResponseMessage.Success(result, Sys_Const.Message.SERVICE_LOGIN_SUCCESS);
                }    
                return ResponseMessage.Success(null, Sys_Const.Message.SERVICE_LOGIN_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Login : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string Token_Google, string ClientId)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Clock = new clock(),
                    Audience = new List<string>() { ClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(Token_Google, settings);
                return payload;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<object> VerifyFacebookToken(string Token_Facebook, string ClientId, string ClientSecret)
        {
            try
            {
                FacebookClient client = new FacebookClient(ClientId, ClientSecret);

                var validator = client.GetUserApi(Token_Facebook);
                if (validator != null)
                {
                    string[] ls = new string[] { "id", "first_name", "last_name" };
                    var req_info = await validator.RequestInformationAsync(ls);
                    var facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(Newtonsoft.Json.JsonConvert.SerializeObject(req_info));
                    return facebookUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }
        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Signup body: ({0})", JsonConvert.SerializeObject(request)));
                if (!UserHelpers.IsValidUserLogin(request.UserName, request.PassWord, out string message))
                {
                    throw new Exception(message);
                }
                var isUserNameExisted = await _service.Sys_User.CheckUserNameExists(request.UserName);
                if (isUserNameExisted)
                {
                    throw new Exception(Sys_Const.Message.SERVICE_USERNAME_EXISTS);
                }
                //var isEmailExisted = await _service.Sys_User.CheckEmailExists(request.Email);
                //if (isEmailExisted)
                //{
                //    throw new Exception(Sys_Const.Message.SERVICE_EMAIL_EXISTS);
                //}
                Sys_User userNew = new Sys_User();
                ObjectHelpers.Mapping<SignupRequest, Sys_User>(request, userNew);
                userNew.UserName = userNew.UserName.RemoveAllWhitespace();
                userNew.Email = userNew.Email.RemoveAllWhitespace();
                userNew.PassWord = Cryption.EncryptByKey(userNew.PassWord, Sys_Const.Security.key);
                if (userNew.UserName == "admin") { 
                    userNew.IsActive = true; 
                    userNew.IsSystem = true; 
                }
                else
                {
                    userNew.IsActive = true;
                    userNew.IsSystem = false;
                }    
                userNew = await _service.Sys_User.SaveEntityAsync(userNew);
                //
                //await SendNoti_RegisteredUsers(userNew.Id, userNew.Email);
                return ResponseMessage.Success(userNew, Sys_Const.Message.SERVICE_SIGNUP_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Signup : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        private async Task SendNoti_RegisteredUsers(Guid userId, string email)
        {
            try
            {
                Sys_Notification oNotification = new Sys_Notification();
                oNotification.Type = Core.Enumeration.Sys_Enum.NotificationType.Info;
                oNotification.Title = "Yêu cầu kích hoạt tài khoản !";
                oNotification.Content = "Email kích hoạt: " + email;
                oNotification.ObjectId = userId.ToString();
                oNotification.ObjectType = Enums.NotiType.RegisteredUser.ToString();
                oNotification.Receiver = "admin";
                await _service.Sys_Notification.SaveEntityAsync(oNotification);
                //await connection.StartAsync();
                //await connection.InvokeAsync("SendVerifyRegisteredUsers", email);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format("Call ChangePassword body: ({0})", JsonConvert.SerializeObject(request)));
                if (!UserHelpers.IsValidUserChangePassword(_userProvider.UserName, request.PasswordOld, request.PasswordNew, out string message))
                {
                    return ResponseMessage.Error(message);
                }
                var user = await _service.Sys_User.CheckUserLogin(_userProvider.UserName, request.PasswordOld);
                await _service.Sys_User.UserChangePassword(user.UserId, request.PasswordNew);              
                return ResponseMessage.Success(Sys_Const.Message.SERVICE_CHANGEPASSWORD_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ChangePassword : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("Logout")]
        [AllowAnonymous]
        public IActionResult Logout(string userName)
        {
            try
            {
                _logger.LogInformation(string.Format("Call Logout params: (userName = {0})", userName));
                _jwtAuthManager.RemoveRefreshTokenByUserName(userName);                
                return ResponseMessage.Success(Sys_Const.Message.SERVICE_EDIT_PROFILE_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Logout : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpPost("EditInfo")]
        [AuthorizeFilter]
        public async Task<IActionResult> EditInfo([FromBody] EditUserInfo userInfo)
        {
            try
            {
                _logger.LogInformation("Call EditInfo");
                await _service.Sys_User.EditUserInfo(userInfo);
                return ResponseMessage.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("EditInfo : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }

        [HttpGet("Info")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetInfo()
        {
            try
            {
                _logger.LogInformation("Call GetInfo");
                var userName = _userProvider.UserName;                
                var userInfo = await _service.Sys_User.GetUserInfo(userName);
                return ResponseMessage.Success(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("GetInfo : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("ChangePasswordNew")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordNew([FromBody] ChangePasswordNewRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format("Call ChangePasswordNewRequest body: ({0})", JsonConvert.SerializeObject(request)));
                await _service.Sys_User.UserChangePasswordNew(request.Code, request.PasswordNew);
                return ResponseMessage.Success(Sys_Const.Message.SERVICE_CHANGEPASSWORD_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ChangePasswordNewRequest : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("SendEmailRestorePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SendEmailRestorePassword([FromBody] SendEmailRestorePasswordRequest inputRequest)
        {
            try
            {
                _logger.LogInformation("Call RestorePassword");
                NetMailProvider netMailProvider = new NetMailProvider(appSettings.MailConfig.Host, appSettings.MailConfig.Port);
                netMailProvider.SetSender(appSettings.MailConfig.Email, appSettings.MailConfig.Password);
                EmailService emailService = new EmailService();
                emailService.SetEmailProvider(netMailProvider);
                string errorMessage = string.Empty;
                var code = await _service.Sys_User.GetCodeByEmailAsync(inputRequest.Email);
                if(string.IsNullOrEmpty(code))
                {
                    return ResponseMessage.Error(Sys_Const.Message.SERVICE_RESTORE_PASSWORD_ERROR);
                }                
                emailService.Send(inputRequest.Email, "Wolf2 khôi phục lại mật khẩu", "link khôi phục: " + inputRequest.Address + "?code=" + code, ref errorMessage);
                if(!string.IsNullOrEmpty(errorMessage))
                {
                    return ResponseMessage.Error(errorMessage);
                }    

                return ResponseMessage.Success(Sys_Const.Message.SERVICE_RESTORE_PASSWORD_SUCCESS);
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("RestorePassword : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                _logger.LogInformation(string.Format("Call RefreshToken body: ({0})", JsonConvert.SerializeObject(refreshTokenRequest)));
                refreshTokenRequest.RefreshToken = (await _service.Sys_AuthToken.GetByUserNameAsync(refreshTokenRequest.UserName)).RefeshToken;
                var user = await _service.Sys_User.CheckUserRefreshToken(refreshTokenRequest.UserName);
                if (user == null)
                {
                    throw new Exception(Sys_Const.Message.SERVICE_REFRESH_ERROR);
                }
                var accessToken = refreshTokenRequest.AccessToken;
                var jwtResult = _jwtAuthManager.Refresh(refreshTokenRequest.RefreshToken, accessToken, DateTime.Now);
                Sys_AuthToken authToken = new Sys_AuthToken();
                authToken.AccessToken = jwtResult.AccessToken;
                authToken.RefeshToken = jwtResult.RefreshToken.TokenString;
                await _service.Sys_AuthToken.SaveByUserNameAsync(refreshTokenRequest.UserName, authToken);
                RefreshTokenResponse refreshTokenResponse = new RefreshTokenResponse();
                refreshTokenResponse.AccessToken = authToken.AccessToken;
                return ResponseMessage.Success(refreshTokenResponse, Sys_Const.Message.SERVICE_REFRESH_SUCCESS);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(string.Format("RefreshToken : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message, null, 401);
            }
        }
    }
}
