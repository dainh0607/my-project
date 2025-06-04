using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class VatTu
    {
        public string VatTuID { get; set; }
        public string LoaiVatTuID { get; set; }
        public string TenVatTu { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }
        public string NhaCungCapID { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string TrangThaiID { get; set; }
    }
}
