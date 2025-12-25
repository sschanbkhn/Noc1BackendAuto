using Network.API.ViewModel.Dashboard;
using Network.API.ViewModel.Speed_DataOkla;
using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Speed_DataOkla
{
    public interface IService: IRepositoryBase<Model.Speed_DataOkla>
    {
        Task<bool> BulkInsertAsync(List<Model.Speed_DataOkla> data);
        Task<List<Rp_ExportMauTestCBNV>> ExportMauTestCBNV(Rq_ExportMauTestCBNV model);
        Task<List<RpTraCuuDuLieu>> TraCuuDuLieu(RqTraCuuDuLieu model);

        Task<Rp_ThongKeSoLuongNguoiTest> ThongKeSoLuongNguoiTestAsync(int month, int year, string donvi);
        Task<Rp_ThongKeSoLuongTestDatNguong> ThongKeSoLuongTestDatNguongAsync(int month, int year, string donvi);
        Task<Rp_Top10NguoiTestDownloadTrungVi> Top10NguoiTestDownloadTrungViAsync(int month, int year, string donvi);
        Task<Rp_Top10NguoiTestNangNo> Top10NguoiTestNangNoAsync(int month, int year, string donvi);
        Task<Rp_NhanVienTheoDonVi> NhanVienTheoDonViAsync(int month, int year);

        Task<List<object>> ListKhuVucAsync();
        Task<RpAutomationChartDownload> AutomationChartDownload(RqAutomationChartDownload model);
        Task<RpAutomationChartUpload> AutomationChartUpload(RqAutomationChartUpload model);
    }
}
