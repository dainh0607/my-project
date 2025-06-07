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
using Guna.UI2.WinForms;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_KhachHang : Form
    {
        private BUSKhachHang busKhachHang = new BUSKhachHang();
        public frmQL_KhachHang()
        {
            InitializeComponent();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            KhachHang khachHang = new KhachHang
            {
                KhachHangID = txtMaKhachHang.Text.Trim(),
                HoTen = txtHoTenKhachHang.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                GhiChu = txtGhiChu.Text.Trim(),
                NgayTao = guna2DateTimePicker1.Value
            };

            string result = busKhachHang.Add(khachHang);
            if (result == null)
            {
                MessageBox.Show("Thêm khách hàng thành công!");
                LoadHoaDon();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result);
            }
        }

        private void frmQL_KhachHang_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            ResetForm();
        }
        private void LoadHoaDon()
        {
            dgvKhachHang.DataSource = busKhachHang.GetAll();
        }
        private void TaoMaKhachHang()
        {
            txtMaKhachHang.Text = busKhachHang.GenerateID();
        }
        private void ResetForm()
        {
            TaoMaKhachHang();
            txtMaKhachHang.Clear();
            txtHoTenKhachHang.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtGhiChu.Clear();
            txtTimKiem.Clear();
            guna2DateTimePicker1.Value = DateTime.Now;
            txtMaKhachHang.Focus();
            txtMaKhachHang.Enabled = true;
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtHoTenKhachHang.Text = row.Cells["Hoten"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();
                guna2DateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgayThanhToan"].Value);
            }
            txtMaKhachHang.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            KhachHang khachHang = new KhachHang
            {
                KhachHangID = txtMaKhachHang.Text.Trim(),
                HoTen = txtHoTenKhachHang.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                GhiChu = txtGhiChu.Text.Trim(),
                NgayTao = guna2DateTimePicker1.Value
            };

            string result = busKhachHang.Update(khachHang);
            if (result == null)
            {
                MessageBox.Show("Cập nhật khách hàng thành công!");
                LoadHoaDon();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string id = txtMaKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string result = busKhachHang.Delete(id);
                if (result == null)
                {
                    MessageBox.Show("Xóa khách hàng thành công!");
                    LoadHoaDon();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            BUSKhachHang bus = new BUSKhachHang();
            string keyword = txtMaKhachHang.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                return;
            }

            var ketQua = bus.Search(keyword);
            dgvKhachHang.DataSource = ketQua;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            BUSKhachHang bus = new BUSKhachHang();
            ResetForm();
        }
    }
}
