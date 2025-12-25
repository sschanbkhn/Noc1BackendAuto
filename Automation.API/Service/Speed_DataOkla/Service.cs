using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Network.API.ViewModel.Speed_DataOkla;
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using k8s;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Data.Common;
using System.Drawing;

namespace Network.API.Service.Speed_DataOkla
{
    public class Service:RepositoryBase<Model.Speed_DataOkla>, Speed_DataOkla.IService
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
        public async Task<List<RpTraCuuDuLieu>> TraCuuDuLieu(RqTraCuuDuLieu model)
        {
            if(string.IsNullOrEmpty(model.tu_ngay) || string.IsNullOrEmpty(model.den_ngay))
            {
                throw new Exception("Yêu cầu nhập Từ Ngày và Đến Ngày !");
            }
            var modelfromDate = DateTime.Parse(model.tu_ngay);
            var fromDay = new DateTimeOffset(modelfromDate.Year, modelfromDate.Month, modelfromDate.Day, 0, 1, 0, TimeSpan.Zero);
            var modeltoDate = DateTime.Parse(model.den_ngay);
            var toDay = (new DateTimeOffset(modeltoDate.Year, modeltoDate.Month, modeltoDate.Day, 23, 59, 59, TimeSpan.Zero));

            List<RpTraCuuDuLieu> items = new List<RpTraCuuDuLieu>();
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fromDay && y.ts_result <= toDay
                        select new RpTraCuuDuLieu() { is_portal_included = y.is_portal_included, Id = x.Id, DonVi = x.don_vi, is_device_5g_capable = x.is_device_5g_capable, is_device_rooted = x.is_device_rooted, NgayDo = y.ts_result.Value, NhanVien = x.ho_va_ten, SoDT = x.so_dien_thoai, Device_Id = y.id_device, attr_device_model = x.attr_device_model, attr_isp_common_name = y.attr_isp_common_name, attr_place_region = y.attr_place_region, Val_download_kbps = y.val_download_kbps, Val_upload_kbps = y.val_upload_kbps };
            if(!string.IsNullOrEmpty(model.attr_isp_common_name))
            {
                query = query.Where(o => o.attr_isp_common_name.ToLower().Contains(model.attr_isp_common_name.ToLower()));
            }
            if (!string.IsNullOrEmpty(model.attr_place_region))
            {
             
                query = query.Where(o => o.attr_place_region.ToLower().Contains(model.attr_place_region.ToLower()));
            }
            items = await query.OrderByDescending(o => o.NgayDo).ToListAsync();
            return items;
        }
        public async Task<List<Rp_ExportMauTestCBNV>> ExportMauTestCBNV(Rq_ExportMauTestCBNV model)
        {
            if (string.IsNullOrEmpty(model.fromDate) || string.IsNullOrEmpty(model.toDate))
            {
                throw new Exception("Yêu cầu nhập Từ Ngày và Đến Ngày !");
            }
            var modelfromDate = DateTime.Parse(model.fromDate);
            var fromDay = new DateTimeOffset(modelfromDate.Year, modelfromDate.Month, modelfromDate.Day, 0, 1, 0, TimeSpan.Zero);
            var modeltoDate = DateTime.Parse(model.toDate);
            var toDay = (new DateTimeOffset(modeltoDate.Year, modeltoDate.Month, modeltoDate.Day, 23, 59, 59, TimeSpan.Zero));

            //var data = await (from x in _dbContext.Speed_ThongTinNhanVienDos
            //            join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
            //                  where y.ts_result >= fromDay && y.ts_result <= toDay
            //                  select new Rp_ExportMauTestCBNV { 
            //                ho_va_ten = x.ho_va_ten, 
            //                email = x.email, 
            //                don_vi = x.don_vi,
            //                so_dien_thoai = x.so_dien_thoai,
            //                id_result = x.id_result,
            //                id_device = y.id_device,
            //                val_download_kbps = y.val_download_kbps,
            //                val_upload_kbps = y.val_upload_kbps,
            //                attr_location_latitude = y.attr_location_latitude,
            //                attr_location_intitude = y.attr_location_longitude,
            //                ts_result = y.ts_result.Value,
            //                is_portal_included = y.is_portal_included,
            //                attr_connection_type_end_string = y.attr_connection_type_end_string,
            //                attr_connection_type_start_string = y.attr_connection_type_start_string
            //                  }).OrderByDescending(o => o.ts_result).ToListAsync();
            var data = await (
                from x in _dbContext.Speed_ThongTinNhanVienDos
                join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                where y.ts_result >= fromDay
                   && y.ts_result <= toDay
                   && x.don_vi == "NET3"
                orderby y.ts_result descending
                select new Rp_ExportMauTestCBNV
                {
                    ho_va_ten = x.ho_va_ten,
                    email = x.email,
                    don_vi = x.don_vi,
                    so_dien_thoai = x.so_dien_thoai,
                    id_result = x.id_result,
                    id_device = y.id_device,
                    val_download_kbps = y.val_download_kbps,
                    val_upload_kbps = y.val_upload_kbps,
                    attr_location_latitude = y.attr_location_latitude,
                    attr_location_intitude = y.attr_location_longitude,
                    ts_result = y.ts_result.Value,
                    is_portal_included = y.is_portal_included,
                    attr_connection_type_end_string = y.attr_connection_type_end_string,
                    attr_connection_type_start_string = y.attr_connection_type_start_string
                }
            ).ToListAsync();

            return data;
        }    
        public async Task<bool> BulkInsertAsync(List<Model.Speed_DataOkla> data)
        {
            try
            {
                if (data == null || data.Count == 0)
                {
                    return false;
                }
                await _dbContext.Speed_DataOklas.AddRangeAsync(data);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) { return false; }
        }
        public int CalculateMedian(int[] values)
        {
            if (values.Length == 0) return 0;
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
        public async Task<Rp_ThongKeSoLuongNguoiTest> ThongKeSoLuongNguoiTestAsync(int month, int year, string donvi)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            Rp_ThongKeSoLuongNguoiTest rs = new Rp_ThongKeSoLuongNguoiTest();
            rs.labels = new string[] { "Tổng số lượng nhân viên", "Số người đã test", "Số người chưa test" };
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fistMonth && y.ts_result <= lastMonth
                        select new { x.Id, x.don_vi, x.id_device };
            if(donvi != "all")
            {
                query = query.Where(o => o.don_vi == donvi);
            }    
            var data = await query.ToListAsync();
            var data_id_device = data.Select(e => e.id_device);
            var querynhanvien = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if (donvi != "all")
            {
                querynhanvien = querynhanvien.Where(o => o.don_vi == donvi);
            }
            var ids_tongsoluongnhanvien = await querynhanvien.Select(o => new { o.Id, o.id_device }).ToListAsync();
            var ids_songuoidatest = ids_tongsoluongnhanvien.Where(o => data_id_device.Contains(o.id_device)).ToList();
            int tongsoluongnhanvien = ids_tongsoluongnhanvien.Count();
            int songuoidatest = ids_songuoidatest.Count();
            rs.data = new int[] { tongsoluongnhanvien, songuoidatest, tongsoluongnhanvien - songuoidatest };
            return rs;
        }
        public async Task<Rp_ThongKeSoLuongTestDatNguong> ThongKeSoLuongTestDatNguongAsync(int month, int year, string donvi)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            Rp_ThongKeSoLuongTestDatNguong rs = new Rp_ThongKeSoLuongTestDatNguong();
            rs.labels = new string[] { "Số lượng đã đạt", "Số lượng chưa đạt", "Số lượng chưa test" };
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fistMonth && y.ts_result <= lastMonth
                        select new { x.Id, x.don_vi, x.id_device, y.val_download_kbps, y.val_upload_kbps };
            if (donvi != "all")
            {
                query = query.Where(o => o.don_vi == donvi);
            }
            var data = await query.ToListAsync();
            var querynhanvien = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if (donvi != "all")
            {
                querynhanvien = querynhanvien.Where(o => o.don_vi == donvi);
            }
            var dsnhanvien = await querynhanvien.Select(o => new { o.Id , o.id_device}).ToListAsync();

