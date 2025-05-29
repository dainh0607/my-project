using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSVatTu
    {
        private DAL_VatTu dal = new DAL_VatTu();

        public List<VatTu> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(VatTu vt)
        {
            if (string.IsNullOrEmpty(vt.TenVatTu))
                return "Tên vật tư không được để trống.";
            if (vt.DonGia <= 0)
                return "Đơn giá phải lớn hơn 0.";
            if (vt.SoLuongTon < 0)
                return "Số lượng tồn không hợp lệ.";
            return dal.Insert(vt);
        }

        public string Update(VatTu vt)
        {
            if (string.IsNullOrEmpty(vt.VatTuID))
                return "Mã vật tư không hợp lệ.";
            return dal.Update(vt);
        }

        public string Delete(string id)
        {
            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }

        public List<VatTu> Search(string keyword)
        {
            string sql = @"SELECT * FROM VatTu WHERE 
                           VatTuID LIKE @0 OR 
                           TenVatTu LIKE @0 OR 
                           GhiChu LIKE @0";
            List<object> args = new List<object> { "%" + keyword + "%" };
            return dal.SelectBySql(sql, args);
        }
    }
}
