using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSTrangThaiVatTu
    {
        public class BUS_TrangThaiVatTu
        {
            private DAL_TrangThaiVatTu dal = new DAL_TrangThaiVatTu();

            public List<TrangThaiVatTu> GetAll()
            {
                return dal.SelectAll();
            }

            public string Add(TrangThaiVatTu ttv)
            {
                if (string.IsNullOrWhiteSpace(ttv.TenTrangThai))
                    return "Tên trạng thái không được để trống.";
                return dal.Insert(ttv);
            }

            public string Update(TrangThaiVatTu ttv)
            {
                if (string.IsNullOrEmpty(ttv.TrangThaiID))
                    return "Mã trạng thái không hợp lệ.";
                return dal.Update(ttv);
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
}
