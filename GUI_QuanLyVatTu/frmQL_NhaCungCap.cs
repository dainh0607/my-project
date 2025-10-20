using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_NhaCungCap : Form
    {
        private BUSNhaCungCap busNCC = new BUSNhaCungCap();

        public frmQL_NhaCungCap()
        {
            InitializeComponent();
            txtMaNCC.Text = busNCC.GenerateID();
            LoadData();
        }

        private void LoadData()
        {
            dgvNhaCungCap.DataSource = null;
            List<NhaCungCap> list = busNCC.GetAll() ?? new List<NhaCungCap>();
            dgvNhaCungCap.DataSource = list;
            dgvNhaCungCap.ClearSelection();
            ClearTexts();
        }

        private void ClearTexts()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            txtGhiChu.Text = "";
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            NhaCungCap ncc = new NhaCungCap
            {
                NhaCungCapID = busNCC.GenerateID(),
                TenNhaCungCap = txtTenNCC.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                NgayTao = DateTime.Now,
                GhiChu = txtGhiChu.Text.Trim()
            };

            string result = busNCC.Add(ncc);
            if (result == null)
            {
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                txtMaNCC.Text = busNCC.GenerateID();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NhaCungCap ncc = new NhaCungCap
            {
                NhaCungCapID = txtMaNCC.Text,
                TenNhaCungCap = txtTenNCC.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                NgayTao = DateTime.Now,
                GhiChu = txtGhiChu.Text.Trim()
            };

            string result = busNCC.Update(ncc);
            if (result == null)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string result = busNCC.Delete(txtMaNCC.Text);
                if (result == null)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BUSNhaCungCap bus = new BUSNhaCungCap();
            List<NhaCungCap> danhSachNhanVien = bus.GetAll();
            var ketQua = danhSachNhanVien
                .Where(nv =>
                    (!string.IsNullOrEmpty(nv.NhaCungCapID) && nv.NhaCungCapID.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(nv.TenNhaCungCap) && nv.TenNhaCungCap.ToLower().Contains(keyword))
                ).ToList();

            if (ketQua.Count > 0)
            {
                dgvNhaCungCap.DataSource = ketQua;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên nào phù hợp!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["NhaCungCapID"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNhaCungCap"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();
            }
        }
    }
}
