using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (string.IsNullOrEmpty(nv.NhanVienID))
                {
                    return "Mã nhân viên không hợp lệ.";
                }
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
            return dalNhanVien.Insert(nv);
        }
    }
}
