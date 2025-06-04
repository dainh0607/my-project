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
    public class DAL_NhanVien
    {
        // Fix for CS0103: Declare and initialize 'dalNhanVien'  
        private static DAL_NhanVien dalNhanVien = new DAL_NhanVien();

        public NhanVien getNhanVien1(string email, string password)
        {
            string sql = "SELECT Top 1 * FROM NhanVien WHERE Email=@0 AND MatKhau=@1";
            List<object> thamSo = new List<object>();
            thamSo.Add(email);
            thamSo.Add(password);
            SqlDataReader reader = DBUtil.Query(sql, thamSo);
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    NhanVien nv = new NhanVien();
                    nv.NhanVienID = reader["NhanVienID"].ToString();
                    nv.HoTen = reader["HoTen"].ToString();
                    nv.ChucVu = reader["ChucVu"].ToString();
                    nv.SoDienThoai = reader["SoDienThoai"].ToString();
                    nv.GhiChu = reader["GhiChu"].ToString();
                    nv.VaiTro = bool.Parse(reader["VaiTro"].ToString());
                    nv.TinhTrang = bool.Parse(reader["TinhTrang"].ToString());
                    nv.Email = reader["Email"].ToString();
                    nv.MatKhau = reader["MatKhau"].ToString();

                    return nv;
                }
            }
            return null;
        }

        public void update(NhanVien nv)
        {
            string sql = "UPDATE NhanVien SET MatKhau = @0 WHERE NhanVienID = @1";
            List<object> parameters = new List<object>();
            parameters.Add(nv.MatKhau);
            parameters.Add(nv.NhanVienID);
            DBUtil.Update(sql, parameters);
        }

        public List<NhanVien> SelectBySql(string sql, List<object> args, CommandType cmdType = CommandType.Text)
        {
            List<NhanVien> list = new List<NhanVien>();
            try
            {
                SqlDataReader reader = DBUtil.Query(sql, args);
                while (reader.Read())
                {
                    NhanVien entity = new NhanVien();
                    entity.NhanVienID = reader["NhanVienID"].ToString();
                    entity.HoTen = reader["HoTen"].ToString();
                    entity.ChucVu = reader["ChucVu"].ToString();
                    entity.SoDienThoai = reader["SoDienThoai"].ToString();
                    entity.GhiChu = reader["GhiChu"].ToString();
                    entity.VaiTro = bool.Parse(reader["VaiTro"].ToString());
                    entity.TinhTrang = bool.Parse(reader["TinhTrang"].ToString());
                    entity.Email = reader["Email"].ToString();
                    entity.MatKhau = reader["MatKhau"].ToString();
                    list.Add(entity);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }

        public List<NhanVien> selectAll()
        {
            String sql = "SELECT * FROM NhanVien";
            return SelectBySql(sql, new List<object>());
        }

        public void updateNhanVien(NhanVien nv)
        {
            try
            {
                string sql = @"UPDATE NhanVien SET HoTen = @1, ChucVu = @2, SoDienThoai = @3, GhiChu = @4, VaiTro = @5, TinhTrang = @6, Email = @7, MatKhau = @8 WHERE NhanVienID = @0";
                List<object> thamSo = new List<object>();
                thamSo.Add(nv.NhanVienID);
                thamSo.Add(nv.HoTen);
                thamSo.Add(nv.ChucVu);
                thamSo.Add(nv.SoDienThoai);
                thamSo.Add(nv.GhiChu);
                thamSo.Add(nv.VaiTro);
                thamSo.Add(nv.TinhTrang);
                thamSo.Add(nv.Email);
                thamSo.Add(nv.MatKhau);
                DBUtil.Update(sql, thamSo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(string maNV)
        {
            try
            {
                string sql = "DELETE FROM NhanVien WHERE NhanVienID = @0";
                List<object> args = new List<object> { maNV };
                DBUtil.Update(sql, args);
                return "Xóa nhân viên thành công.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa: " + ex.Message;
            }
        }

        public string generateMaNhanVien()
        {
            string prefix = "NV";
            string sql = "SELECT TOP 1 NhanVienID FROM NhanVien WHERE NhanVienID LIKE 'NV%' ORDER BY NhanVienID DESC";

            object result = DBUtil.ScalarQuery(sql, new List<object>());

            if (result != null)
            {
                string maCu = result.ToString(); // Ví dụ: NV005  
                string so = maCu.Substring(2);   // Lấy "005"  
                if (int.TryParse(so, out int num))
                {
                    return prefix + (num + 1).ToString("D3"); // Tăng và định dạng lại thành 3 số  
                }
            }

            return prefix + "001"; // Nếu không có mã nào  
        }

        public string Insert(NhanVien nv)
        {
            try
            {
                string sql = "INSERT INTO NhanVien (NhanVienID, HoTen, ChucVu, SoDienThoai, GhiChu, VaiTro, TinhTrang, Email, MatKhau) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
                List<object> args = new List<object> { nv.NhanVienID, nv.HoTen, nv.ChucVu, nv.SoDienThoai, nv.GhiChu, nv.VaiTro, nv.TinhTrang, nv.Email, nv.MatKhau };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<NhanVien> SearchNhanVien(string keyword)
        {
            string sql = @"SELECT * FROM NhanVien WHERE   
           NhanVienID LIKE @kw OR  
           HoTen LIKE @kw OR  
           ChucVu LIKE @kw OR  
           SoDienThoai LIKE @kw OR  
           GhiChu LIKE @kw";
            var args = new List<object> { "%" + keyword + "%" };
            return dalNhanVien.SelectBySql(sql, args);
        }

    }
}
