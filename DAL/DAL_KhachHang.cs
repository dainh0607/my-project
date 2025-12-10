using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_QuanLyVatTu;

namespace DAL_QuanLyVatTu
{
    public class DAL_KhachHang
    {
        public virtual List<KhachHang> SelectBySql(string sql, List<Object> args, CommandType cmdType)
        {
            List<KhachHang> list = new List<KhachHang>();
            SqlDataReader reader = DBUtil.Query(sql, args, CommandType.Text);
            while (reader.Read())
            {
                KhachHang kh = new KhachHang
                {
                    KhachHangID = reader["KhachHangID"].ToString(),
                    HoTen = reader["HoTen"].ToString(),
                    DiaChi = reader["DiaChi"].ToString(),
                    SoDienThoai = reader["SoDienThoai"].ToString(),
                    Email = reader["Email"].ToString(),
                    NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                    GhiChu = reader["GhiChu"].ToString()
                };
                list.Add(kh);
            }
            return list;
        }

        public virtual List<KhachHang> SelectAll()
        {
            string sql = "SELECT * FROM KhachHang";
            return SelectBySql(sql, new List<object>(), CommandType.Text);
        }

        public virtual string Insert(KhachHang kh)
        {
            try
            {
                string sql = "INSERT INTO KhachHang (KhachHangID, HoTen, DiaChi, SoDienThoai, Email, NgayTao, GhiChu) " +
                             "VALUES (@0, @1, @2, @3, @4, @5, @6)";
                List<object> args = new List<object>
                {
                    kh.KhachHangID,
                    kh.HoTen,
                    kh.DiaChi,
                    kh.SoDienThoai,
                    kh.Email,
                    kh.NgayTao,
                    kh.GhiChu
                };
                DBUtil.Update(sql, args);
                return "Thêm thành công";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public virtual string Update(KhachHang kh)
        {
            try
            {
                // 1. Kiểm tra tồn tại
                string checkSql = "SELECT COUNT(1) FROM KhachHang WHERE KhachHangID = @0";
                object exists = DBUtil.ScalarQuery(checkSql, new List<object> { kh.KhachHangID });
                int cnt = 0;
                if (exists != null && int.TryParse(exists.ToString(), out cnt) && cnt > 0)
                {
                    // 2. Thực hiện cập nhật
                    string sql = "UPDATE KhachHang SET HoTen = @1, DiaChi = @2, SoDienThoai = @3, Email = @4, NgayTao = @5, GhiChu = @6 WHERE KhachHangID = @0";
                    List<object> args = new List<object>
            {
                kh.KhachHangID,
                kh.HoTen,
                kh.DiaChi,
                kh.SoDienThoai,
                kh.Email,
                kh.NgayTao,
                kh.GhiChu
            };
                    DBUtil.Update(sql, args);
                    return "Cập nhật thành công";
                }
                else
                {
                    return "Không tìm thấy khách hàng";
                }
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi để test có thể kiểm tra hoặc log
                return ex.Message;
            }
        }

        public virtual string Delete(string id)
        {
            try
            {
                // 1. Kiểm tra tồn tại trước khi xóa
                string checkSql = "SELECT COUNT(1) FROM KhachHang WHERE KhachHangID = @0";
                object exists = DBUtil.ScalarQuery(checkSql, new List<object> { id });
                int cnt = 0;
                if (exists != null && int.TryParse(exists.ToString(), out cnt) && cnt > 0)
                {
                    string sql = "DELETE FROM KhachHang WHERE KhachHangID = @0";
                    DBUtil.Update(sql, new List<object> { id });
                    return "Xóa thành công";
                }
                else
                {
                    return "Không tìm thấy khách hàng";
                }
            }
            catch (SqlException sqlEx)
            {
                // Có thể là ràng buộc FK -> trả về thông báo rõ hơn
                return "Lỗi SQL: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public virtual string GenerateID()
        {
            string prefix = "KH";
            string sql = "SELECT TOP 1 KhachHangID FROM KhachHang WHERE KhachHangID LIKE 'KH%' ORDER BY KhachHangID DESC";
            object result = DBUtil.ScalarQuery(sql, new List<object>());
            if (result != null)
            {
                string currentID = result.ToString();
                string number = currentID.Substring(2);
                if (int.TryParse(number, out int num))
                {
                    return prefix + (num + 1).ToString("D3");
                }
            }
            return prefix + "001";
        }
    }
}
