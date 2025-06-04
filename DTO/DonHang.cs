using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class DonHang
    {
        public string MaDonHang { get; set; }
        public string MaKhachHang { get; set; }
        public string MaNhanVien { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; }   
        public string GhiChu { get; set; }

        public DonHang(string maDH, string maKH, string maNV, DateTime ngayDat, string trangThai, string ghiChu)
        {
            MaDonHang = maDH;
            MaKhachHang = maKH;
            MaNhanVien = maNV;
            NgayDat = ngayDat;
            TrangThai = trangThai;
            GhiChu = ghiChu;
        }

    }
}
