using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_QuanLyVatTu
{
    public class BUSLoaiVatTu
    {
        private DAL_LoaiVatTu dal = new DAL_LoaiVatTu();

        public List<LoaiVatTu> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(LoaiVatTu lvt)
        {
            if (string.IsNullOrEmpty(lvt.TenLoaiVatTu))
                return "Tên loại vật tư không được để trống.";

            return dal.Insert(lvt);
        }

        public string Update(LoaiVatTu lvt)
        {
            if (string.IsNullOrEmpty(lvt.LoaiVatTuID))
                return "Mã loại vật tư không hợp lệ.";

            return dal.Update(lvt);
        }

        public string Delete(string id)
        {
            return dal.Delete(id);
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }

        public List<LoaiVatTu> Search(string keyword)
        {
            string sql = "SELECT * FROM LoaiVatTu WHERE LoaiVatTuID LIKE @0 OR TenLoaiVatTu LIKE @0 OR GhiChu LIKE @0";
            List<object> args = new List<object> { "%" + keyword + "%" };
            return dal.SelectBySql(sql, args);
        }
    }
}
