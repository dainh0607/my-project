using DAL_PolyCafe;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL_QuanLyVatTu.BUSNhaCungCap;

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
            var result = Validate(vt, true);
            if (!result.Success) return result.Message;
            return dal.Insert(vt);
          
        }
        private Result Validate(VatTu vt, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(vt.VatTuID))
                return new Result { Success = false, Message = "Mã vật tư không được để trống." };

            if (isNew && GetAll().Any(x => x.VatTuID.Equals(vt.VatTuID, StringComparison.OrdinalIgnoreCase)))
                return new Result { Success = false, Message = "Mã vật tư đã tồn tại." };

            if (string.IsNullOrWhiteSpace(vt.TenVatTu))
                return new Result { Success = false, Message = "Tên vật tư không được để trống." };

            if (string.IsNullOrWhiteSpace(vt.LoaiVatTuID))
                return new Result { Success = false, Message = "Loại vật tư không được để trống." };

            if (string.IsNullOrWhiteSpace(vt.NhaCungCapID))
                return new Result { Success = false, Message = "Nhà cung cấp không được để trống." };

            if (string.IsNullOrWhiteSpace(vt.TrangThaiID))
                return new Result { Success = false, Message = "Trạng thái không được để trống." };

            if (vt.DonGia <= 0)
                return new Result { Success = false, Message = "Đơn giá không hợp lệ " };

            if (vt.SoLuongTon < 0)
                return new Result { Success = false, Message = "Số lượng tồn không hợp lệ." };

            return new Result { Success = true };
        }
        public string Update(VatTu vt)
        {
            if (string.IsNullOrEmpty(vt.VatTuID))
                return "Mã vật tư không hợp lệ.";
            if (string.IsNullOrEmpty(vt.TenVatTu))
                return "Tên vật tư không được để trống.";
            if (string.IsNullOrEmpty(vt.LoaiVatTuID))
                return "Loại vật tư không được để trống.";
            if (string.IsNullOrEmpty(vt.NhaCungCapID))
                return "Nhà cung cấp không được để trống.";
            if (string.IsNullOrEmpty(vt.TrangThaiID))
                return "Trạng thái không được để trống.";
            if (vt.DonGia <= 0)
                return "Đơn giá phải lớn hơn 0.";
            if (vt.SoLuongTon < 0)
                return "Số lượng tồn không hợp lệ.";
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

        public string DeleteVatTu(string vatTuID)
        {
            if (dal.IsVatTuInUse(vatTuID))
                return "Không thể xóa vật tư này vì đã phát sinh đơn hàng hoặc phiếu liên quan.";

            return dal.Delete(vatTuID) ?? "Xóa vật tư thành công.";
        }



    }
}
