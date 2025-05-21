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

                    return nv;
                }
            }
            return null;
        }
        public void update(NhanVien nv)
        {
            string sql = "UPDATE NhanVien SET MatKhau = @0 WHERE MaNhanVien = @1";
            List<object> parameters = new List<object>();
            parameters.Add(nv.MatKhau);
            parameters.Add(nv.MaNhanVien);
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
                    entity.MaNhanVien = reader["MaNhanVien"].ToString(); // Fix: Use ToString() instead of GetString()
                    entity.HoTen = reader["HoTen"].ToString(); // Fix: Use ToString() instead of GetString()
                    entity.Email = reader["Email"].ToString(); // Fix: Use ToString() instead of GetString()
                    entity.MatKhau = reader["MatKhau"].ToString(); // Fix: Use ToString() instead of GetString()
                    entity.VaiTro = bool.Parse(reader["VaiTro"].ToString()); // Fix: Parse boolean values correctly
                    entity.TrangThai = bool.Parse(reader["TrangThai"].ToString()); // Fix: Parse boolean values correctly
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
                string sql = @"UPDATE NhanVien SET HoTen = @1, Email = @2, MatKhau = @3, VaiTro = @4, TrangThai = @5 WHERE MaNhanVien = @0";
                List<object> thamSo = new List<object>();
                thamSo.Add(nv.MaNhanVien);
                thamSo.Add(nv.HoTen);
                thamSo.Add(nv.Email);
                thamSo.Add(nv.MatKhau);
                thamSo.Add(nv.VaiTro);
                thamSo.Add(nv.TrangThai);
                DBUtil.Update(sql, thamSo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Delete(string maNV)
        {
            try
            {
                string sql = "DELETE FROM NhanVien WHERE MaNhanVien = @0";
                List<object> args = new List<object> { maNV };
                DBUtil.Update(sql, args);
                return null; // Xóa thành công, không có lỗi
            }
            catch (Exception ex)
            {
                return ex.Message; // Trả về thông báo lỗi nếu có
            }
        }

        public string generateMaNhanVien()
        {
            string prefix = "NV";
            string sql = "SELECT TOP 1 MaNhanVien FROM NhanVien WHERE MaNhanVien LIKE 'NV%' ORDER BY MaNhanVien DESC";

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
                string sql = "INSERT INTO NhanVien (MaNhanVien, HoTen, Email, MatKhau, VaiTro, TrangThai) VALUES (@0, @1, @2, @3, @4, @5)";
                List<object> args = new List<object> { nv.MaNhanVien, nv.HoTen, nv.Email, nv.MatKhau, nv.VaiTro, nv.TrangThai };
                DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
