using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_VatTu : Form
    {
        private readonly BUSVatTu _busVatTu = new BUSVatTu();
        private readonly BUSLoaiVatTu _busLoaiVatTu = new BUSLoaiVatTu();
        private readonly BUSNhaCungCap _busNhaCungCap = new BUSNhaCungCap();
        private readonly BUSTrangThaiVatTu _busTrangThai = new BUSTrangThaiVatTu();


        public frmQL_VatTu()
        {
            InitializeComponent();
        }

        

        private void LoadComboBoxData()
        {
            cboLoaiVatTu.DataSource = _busLoaiVatTu.GetAll();
            cboLoaiVatTu.DisplayMember = "TenLoaiVatTu";
            cboLoaiVatTu.ValueMember = "LoaiVatTuID";
            cboLoaiVatTu.SelectedIndex = -1;

            cboNhaCungCap.DataSource = _busNhaCungCap.GetAll();
            cboNhaCungCap.DisplayMember = "TenNhaCungCap";
            cboNhaCungCap.ValueMember = "NhaCungCapID";
            cboNhaCungCap.SelectedIndex = -1;

            cboTrangThai.DataSource = _busTrangThai.GetAll();
            cboTrangThai.DisplayMember = "TenTrangThai";
            cboTrangThai.ValueMember = "TrangThaiID";
            cboTrangThai.SelectedIndex = -1;
        }

        private void LoadData()
        {
            var ds = _busVatTu.GetAll();
            dgvVatTu.AutoGenerateColumns = false;
            dgvVatTu.Columns.Clear();

            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "VatTuID", Name = "VatTuID", HeaderText = "Mã vật tư" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LoaiVatTuID", Name = "LoaiVatTuID", HeaderText = "Mã loại vật tư" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVatTu", Name = "TenVatTu", HeaderText = "Tên vật tư" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGia", Name = "DonGia", HeaderText = "Đơn giá" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongTon", Name = "SoLuongTon", HeaderText = "Số lượng tồn" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NhaCungCapID", Name = "NhaCungCapID", HeaderText = "Nhà cung cấp" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgayTao", Name = "NgayTao", HeaderText = "Ngày nhập" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GhiChu", Name = "GhiChu", HeaderText = "Ghi chú" });
            dgvVatTu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThaiID", Name = "TrangThaiID", HeaderText = "Trạng thái" });

            dgvVatTu.DataSource = ds;
        }



        private void ResetForm()
        {
            txtMaVatTu.Text = _busVatTu.GenerateID();
            txtTenVatTu.Clear();
            txtMaVatTu.Enabled = false;
            cboLoaiVatTu.SelectedIndex = -1;
            txtDonGia.Clear();
            txtSoLuongTon.Clear();
            cboNhaCungCap.SelectedIndex = -1;
            dtpNgayTao.Value = DateTime.Now;
            txtGhiChu.Clear();
            cboTrangThai.SelectedIndex = -1;

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        private VatTu GetVatTuFromForm()
        {
            return new VatTu
            {
                VatTuID = txtMaVatTu.Text.Trim(),
                LoaiVatTuID = cboLoaiVatTu.SelectedValue?.ToString(),
                TenVatTu = txtTenVatTu.Text.Trim(),
                DonGia = decimal.TryParse(txtDonGia.Text, out var dg) ? dg : 0,
                SoLuongTon = int.TryParse(txtSoLuongTon.Text, out var sl) ? sl : 0,
                NhaCungCapID = cboNhaCungCap.SelectedValue?.ToString(),
                NgayTao = dtpNgayTao.Value,
                GhiChu = txtGhiChu.Text.Trim(),
                TrangThaiID = cboTrangThai.SelectedValue?.ToString()
            };
        }
        private bool ValidateVatTuInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenVatTu.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vật tư.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenVatTu.Focus();
                return false;
            }
            if (cboLoaiVatTu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại vật tư.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiVatTu.Focus();
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text, out var donGia) || donGia <= 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return false;
            }
            if (!int.TryParse(txtSoLuongTon.Text, out var soLuong) || soLuong < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số không âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return false;
            }
            if (cboNhaCungCap.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboNhaCungCap.Focus();
                return false;
            }
            if (cboTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }
            return true;
        }

        // Xóa dấu tiếng Việt cho tìm kiếm không dấu
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void txtMaLoaiVatTu_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateVatTuInput()) return;
            var vt = GetVatTuFromForm();
            string result = _busVatTu.Add(vt);
            MessageBox.Show(string.IsNullOrEmpty(result) ? "Thêm vật tư thành công." : result);
            LoadData();
            ResetForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaVatTu.Text))
            {
                MessageBox.Show("Vui lòng chọn vật tư cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateVatTuInput()) return;
            var vt = GetVatTuFromForm();
            string result = _busVatTu.Update(vt);
            MessageBox.Show(string.IsNullOrEmpty(result) ? "Cập nhật vật tư thành công." : result);
            LoadData();
            ResetForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaVatTu.Text))
            {
                MessageBox.Show("Vui lòng chọn vật tư cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xóa mềm: Nếu vật tư có liên kết, chỉ cập nhật trạng thái, không xóa vật lý
            string result = _busVatTu.DeleteVatTu(txtMaVatTu.Text);
            if (result != null && result.Contains("không thể xóa"))
            {
                // Xóa mềm: cập nhật trạng thái về "KHONG_HOAT_DONG" (hoặc trạng thái bạn quy ước)
                var vt = GetVatTuFromForm();
                vt.TrangThaiID = "KHONG_HOAT_DONG";
                _busVatTu.Update(vt);
                MessageBox.Show("Vật tư đã được chuyển sang trạng thái không hoạt động (xóa mềm).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(string.IsNullOrEmpty(result) ? "Xóa vật tư thành công." : result);
            }
            LoadData();
            ResetForm();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
            txtTimKiem.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            var ds = _busVatTu.GetAll();

            // Tìm kiếm không dấu trên tất cả các trường
            var result = ds.Where(vt =>
                RemoveDiacritics(vt.VatTuID ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.LoaiVatTuID ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.TenVatTu ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.NhaCungCapID ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.GhiChu ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.TrangThaiID ?? "").IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.DonGia.ToString()).IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0 ||
                RemoveDiacritics(vt.SoLuongTon.ToString()).IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            dgvVatTu.DataSource = result;
        }

        private void dgvVatTu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVatTu.Rows[e.RowIndex];
                txtMaVatTu.Text = row.Cells["VatTuID"].Value?.ToString();
                cboLoaiVatTu.SelectedValue = row.Cells["LoaiVatTuID"].Value?.ToString();
                txtTenVatTu.Text = row.Cells["TenVatTu"].Value?.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
                txtSoLuongTon.Text = row.Cells["SoLuongTon"].Value?.ToString();
                cboNhaCungCap.SelectedValue = row.Cells["NhaCungCapID"].Value?.ToString();
                dtpNgayTao.Value = DateTime.TryParse(row.Cells["NgayTao"].Value?.ToString(), out var dt) ? dt : DateTime.Now;
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                cboTrangThai.SelectedValue = row.Cells["TrangThaiID"].Value?.ToString();

                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }
        private void frmQL_VatTu_Load_1(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
            LoadComboBoxData();
        }


        private void dgvVatTu_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvVatTu.Rows[e.RowIndex];
                txtMaVatTu.Text = row.Cells["VatTuID"].Value?.ToString();
                txtTenVatTu.Text = row.Cells["TenVatTu"].Value?.ToString();
                cboLoaiVatTu.SelectedValue = row.Cells["LoaiVatTuID"].Value?.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
                txtSoLuongTon.Text = row.Cells["SoLuongTon"].Value?.ToString();
                cboNhaCungCap.SelectedValue = row.Cells["NhaCungCapID"].Value?.ToString();
                dtpNgayTao.Value = DateTime.TryParse(row.Cells["NgayTao"].Value?.ToString(), out var dt) ? dt : DateTime.Now;
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                cboTrangThai.SelectedValue = row.Cells["TrangThaiID"].Value?.ToString();

                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void txtMaVatTu_TextChanged(object sender, EventArgs e)
        {
            txtMaVatTu.Enabled = false;
        }
    }
}
