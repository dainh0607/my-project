using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;

namespace BLL_QuanLyVatTu
{
    public class BUSHoaDon
    {
        private DAL_HoaDon dal = new DAL_HoaDon();

        public List<HoaDon> SelectAll()
        {
            return dal.SelectAll();
        }

        public string Add(HoaDon hd)
        {
            if (string.IsNullOrWhiteSpace(hd.DonHangID))
                return "Đơn hàng không được để trống.";

            DAL_DonHang dalDonHang = new DAL_DonHang();
            hd.KhachHangID = dalDonHang.GetKhachHangIDByDonHangID(hd.DonHangID);

            if (string.IsNullOrWhiteSpace(hd.KhachHangID))
                return "Không tìm thấy khách hàng cho đơn hàng này.";

            if (hd.TongTien <= 0)
                return "Tổng tiền phải lớn hơn 0 (tính từ chi tiết đơn hàng).";

            if (string.IsNullOrWhiteSpace(hd.PhuongThucThanhToan))
                return "Phương thức thanh toán không được để trống.";

            return dal.Insert(hd);
        }

        public string Update(HoaDon hd)
        {
            if (string.IsNullOrWhiteSpace(hd.HoaDonID))
                return "Mã hóa đơn không hợp lệ.";

            var hoaDonList = dal.SelectAll();
            var current = hoaDonList.Find(x => x.HoaDonID == hd.HoaDonID);
            if (current != null && current.NgayThanhToan != DateTime.MinValue)
            {
                return "Hóa đơn đã thanh toán, không được phép sửa.";
            }

            return dal.Update(hd);
        }

        public string Delete(string id)
        {
            var hoaDonList = dal.SelectAll();
            var current = hoaDonList.Find(x => x.HoaDonID == id);
            if (current != null && current.NgayThanhToan != DateTime.MinValue)
            {
                return "Hóa đơn đã thanh toán, không được phép xóa.";
            }

            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }
    }
}
