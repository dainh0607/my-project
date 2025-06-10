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
        private readonly BUSNhanVien _busNhanVien = new BUSNhanVien();

        public frmQL_NhanVien()
        {
            InitializeComponent();
        }

        private void frmQL_NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            ResetForm();
            LoadChucVu();
            txtMaNhanVien.ReadOnly = true;
        }
        private void LoadChucVu()
        {
            cboChucVu.Items.Clear();
            var ds = _busNhanVien.GetNhanVienList();
            var chucVuList = ds.Select(nv => nv.ChucVu).Distinct().Where(s => !string.IsNullOrWhiteSpace(s));
            foreach (var chucVu in chucVuList)
            {
                cboChucVu.Items.Add(chucVu);
            }
        }

        private void LoadData()
        {
            var ds = _busNhanVien.GetNhanVienList();
            dgvNhanVien.DataSource = null;
            dgvNhanVien.Columns.Clear();
            dgvNhanVien.AutoGenerateColumns = false;

            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NhanVienID",
                Name = "NhanVienID",
                HeaderText = "Mã nhân viên"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                Name = "HoTen",
                HeaderText = "Tên nhân viên"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ChucVu",
                Name = "ChucVu",
                HeaderText = "Chức vụ"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoDienThoai",
                Name = "SoDienThoai",
                HeaderText = "Số điện thoại"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                Name = "Email",
                HeaderText = "Email"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MatKhau",
                Name = "MatKhau",
                HeaderText = "Mật khẩu"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GhiChu",
                Name = "GhiChu",
                HeaderText = "Ghi chú"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VaiTro",
                Name = "VaiTro",
                HeaderText = "Vai trò"
            });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TinhTrang",
                Name = "TinhTrang",
                HeaderText = "Tình trạng"
            });

            dgvNhanVien.DataSource = ds;
        }


        private void ResetForm()
        {
            txtMaNhanVien.Text = _busNhanVien.TaoMaNhanVienTuDong();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtMatKhau.Clear();
            txtGhiChu.Clear();
            cboChucVu.SelectedIndex = -1;
            rdoQuanLy.Checked = false;
            rdoNhanVien.Checked = true;
            rdoHoatDong.Checked = true;
            rdoTamNgung.Checked = false;

            txtHoTen.Focus();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
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
                GhiChu = txtGhiChu.Text.Trim(),
                VaiTro = rdoQuanLy.Checked,
                TinhTrang = rdoHoatDong.Checked
            };
        }

        private bool ValidateNhanVienInput()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboChucVu.Text))
            {
                MessageBox.Show("Vui lòng chọn chức vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboChucVu.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text) || !Regex.IsMatch(txtSDT.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text) || txtMatKhau.Text.Length < 4)
            {
                MessageBox.Show("Vui lòng nhập mật khẩu (tối thiểu 4 ký tự).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateNhanVienInput()) return;

            var nv = GetNhanVienFromForm();
            string result = _busNhanVien.InsertNhanVien(nv);
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

            if (!ValidateNhanVienInput()) return;

            var nv = GetNhanVienFromForm();
            string result = _busNhanVien.UpdateNhanVien(nv);
            MessageBox.Show("Sửa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                string result = _busNhanVien.DeleteNhanVien(txtMaNhanVien.Text);
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                txtMaNhanVien.Text = row.Cells["NhanVienID"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();

                string chucVu = row.Cells["ChucVu"].Value?.ToString();
                if (!string.IsNullOrEmpty(chucVu) && cboChucVu.Items.Contains(chucVu))
                    cboChucVu.SelectedItem = chucVu;
                else
                    cboChucVu.SelectedIndex = -1;

                // Gán Vai trò (bool): true = Quản lý, false = Nhân viên
                bool vaiTro = false;
                bool.TryParse(row.Cells["VaiTro"].Value?.ToString(), out vaiTro);
                rdoQuanLy.Checked = vaiTro;
                rdoNhanVien.Checked = !vaiTro;

                // Gán Tình trạng (bool): true = Hoạt động, false = Tạm dừng
                bool tinhTrang = false;
                bool.TryParse(row.Cells["TinhTrang"].Value?.ToString(), out tinhTrang);
                rdoHoatDong.Checked = tinhTrang;
                rdoTamNgung.Checked = !tinhTrang;

                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtGhiChu_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel12_Click(object sender, EventArgs e)
        {

        }
    }
}
