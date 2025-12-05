using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace BLL_QuanLyVatTu
{
    public class BUSDangNhap
    {
        private readonly IDAL_NhanVien _dal;

        public BUSDangNhap(IDAL_NhanVien dal)
        {
            _dal = dal;
        }

        public string MaHoaMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

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
            NhanVien nhanVienHopLe = _dal.getNhanVien1(email, matKhau);

            if (nhanVienHopLe == null)
            {
                return "Mật khẩu không hợp lệ!";
            }
            return "Đăng nhập thành công!";
        }

        private NhanVien TimNhanVienTheoEmail(string email)
        {
            var danhSach = _dal.selectAll();
            foreach (var nv in danhSach)
            {
                if (nv.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    return nv;
            }
            return null;
        }
    }
}
