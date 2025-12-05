using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_QuanLyVatTu;

namespace DAL_QuanLyVatTu
{
    public interface IDAL_NhanVien
    {
        List<NhanVien> selectAll();

        NhanVien getNhanVien1(string email, string matKhau);

        string Insert(NhanVien nhanVien);

        void updateNhanVien(NhanVien nhanVien);

        string Delete(string maNhanVien);

        List<NhanVien> SelectBySql(string sql, List<object> args);

        string generateMaNhanVien();
    }
}
