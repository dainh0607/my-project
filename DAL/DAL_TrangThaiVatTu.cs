using DTO_QuanLyVatTu;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_QuanLyVatTu;

namespace DAL_QuanLyVatTu
{
    public class DAL_TrangThaiVatTu
    {
        public List<TrangThaiVatTu> SelectBySql(string sql, List<object> args)
        {
            List<TrangThaiVatTu> list = new List<TrangThaiVatTu>();
            SqlDataReader reader = DBUtil.Query(sql, args);

            while (reader.Read())
            {
                TrangThaiVatTu ttv = new TrangThaiVatTu
                {
                    TrangThaiID = reader["TrangThaiID"].ToString(),
                    TenTrangThai = reader["TenTrangThai"].ToString()
                };
                list.Add(ttv);
            }

            return list;
        }

        public List<TrangThaiVatTu> SelectAll()
        {
            string sql = "SELECT * FROM TrangThaiVatTu";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(TrangThaiVatTu ttv)
        {
            try
            {
                string sql = "INSERT INTO TrangThaiVatTu (TrangThaiID, TenTrangThai) VALUES (@0, @1)";
                List<object> args = new List<object> { ttv.TrangThaiID, ttv.TenTrangThai };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(TrangThaiVatTu ttv)
        {
            try
            {
                string sql = "UPDATE TrangThaiVatTu SET TenTrangThai = @1 WHERE TrangThaiID = @0";
                List<object> args = new List<object> { ttv.TrangThaiID, ttv.TenTrangThai };
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
                string sql = "DELETE FROM TrangThaiVatTu WHERE TrangThaiID = @0";
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
            string prefix = "TT";
            string sql = "SELECT TOP 1 TrangThaiID FROM TrangThaiVatTu WHERE TrangThaiID LIKE 'TT%' ORDER BY TrangThaiID DESC";
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
