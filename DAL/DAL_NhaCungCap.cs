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
    public class DAL_NhaCungCap
    {
        public List<NhaCungCap> SelectBySql(string sql, List<object> args)
        {
            List<NhaCungCap> list = new List<NhaCungCap>();
            SqlDataReader reader = DBUtil.Query(sql, args);
            while (reader.Read())
            {
                NhaCungCap ncc = new NhaCungCap
                {
                    NhaCungCapID = reader["NhaCungCapID"].ToString(),
                    TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
                    SoDienThoai = reader["SoDienThoai"].ToString(),
                    Email = reader["Email"].ToString(),
                    DiaChi = reader["DiaChi"].ToString(),
                    NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                    GhiChu = reader["GhiChu"].ToString()
                };
                list.Add(ncc);
            }
            return list;
        }

        public List<NhaCungCap> SelectAll()
        {
            string sql = "SELECT * FROM NhaCungCap";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(NhaCungCap ncc)
        {
            try
            {
                string sql = "INSERT INTO NhaCungCap (NhaCungCapID, TenNhaCungCap, SoDienThoai, Email, DiaChi, NgayTao, GhiChu) " +
                             "VALUES (@0, @1, @2, @3, @4, @5, @6)";
                List<object> args = new List<object>
                {
                    ncc.NhaCungCapID,
                    ncc.TenNhaCungCap,
                    ncc.SoDienThoai,
                    ncc.Email,
                    ncc.DiaChi,
                    ncc.NgayTao,
                    ncc.GhiChu
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(NhaCungCap ncc)
        {
            try
            {
                string sql = "UPDATE NhaCungCap SET TenNhaCungCap = @1, SoDienThoai = @2, Email = @3, DiaChi = @4, NgayTao = @5, GhiChu = @6 " +
                             "WHERE NhaCungCapID = @0";
                List<object> args = new List<object>
                {
                    ncc.NhaCungCapID,
                    ncc.TenNhaCungCap,
                    ncc.SoDienThoai,
                    ncc.Email,
                    ncc.DiaChi,
                    ncc.NgayTao,
                    ncc.GhiChu
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
                string sql = "DELETE FROM NhaCungCap WHERE NhaCungCapID = @0";
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
            string prefix = "NCC";
            string sql = "SELECT TOP 1 NhaCungCapID FROM NhaCungCap WHERE NhaCungCapID LIKE 'NCC%' ORDER BY NhaCungCapID DESC";
            object result = DBUtil.ScalarQuery(sql, new List<object>());

            if (result != null)
            {
                string currentID = result.ToString();
                string number = currentID.Substring(3);
                if (int.TryParse(number, out int num))
                {
                    return prefix + (num + 1).ToString("D3");
                }
            }

            return prefix + "001";
        }
    }
}
