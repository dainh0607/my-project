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
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ncc.TenNhaCungCap))
                errors.Add("Tên nhà cung cấp không được để trống.");

            if (string.IsNullOrWhiteSpace(ncc.SoDienThoai))
                errors.Add("Số điện thoại không được để trống.");
            else if (!Regex.IsMatch(ncc.SoDienThoai, @"^\d{10,11}$"))
                errors.Add("Số điện thoại không hợp lệ (10–11 chữ số).");

            if (string.IsNullOrWhiteSpace(ncc.Email))
                errors.Add("Email không được để trống.");
            else if (!Regex.IsMatch(ncc.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email không hợp lệ.");

            if (string.IsNullOrWhiteSpace(ncc.DiaChi))
                errors.Add("Địa chỉ không được để trống.");

            if (isNew && GetAll().Any(x => x.TenNhaCungCap.Equals(ncc.TenNhaCungCap, StringComparison.OrdinalIgnoreCase)))
                errors.Add("Tên nhà cung cấp đã tồn tại.");

            if (errors.Count > 0)
                return new Result { Success = false, Message = string.Join("\n", errors) };

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
