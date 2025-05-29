using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class ChiTietDonHang
    {
        public string ChiTietDonHangID { get; set; }
        public string DonHangID { get; set; }
        public string VatTuID { get; set; }
        public int SoLuong {  get; set; }
        public decimal DonGia { get; set; }
        public bool TrangThai { get; set; }

        public string TrangThaiText => TrangThai ? "Đã thanh toán" : "Chờ thanh toán";

    }
}
