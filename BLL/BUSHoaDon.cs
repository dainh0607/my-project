using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSHoaDon
    {
        private DAL_HoaDon dal = new DAL_HoaDon();

        public List<HoaDon> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(HoaDon hd)
        {
            if (string.IsNullOrWhiteSpace(hd.DonHangID))
                return "Đơn hàng không được để trống.";
            if (hd.TongTien <= 0)
                return "Tổng tiền phải lớn hơn 0.";
            if (string.IsNullOrWhiteSpace(hd.PhuongThucThanhToan))
                return "Phương thức thanh toán không được để trống.";

            return dal.Insert(hd);
        }

        public string Update(HoaDon hd)
        {
            if (string.IsNullOrWhiteSpace(hd.HoaDonID))
                return "Mã hóa đơn không hợp lệ.";
            return dal.Update(hd);
        }

        public string Delete(string id)
        {
            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }
    }
}
