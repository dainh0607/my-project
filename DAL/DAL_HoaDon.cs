using DAL_PolyCafe;
using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL_QuanLyVatTu
{
    public class DAL_HoaDon
    {
        public static readonly List<string> PaymentMethods = new List<string>
        {
            "Chuyển khoản",
            "Tiền mặt",
            "Quẹt thẻ"
        };

        public List<HoaDon> SelectBySql(string sql, List<object> args, CommandType cmdType = CommandType.Text)
        {
            List<HoaDon> list = new List<HoaDon>();

            using (SqlCommand cmd = DBUtil.GetCommand(sql, args, cmdType))
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Lấy schema để kiểm tra cột có tồn tại không
                    var schemaTable = reader.GetSchemaTable();
                    var columnNames = new HashSet<string>(
                        schemaTable.Rows.Cast<DataRow>().Select(r => r["ColumnName"].ToString())
                    );

                    while (reader.Read())
                    {
                        HoaDon hd = new HoaDon
                        {
                            HoaDonID = reader["HoaDonID"].ToString(),
                            DonHangID = reader["DonHangID"].ToString(),
                            TongTien = reader["TongTien"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTien"]),
                            NgayThanhToan = reader["NgayThanhToan"] == DBNull.Value
                                ? DateTime.MinValue
                                : Convert.ToDateTime(reader["NgayThanhToan"]),
                            PhuongThucThanhToan = reader["PhuongThucThanhToan"] == DBNull.Value
                                ? "Tiền mặt"
                                : reader["PhuongThucThanhToan"].ToString()
                        };

                        // Chỉ gán KhachHangID nếu SQL trả ra cột này
                        if (columnNames.Contains("KhachHangID"))
                        {
                            hd.KhachHangID = reader["KhachHangID"] == DBNull.Value
                                ? ""
                                : reader["KhachHangID"].ToString();
                        }

                        list.Add(hd);
                    }
                }
            }

            return list;
        }



        public List<HoaDon> SelectAll()
        {
            string sql = @"
        SELECT hd.HoaDonID, hd.DonHangID, dh.KhachHangID,
               hd.TongTien, hd.NgayThanhToan, hd.PhuongThucThanhToan
        FROM HoaDon hd
        LEFT JOIN DonHang dh ON hd.DonHangID = dh.DonHangID";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(HoaDon hd)
        {
            try
            {
                string sql = "INSERT INTO HoaDon (HoaDonID, DonHangID, TongTien, KhachHangID, NgayThanhToan, PhuongThucThanhToan) " +
                             "VALUES (@0, @1, @2, @3, @4, @5)";
                List<object> args = new List<object>
                {
                    hd.HoaDonID,
                    hd.DonHangID,
                    hd.TongTien,
                    hd.KhachHangID,
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
                string sql = "UPDATE HoaDon " +
                             "SET DonHangID = @1, TongTien = @2, KhachHangID = @3, NgayThanhToan = @4, PhuongThucThanhToan = @5 " +
                             "WHERE HoaDonID = @0";
                List<object> args = new List<object>
                {
                    hd.HoaDonID,
                    hd.DonHangID,
                    hd.TongTien,
                    hd.KhachHangID,
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
