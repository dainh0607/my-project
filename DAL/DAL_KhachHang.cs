using DAL_PolyCafe;
using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_QuanLyVatTu
{
    public class DAL_KhachHang
    {
        public List<KhachHang> SelectBySql(string sql, List<object> args)
        {
            List<KhachHang> list = new List<KhachHang>();
            SqlDataReader reader = DBUtil.Query(sql, args);
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

        public List<KhachHang> SelectAll()
        {
            string sql = "SELECT * FROM KhachHang";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(KhachHang kh)
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
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(KhachHang kh)
        {
            try
            {
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
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Delete(string id)
        {
            try
            {
                string sql = "DELETE FROM KhachHang WHERE KhachHangID = @0";
                DBUtil.Update(sql, new List<object> { id });
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GenerateID()
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
