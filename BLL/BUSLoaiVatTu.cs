
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Data;
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

        public string Delete(string id)
        {
            string result = dal.Delete(id); // gọi DAL  
            return result == "Success" ? "Success" : "Fail";
        }

        public string GenerateID()
        {
            return dal.GenerateID();
        }

        public List<LoaiVatTu> Search(string keyword)
        {
            string sql = "SELECT * FROM LoaiVatTu WHERE LoaiVatTuID LIKE @0 OR TenLoaiVatTu LIKE @1 OR GhiChu LIKE @2";
            List<object> args = new List<object>
            {
                "%" + keyword + "%",
                "%" + keyword + "%",
                "%" + keyword + "%"
            };
            return dal.SelectBySql(sql, args, CommandType.Text);
        }

        public string Add(LoaiVatTu loai)
        {
            if (string.IsNullOrEmpty(loai.TenLoaiVatTu))
                return "Tên loại vật tư không được để trống.";

            return dal.Insert(loai);
        }

        public string Update(LoaiVatTu loai)
        {
            string result = dal.Update(loai); // gọi DAL  
            return result == "Success" ? "Success" : "Fail";
        }
    }
}
