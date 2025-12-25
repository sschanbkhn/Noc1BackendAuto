using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Drawing;
using Network.Core.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Network.API.Service.Speed_ThongTinNhanVienDo
{
    public class Service:RepositoryBase<Model.Speed_ThongTinNhanVienDo>, Speed_ThongTinNhanVienDo.IService
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
        public async Task<List<object>> ListDonViAsync()
        {
            var items = await _dbContext.Speed_ThongTinNhanVienDos.Select(o => o.don_vi).Distinct().ToListAsync();
            List<object> result = new List<object>();
            result.Add(new { label = "Tất cả đơn vị", value = "all" });
            foreach (var item in items)
            {
                result.Add(new { label = item, value = item });
            }    
            return result;
        }
        public async Task<bool> IsExistsThongTinNhanVienDoCreate(string ho_va_ten, string don_vi, string so_dien_thoai)
        {
            return await _dbContext.Speed_ThongTinNhanVienDos.AnyAsync(o => o.ho_va_ten == ho_va_ten && o.don_vi == don_vi && o.so_dien_thoai == so_dien_thoai);
        }
        public async Task<bool> IsExistsThongTinNhanVienDoUpdate(string ho_va_ten, string don_vi, string so_dien_thoai)
        {
            var count = await _dbContext.Speed_ThongTinNhanVienDos.CountAsync(o => o.ho_va_ten == ho_va_ten && o.don_vi == don_vi && o.so_dien_thoai == so_dien_thoai);
            if(count <= 1)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> SyncDeviceAsync()
        {
            var dsnv_not_device = await _dbContext.Speed_ThongTinNhanVienDos.Where(o => string.IsNullOrEmpty(o.id_device)).ToListAsync();
            var dsnv_ids_result = dsnv_not_device.Select(o => o.id_result);

            var data = await _dbContext.Speed_DataOklas.Select(o => new { o.id_result, o.id_device, o.attr_device_model, o.is_device_5g_capable, o.is_device_rooted }).Where(o => dsnv_ids_result.Contains(o.id_result)).ToListAsync();
            foreach(var nv in dsnv_not_device)
            {
                var datanv = data.FirstOrDefault(o => o.id_result ==  nv.id_result);
                if(datanv != null)
                {
                    nv.id_device = datanv.id_device;
                    nv.attr_device_model = datanv.attr_device_model;
                    nv.is_device_5g_capable = datanv.is_device_5g_capable;
                    nv.is_device_rooted = datanv.is_device_rooted;
                }    
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public int CalculateMedian(int[] values)
        {
            if(values.Length == 0) return 0;
            int n = values.Length;
            if (n % 2 == 0)
            {
                int middleIndex1 = n / 2 - 1;
                int middleIndex2 = n / 2;
                return (values[middleIndex1] + values[middleIndex2]) / 2;
            }
            else
            {
                int middleIndex = n / 2;
                return values[middleIndex];
            }
        }
        public async Task<RpThongKeTheoNhanVien> ThongKeTheoNhanVien(Guid id, int month, int year)
        {
            RpThongKeTheoNhanVien rs = new RpThongKeTheoNhanVien();
            var current = DateTime.Now;

            year = year == 0 ? current.Year : year;
            month = month == 0 ? current.Month : month;

            var now = new DateTime(year, month, 1);
            var fistYear = new DateTime(now.Year, 1, 1, 0, 1, 0);
            var lastYear = (new DateTime(now.Year, 1, 1, 23, 59, 0)).AddYears(1).AddDays(-1);

            var nvd = await _dbContext.Speed_ThongTinNhanVienDos.Where(o => o.Id == id).FirstOrDefaultAsync();
            if (nvd == null)
            {
                throw new Exception("Nhân viên đo không tồn tại");
            }
            var datas = await _dbContext.Speed_DataOklas.Select(o => new { o.id_device, o.attr_isp_name_raw, o.ts_result, o.val_download_kbps, o.val_upload_kbps })
                .Where(o => o.attr_isp_name_raw == "MobiFone" && o.id_device == nvd.id_device && o.ts_result >= fistYear && o.ts_result <= lastYear).ToListAsync();
            
            //Tháng
            var items_download_kbps = datas.Where(o => o.ts_result.Value.Month == now.Month).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            var items_upload_kbps = datas.Where(o => o.ts_result.Value.Month == now.Month).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Thang = now.Month;
            rs.Thang_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Thang_Upload = CalculateMedian(items_upload_kbps.ToArray());
            //Năm
            items_download_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            items_upload_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Nam = now.Year;
            rs.Nam_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Nam_Upload = CalculateMedian(items_upload_kbps.ToArray());
            //Quý 1
            items_download_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 1 || o.ts_result.Value.Month == 2 || o.ts_result.Value.Month == 3)).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            items_upload_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 1 || o.ts_result.Value.Month == 2 || o.ts_result.Value.Month == 3)).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Q1 = 1;
            rs.Q1_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Q1_Upload = CalculateMedian(items_upload_kbps.ToArray());
            //Quý 2
            items_download_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 4 || o.ts_result.Value.Month == 5 || o.ts_result.Value.Month == 6)).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            items_upload_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 4 || o.ts_result.Value.Month == 5 || o.ts_result.Value.Month == 6)).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Q2 = 2;
            rs.Q2_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Q2_Upload = CalculateMedian(items_upload_kbps.ToArray());
            //Quý 3
            items_download_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 7 || o.ts_result.Value.Month == 8 || o.ts_result.Value.Month == 9)).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            items_upload_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 7 || o.ts_result.Value.Month == 8 || o.ts_result.Value.Month == 9)).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Q3 = 3;
            rs.Q3_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Q3_Upload = CalculateMedian(items_upload_kbps.ToArray());
            //Quý 4
            items_download_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 10 || o.ts_result.Value.Month == 11 || o.ts_result.Value.Month == 12)).Select(o => o.val_download_kbps).ToList();
            items_download_kbps.Sort();
            items_upload_kbps = datas.Where(o => o.ts_result.Value.Year == now.Year && (o.ts_result.Value.Month == 10 || o.ts_result.Value.Month == 11 || o.ts_result.Value.Month == 12)).Select(o => o.val_upload_kbps).ToList();
            items_upload_kbps.Sort();
            rs.Q4 = 4;
            rs.Q4_Download = CalculateMedian(items_download_kbps.ToArray());
            rs.Q4_Upload = CalculateMedian(items_upload_kbps.ToArray());
            return rs;
        }
        public async Task<List<RpThongKeTheoNhanVienChuaDat>> ThongKeTheoNhanVienChuaDat(int month, int year, string donvi)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            List<RpThongKeTheoNhanVienChuaDat> items = new List<RpThongKeTheoNhanVienChuaDat>();
            var query_nvd = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if (donvi != "all")
            {
                query_nvd = query_nvd.Where(o => o.don_vi == donvi);
            }
            var list_nvd = await query_nvd.Select(o => new { o.Id, o.id_device, o.attr_device_model, o.is_device_5g_capable, o.is_device_rooted, o.ho_va_ten, o.don_vi, o.so_dien_thoai, o.email }).OrderBy(o => o.ho_va_ten).ToListAsync();

            var listIdDevice = list_nvd.Select(o => o.id_device).ToList();

            var datas = await _dbContext.Speed_DataOklas
                .Select(o => new { o.attr_isp_name_raw, o.id_device, o.ts_result, o.val_download_kbps, o.val_upload_kbps })
                .Where(o => o.attr_isp_name_raw == "MobiFone" && listIdDevice.Contains(o.id_device) && o.ts_result.HasValue && o.ts_result >= fistMonth && o.ts_result <= lastMonth).ToListAsync();

            var items_download_kbps = new List<int>();
            var items_upload_kbps = new List<int>();
            var trung_vi_chua_dat_down_kbps = 50;
            var trung_vi_chua_dat_up_kbps = 50;
            var down_mbps = await _dbContext.Sys_Configs.FirstOrDefaultAsync(o => o.Code == "down");
            var up_mbps = await _dbContext.Sys_Configs.FirstOrDefaultAsync(o => o.Code == "up");
            if (down_mbps != null)
            {
                trung_vi_chua_dat_down_kbps = int.Parse(down_mbps.Value) * 1000;
            }
            if (up_mbps != null)
            {
                trung_vi_chua_dat_up_kbps = int.Parse(up_mbps.Value) * 1000;
            }
            foreach (var item in list_nvd)
            {
                RpThongKeTheoNhanVienChuaDat itemrs = new RpThongKeTheoNhanVienChuaDat();
                items_download_kbps = datas.Where(o => o.id_device == item.id_device).Select(o => o.val_download_kbps).ToList();
                items_download_kbps.Sort();
                items_upload_kbps = datas.Where(o => o.id_device == item.id_device).Select(o => o.val_upload_kbps).ToList();
                items_upload_kbps.Sort();
                itemrs.NhanVien = item.ho_va_ten;
                itemrs.SoDT = item.so_dien_thoai;
                itemrs.Email = item.email;
                itemrs.donvi = item.don_vi;
                itemrs.attr_device_model = item.attr_device_model;
                itemrs.is_device_5g_capable = item.is_device_5g_capable;
                itemrs.is_device_rooted = item.is_device_rooted;
                itemrs.Val_download_kbps = CalculateMedian(items_download_kbps.ToArray());
                itemrs.Val_upload_kbps = CalculateMedian(items_upload_kbps.ToArray());
                if (itemrs.Val_download_kbps < trung_vi_chua_dat_down_kbps || itemrs.Val_upload_kbps < trung_vi_chua_dat_up_kbps)
                {
                    itemrs.trangthai = "Chưa đạt";
                    items.Add(itemrs);
                }
            }

            return items;
        }
        public async Task<List<RpThongKeTheoNhanVienChuaDat>> ThongKeTheoNhanVienChuaDatTuNgayDenNgay(string tuNgay, string denNgay, string donvi)
        {
            DateTimeOffset fromDay = DateTimeOffset.Now;
            DateTimeOffset toDay = DateTimeOffset.Now;
            if (!string.IsNullOrEmpty(tuNgay))
            {
                var modelTuNgay = DateTime.Parse(tuNgay);
                fromDay = new DateTimeOffset(modelTuNgay.Year, modelTuNgay.Month, modelTuNgay.Day, 0, 1, 0, TimeSpan.Zero);
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                var modelDenNgay = DateTime.Parse(denNgay);
                toDay = (new DateTimeOffset(modelDenNgay.Year, modelDenNgay.Month, modelDenNgay.Day, 23, 59, 59, TimeSpan.Zero));
            }

            List<RpThongKeTheoNhanVienChuaDat> items = new List<RpThongKeTheoNhanVienChuaDat>();
            var query_nvd = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if(donvi != "all")
            {
                query_nvd = query_nvd.Where(o => o.don_vi == donvi);
            }    
            var list_nvd = await query_nvd.Select(o => new { o.Id, o.id_device, o.attr_device_model, o.is_device_5g_capable, o.is_device_rooted, o.ho_va_ten, o.don_vi, o.so_dien_thoai, o.email}).OrderBy(o => o.ho_va_ten).ToListAsync();
            
            var listIdDevice = list_nvd.Select(o => o.id_device).ToList();

            var datas = await _dbContext.Speed_DataOklas
                .Select(o => new { o.attr_isp_name_raw, o.id_device, o.ts_result, o.val_download_kbps, o.val_upload_kbps })
                .Where(o => o.attr_isp_name_raw == "MobiFone" && listIdDevice.Contains(o.id_device) && o.ts_result.HasValue && o.ts_result >= fromDay && o.ts_result <= toDay).ToListAsync();
            
            var items_download_kbps = new List<int>();
            var items_upload_kbps = new List<int>();
            var trung_vi_chua_dat_down_kbps = 50;
            var trung_vi_chua_dat_up_kbps = 50;
            var down_mbps = await _dbContext.Sys_Configs.FirstOrDefaultAsync(o => o.Code == "down");
            var up_mbps = await _dbContext.Sys_Configs.FirstOrDefaultAsync(o => o.Code == "up");
            if (down_mbps != null)
            {
                trung_vi_chua_dat_down_kbps = int.Parse(down_mbps.Value) * 1000;
            }
            if (up_mbps != null)
            {
                trung_vi_chua_dat_up_kbps = int.Parse(up_mbps.Value) * 1000;
            }
            foreach (var item in list_nvd)
            {
                RpThongKeTheoNhanVienChuaDat itemrs = new RpThongKeTheoNhanVienChuaDat(); 
                items_download_kbps = datas.Where(o => o.id_device == item.id_device).Select(o => o.val_download_kbps).ToList();
                items_download_kbps.Sort();
                items_upload_kbps = datas.Where(o => o.id_device == item.id_device).Select(o => o.val_upload_kbps).ToList();
                items_upload_kbps.Sort();
                itemrs.NhanVien = item.ho_va_ten;
                itemrs.SoDT = item.so_dien_thoai;
                itemrs.Email = item.email;
                itemrs.donvi = item.don_vi;
                itemrs.attr_device_model = item.attr_device_model;
                itemrs.is_device_5g_capable = item.is_device_5g_capable;
                itemrs.is_device_rooted = item.is_device_rooted;
                itemrs.Val_download_kbps = CalculateMedian(items_download_kbps.ToArray());
                itemrs.Val_upload_kbps = CalculateMedian(items_upload_kbps.ToArray());  
                if (itemrs.Val_download_kbps < trung_vi_chua_dat_down_kbps || itemrs.Val_upload_kbps < trung_vi_chua_dat_up_kbps)
                {
                    itemrs.trangthai = "Chưa đạt";
                    items.Add(itemrs);
                }    
            }

            return items;
        }

        public async Task<List<RpListDoKiemTungNhanVien>> ListDoKiemTungNhanVien(RqListDoKiemTungNhanVien model)
        {
            List<RpListDoKiemTungNhanVien> items = new List<RpListDoKiemTungNhanVien>();
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        select new RpListDoKiemTungNhanVien { val_jitter_ms = y.val_jitter_ms, is_portal_included = y.is_portal_included, Id = x.Id, DonVi = x.don_vi, is_device_5g_capable = x.is_device_5g_capable, is_device_rooted = x.is_device_rooted, NgayDo = y.ts_result.Value, NhanVien = x.ho_va_ten, SoDT = x.so_dien_thoai, Device_Id = y.id_device, attr_device_model = x.attr_device_model, attr_isp_common_name = y.attr_isp_common_name, attr_place_region = y.attr_place_region, Val_download_kbps = y.val_download_kbps, Val_upload_kbps = y.val_upload_kbps };
            if(!string.IsNullOrEmpty(model.TenNhanVien))
            {
                query = query.Where(o => o.NhanVien.ToLower().Contains(model.TenNhanVien.ToLower()));
            }    
            if (!string.IsNullOrEmpty(model.TuNgay))
            {
                var modelTuNgay = DateTime.Parse(model.TuNgay);
                var fromDay = new DateTimeOffset(modelTuNgay.Year, modelTuNgay.Month, modelTuNgay.Day, 0, 1, 0, TimeSpan.Zero);
                query = query.Where(o => o.NgayDo >= fromDay);
            }
            if (!string.IsNullOrEmpty(model.DenNgay))
            {
                var modelDenNgay = DateTime.Parse(model.DenNgay);
                var toDay = (new DateTimeOffset(modelDenNgay.Year, modelDenNgay.Month, modelDenNgay.Day, 23, 59, 59, TimeSpan.Zero));
                query = query.Where(o => o.NgayDo <= toDay);
            }
            if (model.Down != null)
            {
                query = query.Where(o => o.Val_download_kbps <= model.Down * 1000);
            }
            if (model.Up != null)
            {
                query = query.Where(o => o.Val_upload_kbps <= model.Up * 1000);
            }
            if (model.DonVi != "all")
            {
                query = query.Where(o => o.DonVi == model.DonVi);
            }
            items = await query.OrderByDescending(o => o.NgayDo).ToListAsync();
            return items;
        }


        public async Task<Paged<Model.Speed_ThongTinNhanVienDo>> GetPagedAsync(int page, int pageSize, int totalLimitItems, string search, string donvi)
        {
            var query = _dbContext.Set<Model.Speed_ThongTinNhanVienDo>().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(search);
            }
            if (donvi != "all")
            {
                query = query.Where(o => o.don_vi == donvi);
            }
            Paged<Model.Speed_ThongTinNhanVienDo> result = new Paged<Model.Speed_ThongTinNhanVienDo>(query, page, pageSize, totalLimitItems);
            result.Items = await query.Paged(page, pageSize, totalLimitItems).OrderByDescending(o => o.don_vi).ThenBy(o => o.ho_va_ten).ToListAsync();
            return result;
        }
    }
}
