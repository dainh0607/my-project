using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;
using System;
using System.Collections.Generic;

namespace BUS_QuanLyVatTu
{
    public class BUS_InHoaDon
    {
        private DAL_InHoaDon dalInHoaDon = new DAL_InHoaDon();

        // Lấy tất cả hóa đơn in
        public List<InHoaDon> LayTatCaHoaDon()
        {
            try
            {
                return dalInHoaDon.SelectAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
        }

        // Lấy hóa đơn theo ID
        public InHoaDon LayHoaDonTheoID(string inHoaDonID)
        {
            try
            {
                if (string.IsNullOrEmpty(inHoaDonID))
                    throw new ArgumentException("ID hóa đơn không được để trống");

                return dalInHoaDon.SelectByID(inHoaDonID);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin hóa đơn: " + ex.Message);
            }
        }

        // Lấy hóa đơn theo đơn hàng
        public List<InHoaDon> LayHoaDonTheoDonHang(string donHangID)
        {
            try
            {
                if (string.IsNullOrEmpty(donHangID))
                    throw new ArgumentException("ID đơn hàng không được để trống");

                return dalInHoaDon.SelectByDonHangID(donHangID);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo đơn hàng: " + ex.Message);
            }
        }

        // Thêm hóa đơn mới
        public string ThemHoaDon(InHoaDon hoaDon)
        {
            try
            {
                // Validate dữ liệu
                string validationError = ValidateHoaDon(hoaDon);
                if (!string.IsNullOrEmpty(validationError))
                    return validationError;

                // Tự động sinh ID nếu chưa có
                if (string.IsNullOrEmpty(hoaDon.InHoaDonID))
                    hoaDon.InHoaDonID = dalInHoaDon.GenerateID();

                // Thiết lập ngày in nếu chưa có
                if (hoaDon.NgayIn == DateTime.MinValue)
                    hoaDon.NgayIn = DateTime.Now;

                return dalInHoaDon.Insert(hoaDon);
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm hóa đơn: " + ex.Message;
            }
        }

        // Cập nhật hóa đơn
        public string CapNhatHoaDon(InHoaDon hoaDon)
        {
            try
            {
                // Validate dữ liệu
                string validationError = ValidateHoaDon(hoaDon);
                if (!string.IsNullOrEmpty(validationError))
                    return validationError;

                if (string.IsNullOrEmpty(hoaDon.InHoaDonID))
                    return "ID hóa đơn không được để trống";

                return dalInHoaDon.Update(hoaDon);
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật hóa đơn: " + ex.Message;
            }
        }

        // Xóa hóa đơn
        public string XoaHoaDon(string inHoaDonID)
        {
            try
            {
                if (string.IsNullOrEmpty(inHoaDonID))
                    return "ID hóa đơn không được để trống";

                // Kiểm tra tồn tại trước khi xóa
                var existing = dalInHoaDon.SelectByID(inHoaDonID);
                if (existing == null)
                    return "Hóa đơn không tồn tại";

                return dalInHoaDon.Delete(inHoaDonID);
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa hóa đơn: " + ex.Message;
            }
        }

        // Tìm kiếm hóa đơn
        public List<InHoaDon> TimKiemHoaDon(string keyword, DateTime? fromDate, DateTime? toDate, string trangThai)
        {
            try
            {
                return dalInHoaDon.Search(keyword, fromDate, toDate, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hóa đơn: " + ex.Message);
            }
        }

        // Cập nhật trạng thái hóa đơn
        public string CapNhatTrangThai(string inHoaDonID, string trangThai)
        {
            try
            {
                if (string.IsNullOrEmpty(inHoaDonID))
                    return "ID hóa đơn không được để trống";

                if (string.IsNullOrEmpty(trangThai))
                    return "Trạng thái không được để trống";

                return dalInHoaDon.UpdateTrangThai(inHoaDonID, trangThai);
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật trạng thái: " + ex.Message;
            }
        }

        // Sinh ID mới
        public string SinhIDMoi()
        {
            try
            {
                return dalInHoaDon.GenerateID();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh ID mới: " + ex.Message);
            }
        }

        // Lấy thống kê theo trạng thái
        public int DemTheoTrangThai(string trangThai)
        {
            try
            {
                return dalInHoaDon.CountByTrangThai(trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đếm hóa đơn: " + ex.Message);
            }
        }

        // Validate dữ liệu hóa đơn
        private string ValidateHoaDon(InHoaDon hoaDon)
        {
            if (hoaDon == null)
                return "Thông tin hóa đơn không được để trống";

            if (string.IsNullOrEmpty(hoaDon.DonHangID))
                return "ID đơn hàng không được để trống";

            if (string.IsNullOrEmpty(hoaDon.NhanVienID))
                return "ID nhân viên không được để trống";

            if (hoaDon.TongTien < 0)
                return "Tổng tiền không được âm";

            if (string.IsNullOrEmpty(hoaDon.TrangThai))
                return "Trạng thái không được để trống";

            return null;
        }

        // Lấy danh sách trạng thái có thể có
        public List<string> LayDanhSachTrangThai()
        {
            return new List<string>
            {
                "Đã in",
                "Chờ in",
                "Hủy",
                "Đã giao"
            };
        }
    }
}