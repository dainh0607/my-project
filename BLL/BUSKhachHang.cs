using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSKhachHang
    {
        private DAL_KhachHang dal = new DAL_KhachHang();

        public List<KhachHang> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(KhachHang kh)
        {
            if (string.IsNullOrWhiteSpace(kh.HoTen))
                return "Họ tên khách hàng không được để trống.";
            return dal.Insert(kh);
        }

        public string Update(KhachHang kh)
        {
            if (string.IsNullOrWhiteSpace(kh.KhachHangID))
                return "Mã khách hàng không hợp lệ.";
            return dal.Update(kh);
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
