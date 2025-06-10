using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_NhaCungCap : Form
    {
        private BUSNhaCungCap busNCC = new BUSNhaCungCap();

        public frmQL_NhaCungCap()
        {
            InitializeComponent();
            this.Load += frmQL_NhaCungCap_Load;
        }

        private void frmQL_NhaCungCap_Load(object sender, EventArgs e)
        {
            LoadNhaCungCap();
            LamMoi();
        }

        private void LoadNhaCungCap()
        {
            dgvNhaCungCap.DataSource = null;
            dgvNhaCungCap.DataSource = busNCC.GetAll();
        }

        private void LamMoi()
        {
            txtNhaCungCapID.Text = busNCC.GenerateID();
            txtTenNhaCungCap.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtGhiChu.Clear();
            dtpNgayTao.Value = DateTime.Now;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var ncc = new NhaCungCap
            {
                NhaCungCapID = txtNhaCungCapID.Text,
                TenNhaCungCap = txtTenNhaCungCap.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                DiaChi = txtDiaChi.Text,
                NgayTao = dtpNgayTao.Value,
                GhiChu = txtGhiChu.Text
            };
            string result = busNCC.Add(ncc);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Thêm thành công!");
                LoadNhaCungCap();
                LamMoi();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void dgvNhaCungCap_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];

                // Đổ dữ liệu lên các control
                txtNhaCungCapID.Text = row.Cells["NhaCungCapID"].Value?.ToString();
                txtTenNhaCungCap.Text = row.Cells["TenNhaCungCap"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();

                // Xử lý ngày tạo (nếu có)
                if (row.Cells["NgayTao"].Value != null && DateTime.TryParse(row.Cells["NgayTao"].Value.ToString(), out DateTime ngayTao))
                {
                    dtpNgayTao.Value = ngayTao;
                }
                else
                {
                    dtpNgayTao.Value = DateTime.Now;
                }
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            var ncc = new NhaCungCap
            {
                NhaCungCapID = txtNhaCungCapID.Text,
                TenNhaCungCap = txtTenNhaCungCap.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                DiaChi = txtDiaChi.Text,
                NgayTao = dtpNgayTao.Value,
                GhiChu = txtGhiChu.Text
            };
            string result = busNCC.Update(ncc);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Sửa thành công!");
                LoadNhaCungCap();
                LamMoi();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            string id = txtNhaCungCapID.Text;
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string result = busNCC.Delete(id);
                if (string.IsNullOrEmpty(result))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadNhaCungCap();
                    LamMoi();
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            LamMoi();
            LoadNhaCungCap();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadNhaCungCap();
                return;
            }

            // Lấy danh sách NCC
            var allNCC = busNCC.GetAll();

            // Loại bỏ dấu từ khóa tìm kiếm
            string keywordNoSign = RemoveDiacritics(keyword).ToLower();

            // Lọc danh sách theo tên (có dấu hoặc không dấu)
            var result = allNCC.Where(ncc =>
                (!string.IsNullOrEmpty(ncc.TenNhaCungCap) && (
                    ncc.TenNhaCungCap.ToLower().Contains(keyword.ToLower()) ||
                    RemoveDiacritics(ncc.TenNhaCungCap).ToLower().Contains(keywordNoSign)
                ))).ToList();

            dgvNhaCungCap.DataSource = null;
            dgvNhaCungCap.DataSource = result;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
