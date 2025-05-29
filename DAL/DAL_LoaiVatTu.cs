using DAL_QuanLyVatTu;
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
    public class DAL_LoaiVatTu
    {
        public List<LoaiVatTu> SelectBySql(string sql, List<object> args)
        {
            List<LoaiVatTu> list = new List<LoaiVatTu>();
            SqlDataReader reader = DBUtil.Query(sql, args);

            while (reader.Read())
            {
                LoaiVatTu lvt = new LoaiVatTu
                {
                    LoaiVatTuID = reader["LoaiVatTuID"].ToString(),
                    TenLoaiVatTu = reader["TenLoaiVatTu"].ToString(),
                    NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                    GhiChu = reader["GhiChu"].ToString()
                };
                list.Add(lvt);
            }

            return list;
        }

        public List<LoaiVatTu> SelectAll()
        {
            string sql = "SELECT * FROM LoaiVatTu";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(LoaiVatTu lvt)
        {
            try
            {
                string sql = "INSERT INTO LoaiVatTu (LoaiVatTuID, TenLoaiVatTu, NgayTao, GhiChu) VALUES (@0, @1, @2, @3)";
                List<object> args = new List<object>
                {
                    lvt.LoaiVatTuID, lvt.TenLoaiVatTu, lvt.NgayTao, lvt.GhiChu
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(LoaiVatTu lvt)
        {
            try
            {
                string sql = "UPDATE LoaiVatTu SET TenLoaiVatTu = @1, NgayTao = @2, GhiChu = @3 WHERE LoaiVatTuID = @0";
                List<object> args = new List<object>
                {
                    lvt.LoaiVatTuID, lvt.TenLoaiVatTu, lvt.NgayTao, lvt.GhiChu
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
                string sql = "DELETE FROM LoaiVatTu WHERE LoaiVatTuID = @0";
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
            string prefix = "LVT";
            string sql = "SELECT TOP 1 LoaiVatTuID FROM LoaiVatTu WHERE LoaiVatTuID LIKE 'LVT%' ORDER BY LoaiVatTuID DESC";
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


