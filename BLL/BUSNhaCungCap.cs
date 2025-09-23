using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


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
            var result = Validate(ncc, true);
            if (!result.Success) return result.Message;
            return dal.Insert(ncc);
        }

        public string Update(NhaCungCap ncc)
        {
            var result = Validate(ncc, false);
            if (!result.Success) return result.Message;
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

        public class Result
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        private Result Validate(NhaCungCap ncc, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(ncc.TenNhaCungCap))
                return new Result { Success = false, Message = "Tên nhà cung cấp không được để trống." };

            if (!string.IsNullOrWhiteSpace(ncc.SoDienThoai) && !System.Text.RegularExpressions.Regex.IsMatch(ncc.SoDienThoai, @"^\d{10,11}$"))
                return new Result { Success = false, Message = "Số điện thoại không hợp lệ (10–11 chữ số)." };

            if (!string.IsNullOrWhiteSpace(ncc.Email) && !System.Text.RegularExpressions.Regex.IsMatch(ncc.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return new Result { Success = false, Message = "Email không hợp lệ." };

            if (isNew && GetAll().Any(x => x.TenNhaCungCap.Equals(ncc.TenNhaCungCap, StringComparison.OrdinalIgnoreCase)))
                return new Result { Success = false, Message = "Tên nhà cung cấp đã tồn tại." };

            return new Result { Success = true };
        }

        public static string RemoveVietnameseSigns(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var c in text)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }


        public List<NhaCungCap> Search(string keyword)
        {
            keyword = RemoveVietnameseSigns(keyword.Trim().ToLower());
            return GetAll()
                .Where(n =>
                    RemoveVietnameseSigns(n.TenNhaCungCap.ToLower()).Contains(keyword) ||
                    RemoveVietnameseSigns(n.SoDienThoai.ToLower()).Contains(keyword) ||
                    RemoveVietnameseSigns(n.Email.ToLower()).Contains(keyword) ||
                    RemoveVietnameseSigns(n.DiaChi.ToLower()).Contains(keyword))
                .ToList();
        }
    }
}
