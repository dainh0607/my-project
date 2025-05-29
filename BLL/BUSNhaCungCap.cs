using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSNhaCungCap
    {
        private DAL_NhaCungCap dal = new DAL_NhaCungCap();

        public List<NhaCungCap> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(NhaCungCap ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.TenNhaCungCap))
                return "Tên nhà cung cấp không được để trống.";
            return dal.Insert(ncc);
        }

        public string Update(NhaCungCap ncc)
        {
            if (string.IsNullOrEmpty(ncc.NhaCungCapID))
                return "Mã nhà cung cấp không hợp lệ.";
            return dal.Update(ncc);
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
