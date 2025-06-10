using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class ThongKeDoanhThuTheoVatTu
    {
        public string MaThongKe { get; set; }
        public string LoaiVatTuID { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int TongVatTu { get; set; }
        public int TongNhap { get; set; }
        public int TongXuat { get; set; }
        public int TonKho { get; set; }
        public string GhiChu { get; set; }
    }
}
