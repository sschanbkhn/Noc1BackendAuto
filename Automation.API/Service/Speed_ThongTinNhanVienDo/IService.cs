using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using Network.Core.Interfaces;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.Core.Models;

namespace Network.API.Service.Speed_ThongTinNhanVienDo
{
    public interface IService: IRepositoryBase<Model.Speed_ThongTinNhanVienDo>
    {
        Task<bool> IsExistsThongTinNhanVienDoCreate(string ho_va_ten, string don_vi, string so_dien_thoai);
        Task<bool> IsExistsThongTinNhanVienDoUpdate(string ho_va_ten, string don_vi, string so_dien_thoai);
        Task<List<RpListDoKiemTungNhanVien>> ListDoKiemTungNhanVien(RqListDoKiemTungNhanVien model);
        Task<bool> SyncDeviceAsync();
        Task<RpThongKeTheoNhanVien> ThongKeTheoNhanVien(Guid id, int month, int year);
        Task<List<RpThongKeTheoNhanVienChuaDat>> ThongKeTheoNhanVienChuaDat(int month, int year, string donvi);
        Task<List<RpThongKeTheoNhanVienChuaDat>> ThongKeTheoNhanVienChuaDatTuNgayDenNgay(string tuNgay, string deNgay, string donvi);
        
        Task<List<object>> ListDonViAsync();

        Task<Paged<Model.Speed_ThongTinNhanVienDo>> GetPagedAsync(int page, int pageSize, int totalLimitItems, string search, string donvi);
    }
}
