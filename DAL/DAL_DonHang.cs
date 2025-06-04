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
    public class DAL_DonHang
    {
        public List<DonHang> SelectBySql(string sql, List<object> args)
        {
            List<DonHang> list = new List<DonHang>();
            SqlDataReader reader = DBUtil.Query(sql, args);
            while (reader.Read())
            {
                DonHang dh = new DonHang
                {
                    DonHangID = reader["DonHangID"].ToString(),
                    KhachHangID = reader["KhachHangID"].ToString(),
                    NhanVienID = reader["NhanVienID"].ToString(),
                    NgayDat = Convert.ToDateTime(reader["NgayDat"]),
                    TrangThai = reader["TrangThai"] == DBNull.Value ? "" : reader["TrangThai"].ToString(),
                    GhiChu = reader["GhiChu"].ToString()
                };
                list.Add(dh);
            }
            return list;
        }

        public List<DonHang> SelectAll()
        {
            string sql = "SELECT * FROM DonHang";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(DonHang dh)
        {
            try
            {
                string sql = "INSERT INTO DonHang (DonHangID, KhachHangID, NhanVienID, NgayDat, TrangThai, GhiChu) " +
                             "VALUES (@0, @1, @2, @3, @4, @5)";
                List<object> args = new List<object>
                {
                    dh.DonHangID,
                    dh.KhachHangID,
                    dh.NhanVienID,
                    dh.NgayDat,
                    dh.TrangThai,
                    dh.GhiChu
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(DonHang dh)
        {
            try
            {
                string sql = "UPDATE DonHang SET KhachHangID = @1, NhanVienID = @2, NgayDat = @3, TrangThai = @4, GhiChu = @5 WHERE DonHangID = @0";
                List<object> args = new List<object>
                {
                    dh.DonHangID,
                    dh.KhachHangID,
                    dh.NhanVienID,
                    dh.NgayDat,
                    dh.TrangThai,
                    dh.GhiChu
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
                string sql = "DELETE FROM DonHang WHERE DonHangID = @0";
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
            string prefix = "DH";
            string sql = "SELECT TOP 1 DonHangID FROM DonHang WHERE DonHangID LIKE 'DH%' ORDER BY DonHangID DESC";
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
