using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class InHoaDon
    {
        public string InHoaDonID { get; set; }
        public string DonHangID { get; set; }
        public string NhanVienID { get; set; }
        public DateTime NgayIn { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public InHoaDon()
        {

        }
        public InHoaDon(string inHoaDonID, string donHangID, string nhanVienID, DateTime ngayIn, decimal tongTien, string trangThai, string ghiChu)
        {
            InHoaDonID = inHoaDonID;
            DonHangID = donHangID;
            NhanVienID = nhanVienID;
            NgayIn = ngayIn;
            TongTien = tongTien;
            TrangThai = trangThai;
            GhiChu = ghiChu;
        }
    }
}

