using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;

namespace DAL_QuanLyVatTu
{
    public class DAL_ThongKeDoanhThu
    {
        public List<ThongKeDoanhThu> SelectAll()
        {
            string sql = @"
                SELECT 
                    dh.DonHangID,
                    ctdh.ChiTietDonHangID,
                    dh.KhachHangID,
                    dh.NhanVienID,
                    dh.NgayDat,
                    ctdh.DonGia,
                    dh.PhuongThucThanhToan,
                    dh.TrangThai,
                    dh.GhiChu
                FROM DonHang dh
                INNER JOIN ChiTietDonHang ctdh ON dh.DonHangID = ctdh.DonHangID
                ORDER BY dh.NgayDat DESC";

            List<ThongKeDoanhThu> list = new List<ThongKeDoanhThu>();

            using (SqlConnection conn = DBUtil.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ThongKeDoanhThu tk = new ThongKeDoanhThu
                        {
                            DonHangID = dr["DonHangID"].ToString(),
                            ChiTietDonHangID = dr["ChiTietDonHangID"].ToString(),
                            KhachHangID = dr["KhachHangID"].ToString(),
                            NhanVienID = dr["NhanVienID"].ToString(),
                            NgayDat = Convert.ToDateTime(dr["NgayDat"]),
                            DonGia = Convert.ToDecimal(dr["DonGia"]),
                            PhuongThucThanhToan = dr["PhuongThucThanhToan"].ToString(),
                            TrangThai = dr["TrangThai"].ToString(),
                            GhiChu = dr["GhiChu"].ToString()
                        };
                        list.Add(tk);
                    }
                }
            }

            return list;
        }

        public List<ThongKeDoanhThu> SelectByFilter(DateTime fromDate, DateTime toDate, string nhanVienID, string khachHangID, string trangThai, string phuongThuc)
        {
            string sql = @"
                SELECT 
                    dh.DonHangID,
                    ctdh.ChiTietDonHangID,
                    dh.KhachHangID,
                    dh.NhanVienID,
                    dh.NgayDat,
                    ctdh.DonGia,
                    dh.PhuongThucThanhToan,
                    dh.TrangThai,
                    dh.GhiChu
                FROM DonHang dh
                INNER JOIN ChiTietDonHang ctdh ON dh.DonHangID = ctdh.DonHangID
                WHERE dh.NgayDat BETWEEN @FromDate AND @ToDate";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            };

            if (!string.IsNullOrEmpty(nhanVienID))
            {
                sql += " AND dh.NhanVienID = @NhanVienID";
                parameters.Add(new SqlParameter("@NhanVienID", nhanVienID));
            }
            if (!string.IsNullOrEmpty(khachHangID))
            {
                sql += " AND dh.KhachHangID = @KhachHangID";
                parameters.Add(new SqlParameter("@KhachHangID", khachHangID));
            }
            if (!string.IsNullOrEmpty(trangThai))
            {
                sql += " AND dh.TrangThai = @TrangThai";
                parameters.Add(new SqlParameter("@TrangThai", trangThai));
            }
            if (!string.IsNullOrEmpty(phuongThuc))
            {
                sql += " AND dh.PhuongThucThanhToan = @PhuongThuc";
                parameters.Add(new SqlParameter("@PhuongThuc", phuongThuc));
            }

            sql += " ORDER BY dh.NgayDat DESC";

            List<ThongKeDoanhThu> list = new List<ThongKeDoanhThu>();

            using (SqlConnection conn = DBUtil.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ThongKeDoanhThu tk = new ThongKeDoanhThu
                            {
                                DonHangID = dr["DonHangID"].ToString(),
                                ChiTietDonHangID = dr["ChiTietDonHangID"].ToString(),
                                KhachHangID = dr["KhachHangID"].ToString(),
                                NhanVienID = dr["NhanVienID"].ToString(),
                                NgayDat = Convert.ToDateTime(dr["NgayDat"]),
                                DonGia = Convert.ToDecimal(dr["DonGia"]),
                                PhuongThucThanhToan = dr["PhuongThucThanhToan"].ToString(),
                                TrangThai = dr["TrangThai"].ToString(),
                                GhiChu = dr["GhiChu"].ToString()
                            };
                            list.Add(tk);
                        }
                    }
                }
            }

            return list;
        }
    }
}
