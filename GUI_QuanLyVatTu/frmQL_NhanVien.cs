using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_NhanVien : Form
    {
        private BUSNhanVien _busNhanVien = new BUSNhanVien();

        public frmQL_NhanVien()
        {
            InitializeComponent();
        }

        private void frmQL_NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadChucVu();
            ResetForm();
            txtMaNhanVien.ReadOnly = true;

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        private void LoadChucVu()
        {
            cboChucVu.Items.Clear();
            var ds = _busNhanVien.GetNhanVienList();
            var chucVuList = ds.Select(nv => nv.ChucVu)
                               .Distinct()
                               .Where(s => !string.IsNullOrWhiteSpace(s));
            foreach (var chucVu in chucVuList)
                cboChucVu.Items.Add(chucVu);
        }

        private void LoadData()
        {
            var ds = _busNhanVien.GetNhanVienList();
            dgvNhanVien.DataSource = null;
            dgvNhanVien.AutoGenerateColumns = true;
            dgvNhanVien.DataSource = ds;
        }


        private void ResetForm()
        {
            txtMaNhanVien.Text = _busNhanVien.TaoMaNhanVienTuDong();
            txtHoTen.Clear();
            cboChucVu.SelectedIndex = -1;
            txtSDT.Clear();
            txtEmail.Clear();
            txtMatKhau.Clear();
            txtGhiChu.Clear();

            rdoQuanLy.Checked = false;
            rdoNhanVien.Checked = true;
            rdoHoatDong.Checked = true;
            rdoTamNgung.Checked = false;

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtHoTen.Focus();
        }

        private NhanVien GetNhanVienFromForm()
        {
            return new NhanVien
            {
                NhanVienID = txtMaNhanVien.Text,
                HoTen = txtHoTen.Text.Trim(),
                ChucVu = cboChucVu.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                VaiTro = rdoQuanLy.Checked,
                TinhTrang = rdoHoatDong.Checked
            };
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var nv = GetNhanVienFromForm();
            string ketQua = _busNhanVien.InsertNhanVien(nv);

            if (!string.IsNullOrEmpty(ketQua))
            {
                MessageBox.Show(ketQua, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ResetForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var nv = GetNhanVienFromForm();
            string ketQua = _busNhanVien.UpdateNhanVien(nv);

            if (!string.IsNullOrEmpty(ketQua))
            {
                MessageBox.Show(ketQua, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ResetForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                string ketQua = _busNhanVien.DeleteNhanVien(txtMaNhanVien.Text);

                if (!string.IsNullOrEmpty(ketQua))
                {
                    MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            var result = string.IsNullOrEmpty(keyword)
                ? _busNhanVien.GetNhanVienList()
                : _busNhanVien.SearchNhanVien(keyword);

            if (result == null || result.Count == 0)
            {
                MessageBox.Show("Không tìm thấy nhân viên phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvNhanVien.DataSource = result;
        }

        private void dgvNhanVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            txtMaNhanVien.Text = row.Cells["NhanVienID"].Value?.ToString() ?? string.Empty;
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? string.Empty;
            txtSDT.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
            txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString() ?? string.Empty;

            string chucVu = row.Cells["ChucVu"].Value?.ToString();
            if (!string.IsNullOrWhiteSpace(chucVu))
            {
                if (cboChucVu.Items.Contains(chucVu))
                    cboChucVu.SelectedItem = chucVu;
                else
                    cboChucVu.SelectedIndex = -1;
            }
            else
            {
                cboChucVu.SelectedIndex = -1;
            }

            bool vaiTro = false;
            bool.TryParse(row.Cells["VaiTro"].Value?.ToString(), out vaiTro);
            rdoQuanLy.Checked = vaiTro;
            rdoNhanVien.Checked = !vaiTro;

            bool tinhTrang = false;
            bool.TryParse(row.Cells["TinhTrang"].Value?.ToString(), out tinhTrang);
            rdoHoatDong.Checked = tinhTrang;
            rdoTamNgung.Checked = !tinhTrang;

            btnThem.Enabled = false;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            btnSua.Enabled = dgvNhanVien.SelectedRows.Count > 0;
            btnXoa.Enabled = dgvNhanVien.SelectedRows.Count > 0;
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
