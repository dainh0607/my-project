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
    public class DAL_InHoaDon
    {
        // Lấy list InHoaDon từ SQL
        public List<InHoaDon> SelectBySql(string sql, List<object> args, CommandType cmdType = CommandType.Text)
        {
            List<InHoaDon> list = new List<InHoaDon>();

            using (SqlCommand cmd = DBUtil.GetCommand(sql, args, cmdType))
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var columnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        foreach (DataRow r in schemaTable.Rows)
                        {
                            columnNames.Add(r["ColumnName"].ToString());
                        }
                    }

                    while (reader.Read())
                    {
                        InHoaDon hd = new InHoaDon();

                        if (columnNames.Contains("InHoaDonID") && !reader.IsDBNull(reader.GetOrdinal("InHoaDonID")))
                            hd.InHoaDonID = reader["InHoaDonID"].ToString();

                        if (columnNames.Contains("DonHangID") && !reader.IsDBNull(reader.GetOrdinal("DonHangID")))
                            hd.DonHangID = reader["DonHangID"].ToString();

                        if (columnNames.Contains("NhanVienID") && !reader.IsDBNull(reader.GetOrdinal("NhanVienID")))
                            hd.NhanVienID = reader["NhanVienID"].ToString();

                        if (columnNames.Contains("NgayIn") && !reader.IsDBNull(reader.GetOrdinal("NgayIn")))
                            hd.NgayIn = Convert.ToDateTime(reader["NgayIn"]);
                        else if (columnNames.Contains("Ngayln") && !reader.IsDBNull(reader.GetOrdinal("Ngayln")))
                            hd.NgayIn = Convert.ToDateTime(reader["Ngayln"]);

                        if (columnNames.Contains("TongTien") && !reader.IsDBNull(reader.GetOrdinal("TongTien")))
                            hd.TongTien = Convert.ToDecimal(reader["TongTien"]);

                        if (columnNames.Contains("TrangThai") && !reader.IsDBNull(reader.GetOrdinal("TrangThai")))
                            hd.TrangThai = reader["TrangThai"].ToString();

                        if (columnNames.Contains("GhiChu") && !reader.IsDBNull(reader.GetOrdinal("GhiChu")))
                            hd.GhiChu = reader["GhiChu"].ToString();

                        list.Add(hd);
                    }
                }
            }

            return list;
        }

        public List<InHoaDon> SelectAll()
        {
            string sql = @"
                SELECT InHoaDonID, DonHangID, NhanVienID, NgayIn, TongTien, TrangThai, GhiChu
                FROM InHoaDon
                ORDER BY NgayIn DESC";
            return SelectBySql(sql, new List<object>());
        }

        public InHoaDon SelectByID(string id)
        {
            string sql = "SELECT * FROM InHoaDon WHERE InHoaDonID = @0";
            List<object> args = new List<object> { id };
            var result = SelectBySql(sql, args);
            return result.Count > 0 ? result[0] : null;
        }

        public List<InHoaDon> SelectByDonHangID(string donHangID)
        {
            string sql = "SELECT * FROM InHoaDon WHERE DonHangID = @0 ORDER BY NgayIn DESC";
            List<object> args = new List<object> { donHangID };
            return SelectBySql(sql, args);
        }

        // Thêm mới hóa đơn in
        public string Insert(InHoaDon hd)
        {
            try
            {
                string sql = "INSERT INTO InHoaDon (InHoaDonID, DonHangID, NhanVienID, NgayIn, TongTien, TrangThai, GhiChu) " +
                             "VALUES (@0,@1,@2,@3,@4,@5,@6)";
                List<object> args = new List<object>
                {
                    hd.InHoaDonID ?? string.Empty,
                    hd.DonHangID ?? string.Empty,
                    hd.NhanVienID ?? string.Empty,
                    hd.NgayIn == DateTime.MinValue ? (object)DBNull.Value : hd.NgayIn,
                    hd.TongTien,
                    hd.TrangThai ?? string.Empty,
                    hd.GhiChu ?? string.Empty
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Cập nhật hóa đơn in
        public string Update(InHoaDon hd)
        {
            try
            {
                string sql = "UPDATE InHoaDon SET DonHangID=@1, NhanVienID=@2, NgayIn=@3, TongTien=@4, TrangThai=@5, GhiChu=@6 WHERE InHoaDonID=@0";
                List<object> args = new List<object>
                {
                    hd.InHoaDonID ?? string.Empty,
                    hd.DonHangID ?? string.Empty,
                    hd.NhanVienID ?? string.Empty,
                    hd.NgayIn == DateTime.MinValue ? (object)DBNull.Value : hd.NgayIn,
                    hd.TongTien,
                    hd.TrangThai ?? string.Empty,
                    hd.GhiChu ?? string.Empty
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Xóa hóa đơn in
        public string Delete(string id)
        {
            try
            {
                string sql = "DELETE FROM InHoaDon WHERE InHoaDonID = @0";
                DBUtil.Update(sql, new List<object> { id });
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Sinh ID mới dạng "IHD001"
        public string GenerateID()
        {
            string prefix = "IHD";
            string sql = "SELECT TOP 1 InHoaDonID FROM InHoaDon WHERE InHoaDonID LIKE 'IHD%' ORDER BY InHoaDonID DESC";
            object result = DBUtil.ScalarQuery(sql, new List<object>());
            if (result != null)
            {
                string currentID = result.ToString();
                if (currentID.Length > 3)
                {
                    string number = currentID.Substring(3);
                    if (int.TryParse(number, out int num))
                    {
                        return prefix + (num + 1).ToString("D3");
                    }
                }
            }
            return prefix + "001";
        }

        // Tìm kiếm hóa đơn in theo các tiêu chí
        public List<InHoaDon> Search(string keyword, DateTime? fromDate, DateTime? toDate, string trangThai)
        {
            string sql = @"SELECT * FROM InHoaDon WHERE 1=1";
            List<object> args = new List<object>();
            int paramIndex = 0;

            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND (InHoaDonID LIKE @" + paramIndex + " OR DonHangID LIKE @" + paramIndex + " OR GhiChu LIKE @" + paramIndex + ")";
                args.Add("%" + keyword + "%");
                paramIndex++;
            }

            if (fromDate.HasValue)
            {
                sql += " AND NgayIn >= @" + paramIndex;
                args.Add(fromDate.Value);
                paramIndex++;
            }

            if (toDate.HasValue)
            {
                sql += " AND NgayIn <= @" + paramIndex;
                args.Add(toDate.Value.AddDays(1).AddSeconds(-1)); // Đến hết ngày
                paramIndex++;
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                sql += " AND TrangThai = @" + paramIndex;
                args.Add(trangThai);
                paramIndex++;
            }

            sql += " ORDER BY NgayIn DESC";

            return SelectBySql(sql, args);
        }

        // Cập nhật trạng thái hóa đơn
        public string UpdateTrangThai(string inHoaDonID, string trangThai)
        {
            try
            {
                string sql = "UPDATE InHoaDon SET TrangThai = @1 WHERE InHoaDonID = @0";
                List<object> args = new List<object> { inHoaDonID, trangThai };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Lấy tổng số hóa đơn theo trạng thái
        public int CountByTrangThai(string trangThai)
        {
            string sql = "SELECT COUNT(*) FROM InHoaDon WHERE TrangThai = @0";
            List<object> args = new List<object> { trangThai };
            object result = DBUtil.ScalarQuery(sql, args);
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}

