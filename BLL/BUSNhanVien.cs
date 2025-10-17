using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSNhanVien
    {
        DAL_NhanVien dalNhanVien = new DAL_NhanVien();
        public NhanVien DangNhap(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            return dalNhanVien.getNhanVien1(username, password);
        }

        public List<NhanVien> GetNhanVienList()
        {
            return dalNhanVien.selectAll();
        }

        public string UpdateNhanVien(NhanVien nv)
        {
            try
            {
                if (string.IsNullOrEmpty(nv.HoTen) ||
                    string.IsNullOrEmpty(nv.ChucVu) ||
                    string.IsNullOrEmpty(nv.SoDienThoai) ||
                    string.IsNullOrEmpty(nv.Email) ||
                    string.IsNullOrEmpty(nv.MatKhau))
                {
                    return "Vui lòng điền đầy đủ thông tin.";
                }

                if (!Regex.IsMatch(nv.HoTen, @"^[\p{L}\s]+$"))
                {
                    return "Tên người dùng chỉ bao gồm chữ.";
                }

                if (!Regex.IsMatch(nv.SoDienThoai, @"^\d+$"))
                {
                    return "Số điện thoại chỉ bao gồm số.";
                }

                if (nv.SoDienThoai.Length < 10)
                {
                    return "Số điện thoại phải có đủ 10 số.";
                }
                else if (nv.SoDienThoai.Length > 10)
                {
                    return "Số điện thoại đã vượt quá 10 số.";
                }

                if (nv.MatKhau.Length < 6)
                    return "Mật khẩu phải có tối thiểu 6 ký tự.";

                string[] dauSoHopLe = { "03", "05", "07", "08", "09", "02" };
                if (!dauSoHopLe.Any(ds => nv.SoDienThoai.StartsWith(ds)))
                {
                    return "Đầu số điện thoại phải khớp với các nhà mạng di động (03, 05, 07, 08, 09, 02).";
                }

                if (string.IsNullOrEmpty(nv.Email))
                    return "Vui lòng nhập Email.";
                if (!nv.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                    return "Email phải có đuôi @gmail.com.";

                dalNhanVien.updateNhanVien(nv);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        public string DeleteNhanVien(string nv)
        {
            try
            {
                dalNhanVien.Delete(nv);
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        public List<NhanVien> SearchNhanVien(string keyword)
        {
            string sql = "SELECT * FROM NhanVien WHERE NhanVienID LIKE @0 OR HoTen LIKE @0 OR Email LIKE @0 OR ChucVu LIKE @0 OR VaiTro LIKE @0";
            List<object> args = new List<object> { "%" + keyword + "%" };
            return dalNhanVien.SelectBySql(sql, args);
        }

        public string TaoMaNhanVienTuDong()
        {
            return dalNhanVien.generateMaNhanVien();
        }

        public string InsertNhanVien(NhanVien nv)
        {
            if (string.IsNullOrEmpty(nv.HoTen) &&
                string.IsNullOrEmpty(nv.ChucVu) &&
                string.IsNullOrEmpty(nv.SoDienThoai) &&
                string.IsNullOrEmpty(nv.Email) &&
                string.IsNullOrEmpty(nv.MatKhau))
            {
                return "Vui lòng điền đầy đủ thông tin.";
            }

            if (string.IsNullOrEmpty(nv.SoDienThoai))
            {
                return "Vui lòng Nhập số điện thoại.";
            }

            if (string.IsNullOrEmpty(nv.Email))
            {
                return "Vui lòng Nhập Email.";
            }

            if (!Regex.IsMatch(nv.HoTen, @"^[\p{L}\s]+$"))
            {
                return "Tên người dùng chỉ bao gồm chữ.";
            }

            if (!Regex.IsMatch(nv.SoDienThoai, @"^\d+$"))
            {
                return "Số điện thoại chỉ bao gồm số.";
            }

            if (nv.SoDienThoai.Length < 10)
            {
                return "Số điện thoại phải có đủ 10 số.";
            }
            else if (nv.SoDienThoai.Length > 10)
            {
                return "Số điện thoại đã vượt quá 10 số.";
            }

            if (nv.MatKhau.Length < 6)
                return "Mật khẩu phải có tối thiểu 6 ký tự.";

            string[] dauSoHopLe = { "03", "05", "07", "08", "09", "02" };
            if (!dauSoHopLe.Any(ds => nv.SoDienThoai.StartsWith(ds)))
            {
                return "Đầu số điện thoại phải khớp với các nhà mạng di động (03, 05, 07, 08, 09, 02).";
            }

            if (!nv.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                return "Email phải có đuôi @gmail.com.";

            var dsNhanVien = dalNhanVien.selectAll();
            bool trung = dsNhanVien.Any(x =>
                x.Email.Equals(nv.Email, StringComparison.OrdinalIgnoreCase) ||
                x.SoDienThoai.Equals(nv.SoDienThoai));

            if (trung)
            {
                return "Nhân viên này đã tồn tại trong hệ thống.";
            }
            return dalNhanVien.Insert(nv);
        }

    }
}
