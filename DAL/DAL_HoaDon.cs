using DAL_PolyCafe;
using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_QuanLyVatTu
{
    public class DAL_HoaDon
    {
        public List<HoaDon> SelectBySql(string sql, List<Object> args, CommandType cmdType)
        {
            List<HoaDon> list = new List<HoaDon>();
            SqlDataReader reader = DBUtil.Query(sql, args, CommandType.Text);
            while (reader.Read())
            {
                HoaDon hd = new HoaDon
                {
                    HoaDonID = reader["HoaDonID"].ToString(),
                    DonHangID = reader["DonHangID"].ToString(),
                    TongTien = Convert.ToDecimal(reader["TongTien"]),
                    NgayThanhToan = Convert.ToDateTime(reader["NgayThanhToan"]),
                    PhuongThucThanhToan = reader["PhuongThucThanhToan"].ToString()
                };
                list.Add(hd);
            }
            return list;
        }

        public List<HoaDon> SelectAll()
        {
            string sql = "SELECT * FROM HoaDon";
            return SelectBySql(sql, new List<object>(), CommandType.Text);
        }

        public string Insert(HoaDon hd)
        {
            try
            {
                string sql = "INSERT INTO HoaDon (HoaDonID, DonHangID, TongTien, NgayThanhToan, PhuongThucThanhToan) " +
                             "VALUES (@0, @1, @2, @3, @4)";
                List<object> args = new List<object>
                {
                    hd.HoaDonID,
                    hd.DonHangID,
                    hd.TongTien,
                    hd.NgayThanhToan,
                    hd.PhuongThucThanhToan
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(HoaDon hd)
        {
            try
            {
                string sql = "UPDATE HoaDon SET DonHangID = @1, TongTien = @2, NgayThanhToan = @3, PhuongThucThanhToan = @4 WHERE HoaDonID = @0";
                List<object> args = new List<object>
                {
                    hd.HoaDonID,
                    hd.DonHangID,
                    hd.TongTien,
                    hd.NgayThanhToan,
                    hd.PhuongThucThanhToan
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
                string sql = "DELETE FROM HoaDon WHERE HoaDonID = @0";
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
            string prefix = "HD";
            string sql = "SELECT TOP 1 HoaDonID FROM HoaDon WHERE HoaDonID LIKE 'HD%' ORDER BY HoaDonID DESC";
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