            //
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
            //
            int soluongdadat = 0, soluongchuadat = 0, soluongchuatest = 0;
            foreach (var item in dsnhanvien)
            {
                var download_kbps = data.Where(o => o.id_device == item.id_device).Select(o => o.val_download_kbps).ToList();
                var upload_kbps = data.Where(o => o.id_device == item.id_device).Select(o => o.val_upload_kbps).ToList();
                if(download_kbps.Count() == 0 && upload_kbps.Count() == 0)
                {
                    soluongchuatest++;
                    continue;
                }    
                upload_kbps.Sort();
                download_kbps.Sort();
                var CalculateMedianDownload = CalculateMedian(download_kbps.ToArray());
                var CalculateMedianUpload = CalculateMedian(upload_kbps.ToArray());
                if (CalculateMedianDownload < trung_vi_chua_dat_down_kbps || CalculateMedianUpload < trung_vi_chua_dat_up_kbps)
                {
                    soluongchuadat++;
                }
                else if (CalculateMedianDownload >= trung_vi_chua_dat_down_kbps && CalculateMedianUpload >= trung_vi_chua_dat_up_kbps) 
                {
                    soluongdadat++;
                }     
            }
            rs.data = new int[] { soluongdadat, soluongchuadat, soluongchuatest };
            return rs;
        }
        public async Task<Rp_Top10NguoiTestDownloadTrungVi> Top10NguoiTestDownloadTrungViAsync(int month, int year, string donvi)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            Rp_Top10NguoiTestDownloadTrungVi rs = new Rp_Top10NguoiTestDownloadTrungVi();
            var querynhanvien = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if (donvi != "all")
            {
                querynhanvien = querynhanvien.Where(o => o.don_vi == donvi);
            }
            var dsnhanvien = await querynhanvien.Select(o => new { o.Id, o.ho_va_ten, o.id_device }).ToListAsync();
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fistMonth && y.ts_result <= lastMonth
                        select new { x.Id, x.don_vi, x.id_device, y.val_download_kbps };
            if (donvi != "all")
            {
                query = query.Where(o => o.don_vi == donvi);
            }
            var data = await query.ToListAsync();
            List<NhanVienSoLuongTest> items = new List<NhanVienSoLuongTest>();
            NhanVienSoLuongTest nhanVienSoLuongTest = null;
            foreach (var nhanvien in dsnhanvien)
            {
                nhanVienSoLuongTest = new NhanVienSoLuongTest();
                nhanVienSoLuongTest.ho_va_ten = nhanvien.ho_va_ten;
                var download_kbps = data.Where(o => o.id_device == nhanvien.id_device).Select(o => o.val_download_kbps).ToList();
                download_kbps.Sort();
                nhanVienSoLuongTest.trungvi_download_kbps = CalculateMedian(download_kbps.ToArray());
                items.Add(nhanVienSoLuongTest);
            }
            var top10 = items.OrderByDescending(o => o.trungvi_download_kbps).Take(10).ToList();
            rs.labels = top10.Select(o => o.ho_va_ten).ToList();
            rs.data = top10.Select(o => o.trungvi_download_kbps).ToList();
            return rs;
        }
        public async Task<Rp_Top10NguoiTestNangNo> Top10NguoiTestNangNoAsync(int month, int year, string donvi)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            Rp_Top10NguoiTestNangNo rs = new Rp_Top10NguoiTestNangNo();
            var querynhanvien = _dbContext.Speed_ThongTinNhanVienDos.AsNoTracking();
            if (donvi != "all")
            {
                querynhanvien = querynhanvien.Where(o => o.don_vi == donvi);
            }
            var dsnhanvien = await querynhanvien.Select(o => new { o.Id, o.ho_va_ten, o.id_device }).ToListAsync();
            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fistMonth && y.ts_result <= lastMonth
                        select new { x.Id, x.don_vi, x.id_device };
            if (donvi != "all")
            {
                query = query.Where(o => o.don_vi == donvi);
            }
            var data = await query.ToListAsync();
            List<NhanVienSoLuongTest> items = new List<NhanVienSoLuongTest>();
            NhanVienSoLuongTest nhanVienSoLuongTest = null;
            foreach (var nhanvien in dsnhanvien)
            {
                nhanVienSoLuongTest = new NhanVienSoLuongTest();
                nhanVienSoLuongTest.ho_va_ten = nhanvien.ho_va_ten;
                nhanVienSoLuongTest.testofnum = data.Where(o => o.id_device == nhanvien.id_device).Count();
                items.Add(nhanVienSoLuongTest);
            }
            var top10 = items.OrderByDescending(o => o.testofnum).Take(10).ToList();
            rs.labels = top10.Select(o => o.ho_va_ten).ToList();
            rs.data = top10.Select(o => o.testofnum).ToList();
            return rs;
        }
        public async Task<Rp_NhanVienTheoDonVi> NhanVienTheoDonViAsync(int month, int year)
        {
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            Rp_NhanVienTheoDonVi rs = new Rp_NhanVienTheoDonVi();

            var dsnhanvien = await _dbContext.Speed_ThongTinNhanVienDos.Select(o => new { o.Id, o.don_vi, o.id_device }).ToListAsync();

            var dsdonvi = await _dbContext.Speed_ThongTinNhanVienDos.Select(o => o.don_vi).Distinct().ToListAsync();

            var query = from x in _dbContext.Speed_ThongTinNhanVienDos
                        join y in _dbContext.Speed_DataOklas on x.id_device equals y.id_device
                        where y.ts_result >= fistMonth && y.ts_result <= lastMonth && y.attr_isp_name_raw == "MobiFone"
                        select new { x.Id, x.don_vi, x.id_device, y.val_download_kbps, y.val_upload_kbps };
            var data = await query.ToListAsync();

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
            rs.data1 = new List<string>();
            rs.data2 = new List<string>();
            foreach (var dv in dsdonvi)
            {
                var data_dv = data.Where(o => o.don_vi == dv).ToList();
                var nhanvien_dv = dsnhanvien.Where(o => o.don_vi == dv).ToList();
                int[] ds_dadat_chuadat = new int[2] { 0, 0 };
                for(var i = 0;i < nhanvien_dv.Count;i++)
                {
                    var download_kbps = data.Where(o => o.id_device == nhanvien_dv[i].id_device).Select(o => o.val_download_kbps).ToList();
                    var upload_kbps = data.Where(o => o.id_device == nhanvien_dv[i].id_device).Select(o => o.val_upload_kbps).ToList();
                    upload_kbps.Sort();
                    download_kbps.Sort();
                    var CalculateMedianDownload = CalculateMedian(download_kbps.ToArray());
                    var CalculateMedianUpload = CalculateMedian(upload_kbps.ToArray());
                    if(CalculateMedianDownload > trung_vi_chua_dat_down_kbps && CalculateMedianUpload > trung_vi_chua_dat_up_kbps)
                    {
                        ds_dadat_chuadat[0]++;
                    }    
                    else
                    {
                        ds_dadat_chuadat[1]++;
                    }    
                }
                rs.data1.Add(ds_dadat_chuadat[0].ToString());
                rs.data2.Add(ds_dadat_chuadat[1].ToString());
            }    
            rs.labels = dsdonvi;
            return rs;
        }
        public List<string> GetListDayOfMonth (int month, int year)
        {
            List<string> rs = new List<string>();
            var fistMonth = new DateTime(year, month, 1, 0, 1, 0);
            var lastMonth = (new DateTime(year, month, 1, 23, 59, 0)).AddMonths(1).AddDays(-1);

            for (DateTime date = fistMonth; date <= lastMonth; date = date.AddDays(1))
            {
                rs.Add("N" + date.Day.ToString());
            }
            return rs;
        }

        public async Task<RpAutomationChartDownload> AutomationChartDownload(RqAutomationChartDownload model)
        {
            var res = new RpAutomationChartDownload();

            List<List<double>> datas = new List<List<double>>();
            List<double> data = null;
            List<string> labels = new List<string>();
            List<AutomationChartView> lView = new List<AutomationChartView>();
            AutomationChartView view = null;

            //
            string queryBuilderKhuVuc = "";
            if (model.listKhuVuc != null && model.listKhuVuc.Count > 0)
            {
                if (!model.listKhuVuc.Contains("all"))
                {
                    queryBuilderKhuVuc += " and attr_place_region in ('" + string.Join("','", model.listKhuVuc.ToArray()) + "') ";
                }
            }
            //
            if (model.type == "TheoThang")
            {
                labels.AddRange(GetListDayOfMonth(model.month, model.year));
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_day, x.sp_month, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_download_kbps) AS median_speed from (select id_device, attr_sim_operator_common_name, extract (day from (ts_result::timestamp - interval '7 hours')) as sp_day, extract (month from (ts_result::timestamp - interval '7 hours')) as sp_month, avg(val_download_kbps) as avg_val_download_kbps from public.\"Speed_DataOklas_Y" + model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' " + queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "') and extract (month from (ts_result::timestamp - interval '7 hours')) = " + model.month + " group by id_device, attr_sim_operator_common_name, extract (day from (ts_result::timestamp - interval '7 hours')), extract (month from (ts_result::timestamp - interval '7 hours'))) as x group by  x.attr_sim_operator_common_name, x.sp_day, x.sp_month";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_day = Convert.ToInt32(dbDataReader.GetValue(1));
                            view.median_speed = dbDataReader.GetDouble(3);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < labels.Count; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && (i + 1) == item.sp_day)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            else if (model.type == "TheoQuy")
            {
                int[] listThangOfQuy = null;
                if (model.quarter == 1)
                {
                    labels.AddRange(new string[] { "T1", "T2", "T3" });
                    listThangOfQuy = new int[] { 1, 2, 3 };
                }
                else if (model.quarter == 2)
                {
                    labels.AddRange(new string[] { "T4", "T5", "T6" });
                    listThangOfQuy = new int[] { 4, 5, 6 };
                }
                else if (model.quarter == 3)
                {
                    labels.AddRange(new string[] { "T7", "T8", "T9" });
                    listThangOfQuy = new int[] { 7, 8, 9 };
                }
                else if (model.quarter == 4)
                {
                    labels.AddRange(new string[] { "T10", "T11", "T12" });
                    listThangOfQuy = new int[] { 10, 11, 12 };
                }
                else
                {
                    labels.AddRange(new string[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" });
                    listThangOfQuy = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                }
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_month, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_download_kbps) AS median_speed from (select id_device, attr_sim_operator_common_name, extract (month from (ts_result::timestamp - interval '7 hours')) as sp_month, avg(val_download_kbps) as avg_val_download_kbps from public.\"Speed_DataOklas_Y" + model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' " + queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "') and extract (month from (ts_result::timestamp - interval '7 hours')) in (" + string.Join(", ", listThangOfQuy) + ") group by id_device, attr_sim_operator_common_name, extract (month from (ts_result::timestamp - interval '7 hours'))) as x group by  x.attr_sim_operator_common_name, x.sp_month";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_month = Convert.ToInt32(dbDataReader.GetValue(1));
                            view.median_speed = dbDataReader.GetDouble(2);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < listThangOfQuy.Length; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && listThangOfQuy[i] == item.sp_month)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            else if (model.type == "TheoNam")
            {
                labels.AddRange(new string[] { "Q1", "Q2", "Q3", "Q4" });
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_quarter, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_download_kbps) AS median_speed from ( select id_device, attr_sim_operator_common_name, sp_quarter, avg(val_download_kbps) as avg_val_download_kbps from ( select id_device, attr_sim_operator_common_name, CASE WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 1 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 2 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 3) THEN 'Q1' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 4 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 5 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 6) THEN 'Q2' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 7 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 8 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 9) THEN 'Q3' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 10 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 11 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 12) THEN 'Q4' END AS sp_quarter , val_download_kbps from public.\"Speed_DataOklas_Y"+ model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' " + queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "')) as x group by id_device, attr_sim_operator_common_name, sp_quarter ) as x group by  x.attr_sim_operator_common_name, x.sp_quarter";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_quarter = dbDataReader.GetString(1);
                            view.median_speed = dbDataReader.GetDouble(2);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < labels.Count; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && labels[i] == item.sp_quarter)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            res.labels = model.listNhaMang;
            res.datas_labels = labels;
            res.datas = datas;
            return res;
        }

        public async Task<RpAutomationChartUpload> AutomationChartUpload(RqAutomationChartUpload model)
        {
            var res = new RpAutomationChartUpload();

            List<List<double>> datas = new List<List<double>>();
            List<double> data = null;
            List<string> labels = new List<string>();
            List<AutomationChartView> lView = new List<AutomationChartView>();
            AutomationChartView view = null;
            //
            string queryBuilderKhuVuc = "";
            if (model.listKhuVuc != null && model.listKhuVuc.Count > 0)
            {
                if (!model.listKhuVuc.Contains("all"))
                {
                    queryBuilderKhuVuc += " and attr_place_region in ('" + string.Join("','", model.listKhuVuc.ToArray()) + "') ";
                }
            }
            //
            if (model.type == "TheoThang")
            {
                labels.AddRange(GetListDayOfMonth(model.month, model.year));
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_day, x.sp_month, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_upload_kbps) AS median_speed from (select id_device, attr_sim_operator_common_name, extract (day from (ts_result::timestamp - interval '7 hours')) as sp_day, extract (month from (ts_result::timestamp - interval '7 hours')) as sp_month, avg(val_upload_kbps) as avg_val_upload_kbps from public.\"Speed_DataOklas_Y" + model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' " + queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "') and extract (month from (ts_result::timestamp - interval '7 hours')) = " + model.month + " group by id_device, attr_sim_operator_common_name, extract (day from (ts_result::timestamp - interval '7 hours')), extract (month from (ts_result::timestamp - interval '7 hours'))) as x group by  x.attr_sim_operator_common_name, x.sp_day, x.sp_month";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_day = Convert.ToInt32(dbDataReader.GetValue(1));
                            view.median_speed = dbDataReader.GetDouble(3);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < labels.Count; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && (i + 1) == item.sp_day)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            else if (model.type == "TheoQuy")
            {
                int[] listThangOfQuy = null;
                if (model.quarter == 1)
                {
                    labels.AddRange(new string[] { "T1", "T2", "T3" });
                    listThangOfQuy = new int[] { 1, 2, 3 };
                }
                else if (model.quarter == 2)
                {
                    labels.AddRange(new string[] { "T4", "T5", "T6" });
                    listThangOfQuy = new int[] { 4, 5, 6 };
                }
                else if (model.quarter == 3)
                {
                    labels.AddRange(new string[] { "T7", "T8", "T9" });
                    listThangOfQuy = new int[] { 7, 8, 9 };
                }
                else if (model.quarter == 4)
                {
                    labels.AddRange(new string[] { "T10", "T11", "T12" });
                    listThangOfQuy = new int[] { 10, 11, 12 };
                }
                else
                {
                    labels.AddRange(new string[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" });
                    listThangOfQuy = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                }    
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_month, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_upload_kbps) AS median_speed from (select id_device, attr_sim_operator_common_name, extract (month from (ts_result::timestamp - interval '7 hours')) as sp_month, avg(val_upload_kbps) as avg_val_upload_kbps from public.\"Speed_DataOklas_Y" + model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' " + queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "') and extract (month from (ts_result::timestamp - interval '7 hours')) in (" + string.Join(", ", listThangOfQuy) + ") group by id_device, attr_sim_operator_common_name, extract (month from (ts_result::timestamp - interval '7 hours'))) as x group by  x.attr_sim_operator_common_name, x.sp_month";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_month = Convert.ToInt32(dbDataReader.GetValue(1));
                            view.median_speed = dbDataReader.GetDouble(2);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < listThangOfQuy.Length; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && listThangOfQuy[i] == item.sp_month)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            else if (model.type == "TheoNam")
            {
                labels.AddRange(new string[] { "Q1", "Q2", "Q3", "Q4" });
                var sqlQuery = "select x.attr_sim_operator_common_name, x.sp_quarter, percentile_cont(0.5) WITHIN GROUP (ORDER BY x.avg_val_upload_kbps) AS median_speed from ( select id_device, attr_sim_operator_common_name, sp_quarter, avg(val_upload_kbps) as avg_val_upload_kbps from ( select id_device, attr_sim_operator_common_name, CASE WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 1 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 2 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 3) THEN 'Q1' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 4 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 5 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 6) THEN 'Q2' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 7 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 8 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 9) THEN 'Q3' WHEN (CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 10 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 11 or CAST(extract (month from (ts_result::timestamp - interval '7 hours')) AS integer) = 12) THEN 'Q4' END AS sp_quarter , val_upload_kbps from public.\"Speed_DataOklas_Y"+ model.year + "\" sdo where is_portal_included = 'true' and attr_place_country_code = 'VN' "+ queryBuilderKhuVuc + " and attr_sim_operator_common_name in ('" + string.Join("', '", model.listNhaMang) + "')) as x group by id_device, attr_sim_operator_common_name, sp_quarter ) as x group by  x.attr_sim_operator_common_name, x.sp_quarter";
                DbDataReader dbDataReader = null;
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    dbDataReader = await command.ExecuteReaderAsync();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            view = new AutomationChartView();
                            view.attr_sim_operator_common_name = dbDataReader.GetString(0);
                            view.sp_quarter = dbDataReader.GetString(1);
                            view.median_speed = dbDataReader.GetDouble(2);
                            lView.Add(view);
                        }
                    }
                    dbDataReader.Close();
                }
                foreach (var nhaMang in model.listNhaMang)
                {
                    data = new List<double>();
                    for (var i = 0; i < labels.Count; i++)
                    {
                        bool flag = false;
                        foreach (var item in lView)
                        {
                            if (nhaMang == item.attr_sim_operator_common_name && labels[i] == item.sp_quarter)
                            {
                                flag = true;
                                data.Add(Math.Round(item.median_speed / 1000, 2));
                                break;
                            }
                        }
                        if (!flag)
                        {
                            data.Add(0);
                        }
                    }
                    datas.Add(data);
                }
            }
            res.labels = model.listNhaMang;
            res.datas_labels = labels;
            res.datas = datas;
            return res;
        }

        private static double CalculateAverage(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0) return 0;
            // Tính tổng các số trong dãy
            int sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }

            // Tính trung bình
            double average = (double)sum / numbers.Count;
            return average;
        }

        public async Task<List<object>> ListKhuVucAsync()
        {
            var list_id_device = await _dbContext.Speed_ThongTinNhanVienDos.Where(o => o.id_device != "").Select(o => o.id_device).ToListAsync();
            var items = await _dbContext.Speed_DataOklas.Where(o => o.attr_place_region != "" && list_id_device.Contains(o.id_device)).Select(o => o.attr_place_region).Distinct().ToListAsync();
            List<object> result = new List<object>();
            result.Add(new { label = "Tất cả khu vực", value = "all" });
            foreach (var item in items)
            {
                result.Add(new { label = item, value = item });
            }
            return result;
        }
    }
}
