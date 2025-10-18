using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace BLL_QuanLyVatTu
{
    public class BUSDangNhap
    {
        private DAL_NhanVien dalNhanVien = new DAL_NhanVien();

        public string KiemTraDangNhap(string email, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(matKhau))
            {
                return "Email và mật khẩu không được để trống!";
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                return "Email không được để trống!";
            }
            else if (string.IsNullOrWhiteSpace(matKhau))
            {
                return "Mật khẩu không được để trống!";
            }
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!Regex.IsMatch(email, pattern))
            {
                return "Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com";
            }
            NhanVien nhanVienTheoEmail = TimNhanVienTheoEmail(email);

            if (nhanVienTheoEmail == null)
            {
                return "Email không hợp lệ!";
            }
            NhanVien nhanVienHopLe = dalNhanVien.getNhanVien1(email, matKhau);

            if (nhanVienHopLe == null)
            {
                return "Mật khẩu không hợp lệ!";
            }
            return "Đăng nhập thành công!";
        }

        private NhanVien TimNhanVienTheoEmail(string email)
        {
            var danhSach = dalNhanVien.selectAll();
            foreach (var nv in danhSach)
            {
                if (nv.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    return nv;
            }
            return null;
        }
    }
}
