using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSThongKeDoanhThu
    {
        private readonly DAL_ThongKeDoanhThu dalThongKe = new DAL_ThongKeDoanhThu();
        private readonly DAL_NhanVien dalNhanVien = new DAL_NhanVien();
        private readonly DAL_KhachHang dalKhachHang = new DAL_KhachHang();

        public virtual List<ThongKeDoanhThu> SelectAll()
        {
            return dalThongKe.SelectAll();
        }

        public virtual List<ThongKeDoanhThu> ThongKeTheoDieuKien(
            DateTime fromDate,
            DateTime toDate,
            string nhanVienID,
            string khachHangID,
            string trangThai,
            string phuongThuc)
        {
            return dalThongKe.SelectByFilter(fromDate, toDate, nhanVienID, khachHangID, trangThai, phuongThuc);
        }

        public virtual DataTable GetNhanVienList()
        {
            var list = dalNhanVien.selectAll();
            DataTable dt = new DataTable();
            dt.Columns.Add("NhanVienID");
            dt.Columns.Add("HoTen");
            foreach (var nv in list)
            {
                dt.Rows.Add(nv.NhanVienID, nv.HoTen);
            }
            return dt;
        }

        public virtual DataTable GetKhachHangList()
        {
            var list = dalKhachHang.SelectAll();
            DataTable dt = new DataTable();
            dt.Columns.Add("KhachHangID");
            dt.Columns.Add("HoTen");
            foreach (var kh in list)
            {
                dt.Rows.Add(kh.KhachHangID, kh.HoTen);
            }
            return dt;
        }
    }
}
