using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSKhachHang
    {
        private DAL_KhachHang dal = new DAL_KhachHang();

        public List<KhachHang> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(KhachHang kh)
        {

            try
            {
                if (string.IsNullOrEmpty(kh.HoTen) ||
                    string.IsNullOrEmpty(kh.DiaChi) ||
                    string.IsNullOrEmpty(kh.SoDienThoai) ||
                    string.IsNullOrEmpty(kh.Email))

                {
                    return "Vui lòng điền đầy đủ thông tin.";
                }

                if (!Regex.IsMatch(kh.HoTen, @"^[\p{L}\s]+$"))
                {
                    return "Tên người dùng chỉ bao gồm chữ.";
                }

                if (!Regex.IsMatch(kh.SoDienThoai, @"^\d+$"))
                {
                    return "Số điện thoại chỉ bao gồm số.";
                }

                if (kh.SoDienThoai.Length < 10)
                {
                    return "Số điện thoại phải có đủ 10 số.";
                }
                else if (kh.SoDienThoai.Length > 10)
                {
                    return "Số điện thoại đã vượt quá 10 số.";
                }

                if (kh.GhiChu.Length > 255)
                    return "ghi chú không quá 255 ký tự.";

                string[] dauSoHopLe = { "03", "05", "07", "08", "09", "02" };
                if (!dauSoHopLe.Any(ds => kh.SoDienThoai.StartsWith(ds)))
                {
                    return "Đầu số điện thoại phải khớp với các nhà mạng di động (03, 05, 07, 08, 09, 02).";
                }

                if (string.IsNullOrEmpty(kh.Email))
                    return "Vui lòng nhập Email.";
                if (!kh.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                    return "Email phải có đuôi @gmail.com.";

                dal.Insert(kh);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
            ;
        }

        public string Update(KhachHang kh)
        {
            try
            {
                if (string.IsNullOrEmpty(kh.HoTen) ||
                    string.IsNullOrEmpty(kh.DiaChi) ||
                    string.IsNullOrEmpty(kh.SoDienThoai) ||
                    string.IsNullOrEmpty(kh.Email))

                {
                    return "Vui lòng điền đầy đủ thông tin.";
                }

                if (!Regex.IsMatch(kh.HoTen, @"^[\p{L}\s]+$"))
                {
                    return "Tên người dùng chỉ bao gồm chữ.";
                }

                if (!Regex.IsMatch(kh.SoDienThoai, @"^\d+$"))
                {
                    return "Số điện thoại chỉ bao gồm số.";
                }

                if (kh.SoDienThoai.Length < 10)
                {
                    return "Số điện thoại phải có đủ 10 số.";
                }
                else if (kh.SoDienThoai.Length > 10)
                {
                    return "Số điện thoại đã vượt quá 10 số.";
                }

                if (kh.GhiChu.Length > 255)
                    return "ghi chú không quá 255 ký tự.";

                string[] dauSoHopLe = { "03", "05", "07", "08", "09", "02" };
                if (!dauSoHopLe.Any(ds => kh.SoDienThoai.StartsWith(ds)))
                {
                    return "Đầu số điện thoại phải khớp với các nhà mạng di động (03, 05, 07, 08, 09, 02).";
                }

                if (string.IsNullOrEmpty(kh.Email))
                    return "Vui lòng nhập Email.";
                if (!kh.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                    return "Email phải có đuôi @gmail.com.";

                dal.Update(kh);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
            ;
        }

        public string Delete(string id)
        {
            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }
    }
}
