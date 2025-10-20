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
using UTIL_PolyCafe;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_KhachHang : Form
    {
        private BUSKhachHang bus = new BUSKhachHang();


        public frmQL_KhachHang()
        {
            InitializeComponent();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            KhachHang dh = GetInput();
            string result = bus.Add(dh);

            if (result == null)
            {
                MessageBox.Show("Thêm đơn hàng thành công!", "Thông báo");
                LoadData();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result, "Thông báo lỗi");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BUSKhachHang bus = new BUSKhachHang();
            List<KhachHang> danhSachNhanVien = bus.GetAll();

            var ketQua = danhSachNhanVien
                .Where(nv =>
                    (!string.IsNullOrEmpty(nv.KhachHangID) && nv.KhachHangID.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(nv.HoTen) && nv.HoTen.ToLower().Contains(keyword))
                ).ToList();

            if (ketQua.Count > 0)
            {
                dgvKhachHang.DataSource = ketQua;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên nào phù hợp!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmQL_KhachHang_Load(object sender, EventArgs e)
        {
            txtMaKhachHang.Enabled = false;
            dtpNgay.Enabled = false;
            txtHoTenKhachHang.Enabled = true;

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLamMoi.Enabled = true;

            LoadData();

        }
        private void ResetForm()
        {
            txtMaKhachHang.Text = bus.GenerateID();
            dtpNgay.Value = DateTime.Now;
            txtGhiChu.Clear();
            txtGhiChu.ReadOnly = false;
            txtTimKiem.Clear();
            dgvKhachHang.ClearSelection();
        }
        private void LoadData()
        {
            dgvKhachHang.DataSource = bus.GetAll();
            dgvKhachHang.ClearSelection();

            ResetForm();
        }
        private KhachHang GetInput()
        {
            return new KhachHang
            {
                KhachHangID = txtMaKhachHang.Text.Trim(),
                HoTen = txtHoTenKhachHang.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                NgayTao = dtpNgay.Value,
                GhiChu = txtGhiChu.Text.Trim()
            };
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!txtMaKhachHang.Enabled)
            {
                MessageBox.Show("Không thể thay đổi mã khách hàng đã được tạo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KhachHang kh = GetInput();
            string ketQua = bus.Update(kh);

            if (string.IsNullOrEmpty(ketQua))
            {
                MessageBox.Show(" Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(" Lỗi: " + ketQua, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string id = txtMaKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(id)) return;

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string ketQua = bus.Delete(id);

                if (string.IsNullOrEmpty(ketQua))
                {
                    MessageBox.Show(" Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(" Lỗi: " + ketQua, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadData();
        }

        private void dgvKhachHang_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

           
                txtMaKhachHang.Text = row.Cells["KhachHangID"].Value.ToString();
                txtHoTenKhachHang.Text = row.Cells["HoTen"].Value.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                dtpNgay.Value = Convert.ToDateTime(row.Cells["NgayTao"].Value);
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";         

                LoadData();
            }
        }
    }
}
