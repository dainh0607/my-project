using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QuanLyVatTu
{
    public class NhanVien
    {
        public string NhanVienID { get; set; }
        public string HoTen { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
        public bool VaiTro { get; set; }
        public bool TinhTrang { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }

        public string VaiTroText => VaiTro ? "Quản lý" : "Nhân viên";
        public string TinhTrangText => TinhTrang ? "Hoạt động" : "Tạm dừng";

    }
}
