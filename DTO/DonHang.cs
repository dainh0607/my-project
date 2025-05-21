using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class DonHang
    {
        public string DonHangID { get; set; }
        public string KhachHangID { get; set; }
        public string NhanVienID { get; set; }
        public DateTime NgayDat { get; set; }
        public bool TrangThai { get; set; }
        public string GhiChu { get; set; }

    }
}
