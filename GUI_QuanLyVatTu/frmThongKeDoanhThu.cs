using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using Guna.Charts.WinForms;
using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace GUI_QuanLyVatTu
{
    public partial class frmThongKeDoanhThu : Form
    {
        BUSThongKeDoanhThu busThongKe = new BUSThongKeDoanhThu();
        public frmThongKeDoanhThu()
        {
            InitializeComponent();
            LoadChartDoanhThuTheoThang();
        }

        private void LoadData()
        {
            dgvThongKe.DataSource = busThongKe.SelectAll();
            DinhDangBang();
        }


        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComboBoxData();
                LoadData();
                CapNhatThongKe();

                // Gắn sự kiện khi chọn combobox
                cboTrangThai.SelectionChangeCommitted += cboTrangThai_SelectionChangeCommitted;
                cboPhuongThucThanhToan.SelectionChangeCommitted += cboPhuongThucThanhToan_SelectionChangeCommitted;
                cboNhanVien.SelectionChangeCommitted += cboNhanVien_SelectionChangeCommitted;
                cboKhachHang.SelectionChangeCommitted += cboKhachHang_SelectionChangeCommitted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadComboBoxData()
        {
            cboNhanVien.DataSource = busThongKe.GetNhanVienList();
            cboNhanVien.DisplayMember = "HoTen";
            cboNhanVien.ValueMember = "NhanVienID";
            cboNhanVien.SelectedIndex = -1;

            cboKhachHang.DataSource = busThongKe.GetKhachHangList();
            cboKhachHang.DisplayMember = "HoTen";
            cboKhachHang.ValueMember = "KhachHangID";
            cboKhachHang.SelectedIndex = -1;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đã thanh toán", "Chưa thanh toán", "Đang xử lý" });
            cboTrangThai.SelectedIndex = -1;

            cboPhuongThucThanhToan.Items.Clear();
            cboPhuongThucThanhToan.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Quẹt thẻ" });
            cboPhuongThucThanhToan.SelectedIndex = -1;
        }

        private void LoadChartDoanhThuTheoThang()
        {
            try
            {
                var data = busThongKe.SelectAll();

                int year = DateTime.Now.Year;
                var dataTheoNam = data.Where(x => x.NgayDat.Year == year).ToList();

                var doanhThuTheoThang = dataTheoNam
                    .GroupBy(x => x.NgayDat.Month)
                    .Select(g => new
                    {
                        Thang = g.Key,
                        TongDoanhThu = g.Sum(x => x.DonGia)
                    })
                    .OrderBy(x => x.Thang)
                    .ToList();

                gunaChart1.Datasets.Clear();

                // ===== BIỂU ĐỒ CỘT =====
                Guna.Charts.WinForms.GunaBarDataset barDataset = new Guna.Charts.WinForms.GunaBarDataset();
                barDataset.Label = $"Doanh thu năm {year}";
                barDataset.FillColors.Add(Color.FromArgb(94, 148, 255)); // màu cột
                barDataset.BorderColors.Add(Color.FromArgb(64, 64, 64)); // viền cột
                barDataset.BorderWidth = 1;

                // Gán dữ liệu 12 tháng
                for (int month = 1; month <= 12; month++)
                {
                    var doanhThu = doanhThuTheoThang
                        .FirstOrDefault(x => x.Thang == month)?.TongDoanhThu ?? 0;
                    barDataset.DataPoints.Add($"Tháng {month}", (double)doanhThu);
                }

                // Cấu hình chart
                gunaChart1.YAxes.GridLines.Display = true;
                gunaChart1.XAxes.GridLines.Display = false;
                gunaChart1.Legend.Display = true;
                gunaChart1.Title.Text = "Biểu đồ doanh thu theo tháng";
                gunaChart1.Title.ForeColor = Color.Black;
                gunaChart1.BackColor = Color.White;

                // Gắn dataset và cập nhật chart
                gunaChart1.Datasets.Add(barDataset);
                gunaChart1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DinhDangBang()
        {
            if (dgvThongKe.Columns.Count == 0) return;

            dgvThongKe.Columns["DonHangID"].HeaderText = "Mã Đơn Hàng";
            dgvThongKe.Columns["ChiTietDonHangID"].HeaderText = "Mã Chi Tiết";
            dgvThongKe.Columns["KhachHangID"].HeaderText = "Mã Khách Hàng";
            dgvThongKe.Columns["NhanVienID"].HeaderText = "Mã Nhân Viên";
            dgvThongKe.Columns["NgayDat"].HeaderText = "Ngày Đặt";
            dgvThongKe.Columns["DonGia"].HeaderText = "Đơn Giá (VNĐ)";
            dgvThongKe.Columns["PhuongThucThanhToan"].HeaderText = "Phương Thức TT";
            dgvThongKe.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dgvThongKe.Columns["GhiChu"].HeaderText = "Ghi Chú";

            dgvThongKe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongKe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ThucHienThongKe()
        {
            try
            {
                DateTime fromDate = dtpNgayBatDau.Value.Date;
                DateTime toDate = dtpNgayKetThuc.Value.Date;

                if (fromDate > toDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!");
                    return;
                }

                string nhanVienID = cboNhanVien.SelectedIndex >= 0 ? cboNhanVien.SelectedValue.ToString() : null;
                string khachHangID = cboKhachHang.SelectedIndex >= 0 ? cboKhachHang.SelectedValue.ToString() : null;
                string trangThai = cboTrangThai.Text;
                string phuongThuc = cboPhuongThucThanhToan.Text;

                var data = busThongKe.ThongKeTheoDieuKien(fromDate, toDate, nhanVienID, khachHangID, trangThai, phuongThuc);
                dgvThongKe.DataSource = data;
                DinhDangBang();
                CapNhatThongKe();
                LoadChartDoanhThuTheoThang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message);
            }
        }


        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ThucHienThongKe();
        }

        private void CapNhatThongKe()
        {
            try
            {
                decimal tongDoanhThu = 0;
                int tongDonHang = 0;
                int daThanhToan = 0;

                foreach (DataGridViewRow row in dgvThongKe.Rows)
                {
                    if (row.DataBoundItem is ThongKeDoanhThu tk)
                    {
                        tongDonHang++;
                        tongDoanhThu += tk.DonGia;
                        if (tk.TrangThai == "Đã thanh toán")
                            daThanhToan++;
                    }
                }

                lblTongDoanhThu.Text = $"Tổng doanh thu: {tongDoanhThu:N0} VNĐ";
                lblTongSoDon.Text = $"Tổng số đơn hàng: {tongDonHang}";
                lblDaThanhToan.Text = $"Đã thanh toán: {daThanhToan}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thống kê: " + ex.Message);
            }
        }

        private void dgvThongKe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgvThongKe.DataSource = busThongKe.SelectAll();

                if (dgvThongKe.Columns.Count > 0)
                {
                    dgvThongKe.Columns["DonHangID"].HeaderText = "Mã đơn hàng";
                    dgvThongKe.Columns["ChiTietDonHangID"].HeaderText = "Mã chi tiết đơn hàng";
                    dgvThongKe.Columns["KhachHangID"].HeaderText = "Mã khách hàng";
                    dgvThongKe.Columns["NhanVienID"].HeaderText = "Mã nhân viên";
                    dgvThongKe.Columns["NgayDat"].HeaderText = "Ngày đặt";
                    dgvThongKe.Columns["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                    dgvThongKe.Columns["PhuongThucThanhToan"].HeaderText = "Phương thức thanh toán";
                    dgvThongKe.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvThongKe.Columns["GhiChu"].HeaderText = "Ghi chú";
                }

                dgvThongKe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvThongKe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                CapNhatThongKe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvThongKe_SelectionChanged(object sender, EventArgs e)
        {
            CapNhatThongKe();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpNgayBatDau.Value = DateTime.Now.AddMonths(-1);
            dtpNgayKetThuc.Value = DateTime.Now;
            cboNhanVien.SelectedIndex = -1;
            cboKhachHang.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            cboPhuongThucThanhToan.SelectedIndex = -1;
            LoadData();
            CapNhatThongKe();
        }

        private void cboTrangThai_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ThucHienThongKe();
        }

        private void cboPhuongThucThanhToan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ThucHienThongKe();
        }

        private void cboNhanVien_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ThucHienThongKe();
        }

        private void cboKhachHang_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ThucHienThongKe();
        }
    }
}