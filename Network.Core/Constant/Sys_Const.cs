using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Constant
{
    public class Sys_Const
    {
        public static string GuidAdmin = "11111111-1111-1111-1111-111111111111";
        public static int TimeZone = 0;
        public struct Security
        {
            public const string purpose = "DataProtection_v2h2b7";//using Microsoft.AspNetCore.DataProtection;
            public const string key = "v2h2b7";
        }
        public struct Message
        {
            public const string SERVICE_ERROR_ENTITY_NOT_FOUND = "Không tìm thấy đối tượng !";
            public const string SERVICE_ERROR_PARAM_INVALID = "Tham số không hợp lệ !";
            public const string SERVICE_SUCCESS = "Xử lý thành công !";
            public const string SERVICE_ERROR = "Xử lý thất bại !";
            public const string SERVICE_LOGIN_SUCCESS = "Đăng nhập thành công !";
            public const string SERVICE_SIGNUP_SUCCESS = "Đăng ký thành công !";
            public const string SERVICE_LOGIN_ERROR = "Đăng nhập thất bại !";
            public const string SERVICE_SIGNUP_ERROR = "Đăng ký thất bại !";
            public const string SERVICE_LOGIN_USERNAME_EMPTY = "Tài khoản không được để trống !";
            public const string SERVICE_LOGIN_PASSWORD_EMPTY = "Mật khẩu không được để trống !";
            public const string SERVICE_LOGIN_PASSWORDOld_EMPTY = "Mật khẩu cũ không được để trống !";
            public const string SERVICE_LOGIN_PASSWORDNEW_EMPTY = "Mật khẩu mới không được để trống !";
            public const string SERVICE_LOGIN_PASSNEW_PASSOld_DIFFERENT = "Mật khẩu cũ và Mật khẩu mới không được trùng nhau !";
            public const string SERVICE_LOGOUT_SUCCESS = "Đăng xuất thành công !";
            public const string SERVICE_EDIT_PROFILE_SUCCESS = "Đăng xuất thành công !";
            public const string SERVICE_LOGOUT_ERROR = "Đăng xuất thất bại !";
            public const string SERVICE_RESTORE_PASSWORD_SUCCESS = "Khôi phục mật khẩu thành công !";
            public const string SERVICE_RESTORE_PASSWORD_ERROR = "Khôi phục mật khẩu thất bại !";
            public const string SERVICE_REFRESH_SUCCESS = "Làm mới Token thành công !";
            public const string SERVICE_REFRESH_ERROR = "Làm mới Token thất bại !";
            public const string SERVICE_PASS_INCORRECT = "Mật khẩu không đúng !";
            public const string SERVICE_PASSOLD_INCORRECT = "Mật khẩu cũ không đúng !";                        
            public const string SERVICE_PHONE_EXISTS = "Số điện thoại đã tồn tại !";
            public const string SERVICE_EMAIL_NOT_EXISTS = "Email không tồn tại !";
            public const string SERVICE_USERNAME_UNACTIVE = "Tài khoản chưa kích hoạt !";
            public const string SERVICE_USERNAME_UNEXISTS = "Tài khoản không tồn tại !";
            public const string SERVICE_USERNAME_EXISTS = "Tên tài khoản đã tồn tại !";
            public const string SERVICE_EMAIL_EXISTS = "Email đã tồn tại !";
            public const string SERVICE_CHANGEPASSWORD_SUCCESS = "Thay đổi mật khẩu thành công !";
            public const string SERVICE_FORGETPASSWORD_SUCCESS = "Gửi mật khẩu mới tới Email thành công !";
            public const string SERVICE_CHANGEPASSWORD_ERROR = "Thay đổi mật khẩu thất bại !";
            public const string SERVICE_CODE_CHANGEPASSWORD_EXPIRE = "Đường dẫn thay đổi mật khẩu hết hạn !";
            public const string SERVICE_DATA_EXISTS_ID = "Dữ liệu {0} ({1}) đã tồn tại trong {2} ({3}) !";
            public const string SERVICE_PLEASE_REMOVE_DATA = "Vui lòng xóa {0} ({1}) khỏi {2} ({3}) !";
            public const string SERVICE_FILE_NOT_EMPTY = "File không được để trống !";
            public const string SERVICE_CODE_NOT_EMPTY = "Mã không được để trống !";
            public const string SERVICE_NAME_NOT_EMPTY = "Tên không được để trống !";
            public const string SERVICE_ROLE_UNEXISTED = "Vai trò không tồn tại !";
            public const string SERVICE_ORGAN_UNEXISTED = "Đơn vị/phòng ban không tồn tại !";
            public const string SERVICE_ORGAN_EXISTS_ORGAN_CHILD = "Đơn vị/phòng ban hiện tại, chứa Đơn vị/phòng ban con!";
            public const string SERVICE_ORGAN_EXIST_USER = "Đơn vị/phòng ban tồn tại người dùng !";
            public const string SERVICE_ROLE_EXIST_USER = "Vai trò tồn tại người dùng !";
            public const string SERVICE_INVALID_PARAMETER = "Tham số không hợp lệ";
        }
    }
}
