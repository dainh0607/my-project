using BUS_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_QuanLyVatTu
{
    public partial class frmInHoaDon : Form
    {
        private BUS_InHoaDon busInHoaDon = new BUS_InHoaDon();
        private List<InHoaDon> danhSachHoaDon = new List<InHoaDon>();

        public frmInHoaDon()
        {
            InitializeComponent();
            LoadDanhSachHoaDon();
            LoadTrangThai();
        }

        private void frmInHoaDon_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now.AddDays(-30);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void LoadDanhSachHoaDon()
        {
            try
            {
                danhSachHoaDon = busInHoaDon.LayTatCaHoaDon();
                dgvHoaDon.DataSource = danhSachHoaDon;
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.AddRange(busInHoaDon.LayDanhSachTrangThai().ToArray());
            cboTrangThai.SelectedIndex = 0;
        }

        private void FormatDataGridView()
        {
            dgvHoaDon.AutoGenerateColumns = false;
            dgvHoaDon.Columns.Clear();

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mã HĐ",
                DataPropertyName = "InHoaDonID",
                Width = 80
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mã ĐH",
                DataPropertyName = "DonHangID",
                Width = 80
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mã NV",
                DataPropertyName = "NhanVienID",
                Width = 80
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Ngày In",
                DataPropertyName = "NgayIn",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Tổng Tiền",
                DataPropertyName = "TongTien",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "N0" }
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Trạng Thái",
                DataPropertyName = "TrangThai",
                Width = 80
            });

            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Ghi Chú",
                DataPropertyName = "GhiChu",
                Width = 150
            });
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                DateTime? tuNgay = dtpTuNgay.Value;
                DateTime? denNgay = dtpDenNgay.Value;
                string trangThai = cboTrangThai.SelectedIndex > 0 ? cboTrangThai.SelectedItem.ToString() : null;

                danhSachHoaDon = busInHoaDon.TimKiemHoaDon(keyword, tuNgay, denNgay, trangThai);
                dgvHoaDon.DataSource = danhSachHoaDon;

                lblTongSo.Text = $"Tổng số: {danhSachHoaDon.Count} hóa đơn";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var hoaDon = dgvHoaDon.CurrentRow.DataBoundItem as InHoaDon;
                if (hoaDon != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.FileName = $"HoaDon_{hoaDon.InHoaDonID}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PDFExporter exporter = new PDFExporter();
                        string result = exporter.XuatHoaDonPDF(hoaDon, saveFileDialog.FileName);

                        if (string.IsNullOrEmpty(result))
                        {
                            MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Cập nhật trạng thái thành "Đã in"
                            busInHoaDon.CapNhatTrangThai(hoaDon.InHoaDonID, "Đã in");
                            btnTimKiem.PerformClick(); // Refresh danh sách
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi xuất hóa đơn: " + result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXemTruoc_Click(object sender, EventArgs e)
        {
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            dtpTuNgay.Value = DateTime.Now.AddDays(-30);
            dtpDenNgay.Value = DateTime.Now;
            cboTrangThai.SelectedIndex = 0;
            LoadDanhSachHoaDon();
        }

        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnXemTruoc.PerformClick();
        }
    }
}