using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Infrastructure
{
    public class Enums
    {
        public enum PhieuBienNhanConfig
        {
            MauPhieuBienNhan,
        }
        public enum DonViVanChuyenConfig
        {
            EmailGui,
            MauEmail,
            SmsGui,
            MauSms
        }
        public enum EmailConfig
        {
            Mau_Email_HSDVC_ThanhToan,
            Mau_Email_HSDVC_TiepNhan,
            Mau_Email_HSDVC_TraNguocLai,
            Mau_Email_HSDVC_Huy,
            Mau_Email_HSDVC_TraKetQua,
            Mau_Email_HSDVC_NopHoSo,
            Mau_Email_HSDVC_ToiBuocCuoi,
            Mau_Email_HSDVC_GuiHoSo,
            Mau_Email_HSDVC_TamDung,
            Mau_Email_HSDVC_TiepTuc,
            //
            Mau_Email_HSPA_TraNguocLai,
            Mau_Email_HSPA_TiepNhan,
            Mau_Email_HSPA_Huy,
            Mau_Email_HSPA_TraKetQua,
            Mau_Email_HSPA_NopHoSo,
            Mau_Email_HSPA_KetThuc,
            Mau_Email_HSPA_ToiBuocCuoi
        }
        public enum SmsConfig
        {
            Mau_Sms_HSDVC_ThanhToan,
            Mau_Sms_HSDVC_TiepNhan,
            Mau_Sms_HSDVC_TraNguocLai,
            Mau_Sms_HSDVC_Huy,
            Mau_Sms_HSDVC_TraKetQua,
            Mau_Sms_HSDVC_NopHoSo,
            Mau_Sms_HSDVC_ToiBuocCuoi,
            Mau_Sms_HSDVC_GuiHoSo,
            Mau_Sms_HSDVC_TamDung,
            Mau_Sms_HSDVC_TiepTuc,
            //
            Mau_Sms_HSPA_TraNguocLai,
            Mau_Sms_HSPA_TiepNhan,
            Mau_Sms_HSPA_Huy,
            Mau_Sms_HSPA_TraKetQua,
            Mau_Sms_HSPA_NopHoSo,
            Mau_Sms_HSPA_ToiBuocCuoi
        }

        public enum LoaiHinhThucThanhToan
        {
            ViDienTu,
            TheThanhToanQuocTe,
            TheNganHangNoiDia,
            ThanhToanTrucTiep,
            MienPhi,
        }
        public enum LoaiFileHoSoNguoiNop
        {
            AnhGiayChungNhan,
            AnhBanDo
        }    
        public enum LoaiLinhVuc_GopYPhanAnh
        {
            QuyHoachDatDai,
            QuyHoachXayDung,
            QuyHoachDoThi,
            ThuTucHanhChinh,
            Khac,
        }
        public enum TrangThai_GopYPhanAnh
        {
            ChoXuLy,
            Daxuly,
            Huy,
        }
        public enum TrangThaiQuyTrinh
        {
            huy,            
            choxuly,
            traketqua,
            dangxuly,
            daxuly,
            tamdung,
            tranguoclai,
            dangvanchuyen
        }
        public enum FileType
        {
            Feedback
        }
        public enum NotiType
        {
            RegisteredUser
        }
        public enum SecurityPolicy
        {

        }
        
    }
}
