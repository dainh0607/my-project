using DAL_PolyCafe;
using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL_QuanLyVatTu
{
    public class DAL_ChiTietDonHang
    {
        public List<ChiTietDonHang> SelectBySql(string sql, List<object> args, CommandType cmdType)
        {
            List<ChiTietDonHang> list = new List<ChiTietDonHang>();
            SqlDataReader reader = DBUtil.Query(sql, args, cmdType); // Sửa lại để cmdType được truyền đúng

            while (reader.Read())
            {
                ChiTietDonHang ct = new ChiTietDonHang
                {
                    ChiTietDonHangID = reader["ChiTietDonHangID"].ToString(),
                    DonHangID = reader["DonHangID"].ToString(),
                    VatTuID = reader["VatTuID"].ToString(),
                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                    DonGia = Convert.ToDecimal(reader["DonGia"]),
                    TrangThai = Convert.ToBoolean(reader["TrangThai"])
                };
                list.Add(ct);
            }
            reader.Close(); // Đừng quên đóng reader!
            return list;
        }

        public List<ChiTietDonHang> SelectAll()
        {
            string sql = "SELECT * FROM ChiTietDonHang";
            return SelectBySql(sql, new List<object>(), CommandType.Text);
        }

        public List<ChiTietDonHang> SelectByDonHangID(string donHangID)
        {
            string sql = "SELECT * FROM ChiTietDonHang WHERE DonHangID = @0";
            return SelectBySql(sql, new List<object> { donHangID }, CommandType.Text);
        }

        public string Insert(ChiTietDonHang ct)
        {
            try
            {
                string sql = "INSERT INTO ChiTietDonHang (ChiTietDonHangID, DonHangID, VatTuID, SoLuong, DonGia, TrangThai) " +
                             "VALUES (@0, @1, @2, @3, @4, @5)";
                List<object> args = new List<object>
                {
                    ct.ChiTietDonHangID,
                    ct.DonHangID,
                    ct.VatTuID,
                    ct.SoLuong,
                    ct.DonGia,
                    ct.TrangThai
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(ChiTietDonHang ct)
        {
            try
            {
                string sql = "UPDATE ChiTietDonHang SET DonHangID = @1, VatTuID = @2, SoLuong = @3, DonGia = @4, TrangThai = @5 WHERE ChiTietDonHangID = @0";
                List<object> args = new List<object>
                {
                    ct.ChiTietDonHangID,
                    ct.DonHangID,
                    ct.VatTuID,
                    ct.SoLuong,
                    ct.DonGia,
                    ct.TrangThai
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
            if(string.IsNullOrWhiteSpace(id))
                return "Mã chi tiết đơn hàng không hợp lệ.";
            try
            {
                string sql = "DELETE FROM ChiTietDonHang WHERE ChiTietDonHangID = @0";
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
            string prefix = "CTDH";
            string sql = "SELECT TOP 1 ChiTietDonHangID FROM ChiTietDonHang WHERE ChiTietDonHangID LIKE 'CTDH%' ORDER BY ChiTietDonHangID DESC";
            object result = DBUtil.ScalarQuery(sql, new List<object>());
            if (result != null)
            {
                string currentID = result.ToString();
                string number = currentID.Substring(4);
                if (int.TryParse(number, out int num))
                {
                    return prefix + (num + 1).ToString("D3");
                }
            }
            return prefix + "001";
        }
    }
}
