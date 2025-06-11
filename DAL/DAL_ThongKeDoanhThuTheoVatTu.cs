using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_QuanLyVatTu;

namespace DAL_QuanLyVatTu
{
    public class DAL_ThongKeDoanhThuTheoVatTu
    {
        public List<ThongKeDoanhThuTheoVatTu> SelectAll()
        {
            string sql = "SELECT * FROM ThongKeDoanhThu";
            var list = new List<ThongKeDoanhThuTheoVatTu>();
            using (var reader = DAL_PolyCafe.DBUtil.Query(sql, new List<object>()))
            {
                while (reader.Read())
                {
                    var tk = new ThongKeDoanhThuTheoVatTu
                    {
                        MaThongKe = reader["MaThongKe"].ToString(),
                        LoaiVatTuID = reader["MaLoaiVatTu"].ToString(),
                        TuNgay = Convert.ToDateTime(reader["TuNgay"]),
                        DenNgay = Convert.ToDateTime(reader["DenNgay"]),
                        TongVatTu = Convert.ToInt32(reader["TongVatTu"]),
                        TongNhap = Convert.ToInt32(reader["TongNhap"]),
                        TongXuat = Convert.ToInt32(reader["TongXuat"]),
                        TonKho = Convert.ToInt32(reader["TonKho"]),
                        GhiChu = reader["GhiChu"].ToString()
                    };
                    list.Add(tk);
                }
            }
            return list;
        }

        public List<ThongKeDoanhThuTheoVatTu> SelectByLoaiVatTuID(string loaiVatTuID)
        {
            string sql = "SELECT * FROM ThongKeDoanhThu WHERE LoaiVatTuID = @0";
            var list = new List<ThongKeDoanhThuTheoVatTu>();
            using (var reader = DAL_PolyCafe.DBUtil.Query(sql, new List<object> { loaiVatTuID }))
            {
                while (reader.Read())
                {
                    var tk = new ThongKeDoanhThuTheoVatTu
                    {
                        MaThongKe = reader["MaThongKe"].ToString(),
                        LoaiVatTuID = reader["MaLoaiVatTu"].ToString(),
                        TuNgay = Convert.ToDateTime(reader["TuNgay"]),
                        DenNgay = Convert.ToDateTime(reader["DenNgay"]),
                        TongVatTu = Convert.ToInt32(reader["TongVatTu"]),
                        TongNhap = Convert.ToInt32(reader["TongNhap"]),
                        TongXuat = Convert.ToInt32(reader["TongXuat"]),
                        TonKho = Convert.ToInt32(reader["TonKho"]),
                        GhiChu = reader["GhiChu"].ToString()
                    };
                    list.Add(tk);
                }
            }
            return list;
        }

        public string Insert(ThongKeDoanhThuTheoVatTu tk)
        {
            try
            {
                string sql = "INSERT INTO ThongKeDoanhThu (MaThongKe, MaLoaiVatTu, TuNgay, DenNgay, TongVatTu, TongNhap, TongXuat, TonKho, GhiChu) " +
                             "VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
                var args = new List<object>
                {
                    tk.MaThongKe,
                    tk.LoaiVatTuID,
                    tk.TuNgay,
                    tk.DenNgay,
                    tk.TongVatTu,
                    tk.TongNhap,
                    tk.TongXuat,
                    tk.TonKho,
                    tk.GhiChu
                };
                DAL_PolyCafe.DBUtil.Update(sql, args);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Lấy danh sách loại vật tư (giả sử bảng LoaiVatTu)
        public List<LoaiVatTu> GetAllLoaiVatTu()
        {
            string sql = "SELECT LoaiVatTuID, TenLoaiVatTu FROM LoaiVatTu";
            var list = new List<LoaiVatTu>();
            using (var reader = DAL_PolyCafe.DBUtil.Query(sql, new List<object>()))
            {
                while (reader.Read())
                {
                    list.Add(new LoaiVatTu
                    {
                        LoaiVatTuID = reader["LoaiVatTuID"].ToString(),
                        TenLoaiVatTu = reader["TenLoaiVatTu"].ToString()
                    });
                }
            }
            return list;
        }
    }
}
