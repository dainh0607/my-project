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
    public class DAL_VatTu
    {
        public List<VatTu> SelectBySql(string sql, List<object> args)
        {
            List<VatTu> list = new List<VatTu>();
            SqlDataReader reader = DBUtil.Query(sql, args);

            while (reader.Read())
            {
                VatTu vt = new VatTu
                {
                    VatTuID = reader["VatTuID"].ToString(),
                    LoaiVatTuID = reader["LoaiVatTuID"].ToString(),
                    TenVatTu = reader["TenVatTu"].ToString(),
                    DonGia = Convert.ToDecimal(reader["DonGia"]),
                    SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                    NhaCungCapID = reader["NhaCungCapID"].ToString(),
                    NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                    GhiChu = reader["GhiChu"].ToString(),
                    TrangThaiID = reader["TrangThaiID"].ToString()
                };
                list.Add(vt);
            }

            return list;
        }

        public List<VatTu> SelectAll()
        {
            string sql = "SELECT * FROM VatTu";
            return SelectBySql(sql, new List<object>());
        }

        public string Insert(VatTu vt)
        {
            try
            {
                string sql = @"INSERT INTO VatTu (VatTuID, LoaiVatTuID, TenVatTu, DonGia, SoLuongTon, NhaCungCapID, NgayNhap, GhiChu, TrangThaiID)
                               VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
                List<object> args = new List<object>
                {
                    vt.VatTuID, vt.LoaiVatTuID, vt.TenVatTu, vt.DonGia, vt.SoLuongTon,
                    vt.NhaCungCapID, vt.NgayNhap, vt.GhiChu, vt.TrangThaiID
                };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(VatTu vt)
        {
            try
            {
                string sql = @"UPDATE VatTu SET 
                                LoaiVatTuID = @1, TenVatTu = @2, DonGia = @3, SoLuongTon = @4,
                                NhaCungCapID = @5, NgayNhap = @6, GhiChu = @7, TrangThaiID = @8
                                WHERE VatTuID = @0";
                List<object> args = new List<object>
                {
                    vt.VatTuID, vt.LoaiVatTuID, vt.TenVatTu, vt.DonGia, vt.SoLuongTon,
                    vt.NhaCungCapID, vt.NgayNhap, vt.GhiChu, vt.TrangThaiID
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
                string sql = "DELETE FROM VatTu WHERE VatTuID = @0";
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
            string prefix = "VT";
            string sql = "SELECT TOP 1 VatTuID FROM VatTu WHERE VatTuID LIKE 'VT%' ORDER BY VatTuID DESC";
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
