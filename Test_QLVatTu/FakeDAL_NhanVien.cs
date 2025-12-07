using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu; 

namespace Test_QLVatTu
{
    public class FakeDAL_NhanVien : IDAL_NhanVien
    {
        public List<NhanVien> DatabaseAo = new List<NhanVien>();

        public List<NhanVien> selectAll()
        {
            return DatabaseAo;
        }

        public NhanVien getNhanVien1(string email, string matKhau)
        {
            return DatabaseAo.FirstOrDefault(x => x.Email == email);
        }

        public string Insert(NhanVien nv)
        {
            DatabaseAo.Add(nv);
            return "";
        }

        public void updateNhanVien(NhanVien nv)
        {
            var existing = DatabaseAo.FirstOrDefault(x => x.Email == nv.Email);
            if (existing != null)
            {
                existing.HoTen = nv.HoTen;
            }
        }

        public string Delete(string maNhanVien)
        {
            var item = DatabaseAo.FirstOrDefault(x => x.Email == maNhanVien);
            if (item != null)
            {
                DatabaseAo.Remove(item);
                return "";
            }
            return "Lỗi: Không tìm thấy nhân viên để xóa";
        }

        public List<NhanVien> SelectBySql(string sql, List<object> args)
        {
            return new List<NhanVien>();
        }
        public string generateMaNhanVien()
        {
            return "NV_AUTO_TEST";
        }

        public bool KiemTraLienKetDuLieu(string maNhanVien)
        {
            return false;
        }
    }
}