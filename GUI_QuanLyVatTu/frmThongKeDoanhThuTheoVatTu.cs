using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace GUI_QuanLyVatTu
{
    public partial class frmThongKeDoanhThuTheoVatTu : Form
    {
        private BUS_ThongKeDoanhThuTheoVatTu busTKDT = new BUS_ThongKeDoanhThuTheoVatTu();

        public frmThongKeDoanhThuTheoVatTu()
        {
            InitializeComponent();
            this.Load += frmThongKeDoanhThu_Load;
        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            LoadLoaiVatTu();
            LoadDoanhThu();
            LamMoi();
        }

        private void LoadLoaiVatTu()
        {
            // Giả sử bạn có hàm lấy danh sách loại vật tư từ BUS
            var dsLoai = busTKDT.GetAllLoaiVatTu(); // Trả về List<string> hoặc List<LoaiVatTu>
            cboLoaiVatTu.DataSource = dsLoai;
            cboLoaiVatTu.DisplayMember = "TenLoaiVatTu";
            cboLoaiVatTu.ValueMember = "LoaiVatTuID";
        }

        private void LoadDoanhThu()
        {
            dgvDoanhThu.DataSource = null;
            dgvDoanhThu.DataSource = busTKDT.GetAll();
        }

        private void LamMoi()
        {
            cboLoaiVatTu.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;
            txtTongVatTu.Clear();
            txtTongNhap.Clear();
            txtTongXuat.Clear();
            txtTonKho.Clear();
            txtTongXuat.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadDoanhThu();
                return;
            }
            var all = busTKDT.GetAll();
            var result = all.Where(x =>
                (x.GhiChu != null && x.GhiChu.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (x.LoaiVatTuID != null && x.LoaiVatTuID.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
            dgvDoanhThu.DataSource = null;
            dgvDoanhThu.DataSource = result;
        }

        private void btnThemBieuDo_Click(object sender, EventArgs e)
        {
            var tk = new ThongKeDoanhThuTheoVatTu
            {
                LoaiVatTuID = cboLoaiVatTu.SelectedValue.ToString(),
                TuNgay = dtpTuNgay.Value,
                DenNgay = dtpDenNgay.Value,
                TongVatTu = int.TryParse(txtTongVatTu.Text, out var tongvt) ? tongvt : 0,
                TongNhap = int.TryParse(txtTongNhap.Text, out var tongnhap) ? tongnhap : 0,
                TongXuat = int.TryParse(txtTongXuat.Text, out var tongxuat) ? tongxuat : 0,
                TonKho = int.TryParse(txtTonKho.Text, out var tonkho) ? tonkho : 0,
                GhiChu = txtTongXuat.Text
            };
            string result = busTKDT.Add(tk);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Thêm thành công!");
                LoadDoanhThu();
                LamMoi();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void btnXuat_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var sb = new StringBuilder();
                // Header
                for (int i = 0; i < dgvDoanhThu.Columns.Count; i++)
                    sb.Append(dgvDoanhThu.Columns[i].HeaderText + (i == dgvDoanhThu.Columns.Count - 1 ? "\n" : ","));
                // Rows
                foreach (DataGridViewRow row in dgvDoanhThu.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < dgvDoanhThu.Columns.Count; i++)
                            sb.Append(row.Cells[i].Value?.ToString() + (i == dgvDoanhThu.Columns.Count - 1 ? "\n" : ","));
                    }
                }
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Xuất file thành công!");
            }
        }

        private void btnIn_Click_1(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDialog.Document = printDocument;
            printDocument.PrintPage += (s, ev) =>
            {
                string printText = "THỐNG KÊ DOANH THU VẬT TƯ\n\n";
                foreach (DataGridViewRow row in dgvDoanhThu.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < dgvDoanhThu.Columns.Count; i++)
                            printText += row.Cells[i].Value?.ToString() + "\t";
                        printText += "\n";
                    }
                }
                ev.Graphics.DrawString(printText, new Font("Arial", 10), Brushes.Black, 10, 10);
            };
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDocument.Print();
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDoanhThu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDoanhThu.Rows[e.RowIndex];
                cboLoaiVatTu.SelectedValue = row.Cells["MaLoaiVatTu"].Value?.ToString();
                dtpTuNgay.Value = row.Cells["TuNgay"].Value != null && DateTime.TryParse(row.Cells["TuNgay"].Value.ToString(), out var tu) ? tu : DateTime.Now;
                dtpDenNgay.Value = row.Cells["DenNgay"].Value != null && DateTime.TryParse(row.Cells["DenNgay"].Value.ToString(), out var den) ? den : DateTime.Now;
                txtTongVatTu.Text = row.Cells["TongVatTu"].Value?.ToString();
                txtTongNhap.Text = row.Cells["TongNhap"].Value?.ToString();
                txtTongXuat.Text = row.Cells["TongXuat"].Value?.ToString();
                txtTonKho.Text = row.Cells["TonKho"].Value?.ToString();
                txtTongXuat.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }
    }
}