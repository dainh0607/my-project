using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class ThongKeDoanhThu
    {
        public string DonHangID { get; set; }
        public string ChiTietDonHangID { get; set; }
        public string KhachHangID { get; set; }
        public string NhanVienID { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal DonGia { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }

        public ThongKeDoanhThu() { }

        public ThongKeDoanhThu(string donHangID, string chiTietDonHangID, string khachHangID,
                          string nhanVienID, DateTime ngayDat, decimal donGia,
                          string phuongThucThanhToan, string trangThai, string ghiChu)
        {
            DonHangID = donHangID;
            ChiTietDonHangID = chiTietDonHangID;
            KhachHangID = khachHangID;
            NhanVienID = nhanVienID;
            NgayDat = ngayDat;
            DonGia = donGia;
            PhuongThucThanhToan = phuongThucThanhToan;
            TrangThai = trangThai;
            GhiChu = ghiChu;
        }
    }
}
