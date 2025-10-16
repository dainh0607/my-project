
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL_QuanLyVatTu.BUSNhaCungCap;

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
            var result = Validate(loai, true);
            if (!result.Success) return result.Message;
            return dal.Insert(loai);
        }
        private Result Validate(LoaiVatTu loai, bool isNew)
        {
      

            if (isNew && GetAll().Any(x => x.TenLoaiVatTu.Equals(loai.TenLoaiVatTu, StringComparison.OrdinalIgnoreCase)))
                return new Result { Success = false, Message = "Tên loại vật tư đã tồn tại." };

            if (string.IsNullOrWhiteSpace(loai.TenLoaiVatTu))
                return new Result { Success = false, Message = "Tên vật tư không được để trống." };

            if (string.IsNullOrWhiteSpace(loai.LoaiVatTuID))
                return new Result { Success = false, Message = "Mã Loại vật tư không được để trống." };
            if (!string.IsNullOrWhiteSpace(loai.GhiChu) && loai.GhiChu.Length > 255)
                return new Result { Success = false, Message = "Ghi chú không được vượt quá 255 ký tự." };
            return new Result { Success = true };
        }
        public string Update(LoaiVatTu loai)
        {
            string result = dal.Update(loai); // gọi DAL  
            return result == "Success" ? "Success" : "Fail";
        }
    }
}
