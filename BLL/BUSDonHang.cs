using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSDonHang
    {
        private DAL_DonHang dal = new DAL_DonHang();

        public List<DonHang> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(DonHang dh)
        {
            if (string.IsNullOrWhiteSpace(dh.KhachHangID))
                return "Khách hàng không được để trống.";
            if (string.IsNullOrWhiteSpace(dh.NhanVienID))
                return "Nhân viên không được để trống.";
            return dal.Insert(dh);
        }

        public string Update(DonHang dh)
        {
            if (string.IsNullOrWhiteSpace(dh.DonHangID))
                return "Mã đơn hàng không hợp lệ.";
            return dal.Update(dh);
        }

        public string Delete(string id)
        {
            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }

        public DonHang GetByID(string id)
        {
            return dal.GetByID(id);
        }

    }
}
