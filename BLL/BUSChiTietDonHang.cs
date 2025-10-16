using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSChiTietDonHang
    {
        private DAL_ChiTietDonHang dal = new DAL_ChiTietDonHang();

        public List<ChiTietDonHang> GetAll()
        {
            return dal.SelectAll();
        }

        public List<ChiTietDonHang> GetByDonHangID(string donHangID)
        {
            return dal.SelectByDonHangID(donHangID);
        }

        public string Add(ChiTietDonHang ct)
        {
            if (string.IsNullOrWhiteSpace(ct.DonHangID) || string.IsNullOrWhiteSpace(ct.VatTuID))
                return "Mã đơn hàng và vật tư không được để trống.";
            if (ct.SoLuong <= 0)
                return "Số lượng phải lớn hơn 0.";
            if (ct.DonGia < 0)
                return "Đơn giá không hợp lệ.";

            return dal.Insert(ct);
        }

        public string Update(ChiTietDonHang ct)
        {
            if (string.IsNullOrWhiteSpace(ct.ChiTietDonHangID))
                return "Mã chi tiết đơn hàng không hợp lệ.";
            return dal.Update(ct);
        }

        public string Delete(string ct)
        {
            return dal.Delete(ct);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }
    }


}
