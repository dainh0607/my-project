using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;

namespace BLL_QuanLyVatTu
{
    public class BUS_ThongKeDoanhThuTheoVatTu
    {
        private DAL_ThongKeDoanhThuTheoVatTu dal = new DAL_ThongKeDoanhThuTheoVatTu();

        public List<ThongKeDoanhThuTheoVatTu> GetAll()
        {
            return dal.SelectAll();
        }

        public string Add(ThongKeDoanhThuTheoVatTu tk)
        {
            // Có thể kiểm tra nghiệp vụ ở đây nếu cần
            if (string.IsNullOrWhiteSpace(tk.LoaiVatTuID))
                return "Vui lòng chọn loại vật tư.";
            return dal.Insert(tk);
        }

        public List<DAL_QuanLyVatTu.LoaiVatTu> GetAllLoaiVatTu()
        {
            return dal.GetAllLoaiVatTu();
        }
    }
}
