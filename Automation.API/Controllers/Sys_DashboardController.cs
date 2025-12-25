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

namespace Network.API.Controllers
{
    [ApiController]
    [AuthorizeFilter]
    [Route("api/[controller]")]
    public class Sys_DashboardController : ControllerBase
    {                           
        private readonly IServiceWrapper _service;
        private readonly ILogger<Sys_DashboardController> _logger;
        public Sys_DashboardController(IServiceWrapper service, ILogger<Sys_DashboardController> logger)
        {                                            
            _service = service;
            _logger = logger;
        }
        [HttpGet("ThongKeSoLuongNguoiTest/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> ThongKeSoLuongNguoiTest(int month, int year, string donvi)
        {
            try
            {
                _logger.LogInformation("Call ThongKeSoLuongNguoiTest");
                var result = await _service.Speed_DataOkla.ThongKeSoLuongNguoiTestAsync(month, year, donvi);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeSoLuongNguoiTest : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("ThongKeSoLuongTestDatNguong/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> ThongKeSoLuongTestDatNguong(int month, int year, string donvi)
        {
            try
            {
                _logger.LogInformation("Call ThongKeSoLuongTestDatNguong");
                var result = await _service.Speed_DataOkla.ThongKeSoLuongTestDatNguongAsync(month, year, donvi);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ThongKeSoLuongTestDatNguong : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("Top10NguoiTestNangNo/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> Top10NguoiTestNangNo(int month, int year, string donvi)
        {
            try
            {
                _logger.LogInformation("Call Top10NguoiTestNangNo");
                var result = await _service.Speed_DataOkla.Top10NguoiTestNangNoAsync(month, year, donvi);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Top10NguoiTestNangNo : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("Top10NguoiTestDownloadTrungVi/{month}/{year}/{donvi}")]
        [AuthorizeFilter]
        public async Task<IActionResult> Top10NguoiTestDownloadTrungVi(int month, int year, string donvi)
        {
            try
            {
                _logger.LogInformation("Call Top10NguoiTestDownloadTrungVi");
                var result = await _service.Speed_DataOkla.Top10NguoiTestDownloadTrungViAsync(month, year, donvi);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Top10NguoiTestDownloadTrungVi : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
        [HttpGet("NhanVienTheoDonVi/{month}/{year}")]
        [AuthorizeFilter]
        public async Task<IActionResult> NhanVienTheoDonVi(int month, int year)
        {
            try
            {
                _logger.LogInformation("Call NhanVienTheoDonVi");
                var result = await _service.Speed_DataOkla.NhanVienTheoDonViAsync(month, year);
                return ResponseMessage.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("NhanVienTheoDonVi : {0}", ex.Message));
                return ResponseMessage.Error(ex.Message);
            }
        }
    }
}
