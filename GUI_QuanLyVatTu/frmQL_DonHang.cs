using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using UTIL_PolyCafe;
using UTIL_QuanLyVatTu;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_DonHang : Form
    {
        private BUSDonHang bus = new BUSDonHang();

        public frmQL_DonHang()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            txtMaDonHang.Text = bus.GenerateID();
            cboMaKhachHang.SelectedIndex = -1;
            cboMaKhachHang.Enabled = true;

            txtMaNhanVien.Text = AuthUtil.user?.NhanVienID ?? "NV001";
            dtpNgayDat.Value = DateTime.Now;

            cboTrangThai.SelectedIndex = cboTrangThai.Items.IndexOf("Chưa thanh toán");
            cboTrangThai.Enabled = true;

            txtGhiChu.Clear();
            txtGhiChu.ReadOnly = false;

            txtTimKiem.Clear();
            dgvDonHang.ClearSelection();
        }

        private void LoadData()
        {
            dgvDonHang.DataSource = bus.GetAll();
            dgvDonHang.ClearSelection();
            
        }

        private DonHang GetInput()
        {
            return new DonHang
            {
                DonHangID = txtMaDonHang.Text.Trim(),
                KhachHangID = cboMaKhachHang.SelectedValue != null ? cboMaKhachHang.SelectedValue.ToString() : "",
                NhanVienID = txtMaNhanVien.Text.Trim(),
                NgayDat = dtpNgayDat.Value,
                TrangThai = cboTrangThai.SelectedItem != null ? cboTrangThai.SelectedItem.ToString() : "Chưa thanh toán",
                GhiChu = txtGhiChu.Text.Trim()
            };
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DonHang dh = GetInput();
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cboTrangThai.SelectedItem != null && cboTrangThai.SelectedItem.ToString() == "Đã giao")
            {
                MessageBox.Show("Không thể sửa trạng thái và ghi chú khi đơn hàng đã giao!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cboMaKhachHang.Enabled)
            {
                MessageBox.Show("Không thể thay đổi mã khách hàng của đơn hàng đã tạo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DonHang dh = GetInput();
            string result = bus.Update(dh);

            if (result == null)
            {
                MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo");
                LoadData();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result, "Thông báo lỗi");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {


            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa đơn hàng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bus.Delete(txtMaDonHang.Text);
                MessageBox.Show("Đã xóa toàn bộ vật tư!", "Thông báo");
                LoadData();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadData();
        }

        private void frmQL_DonHang_Load(object sender, EventArgs e)
        {
            txtMaDonHang.Enabled = false;
            txtMaNhanVien.Enabled = false;

            var danhSachKH = new BUSKhachHang().GetAll();
            cboMaKhachHang.DataSource = danhSachKH;
            cboMaKhachHang.DisplayMember = "TenKhachHang";
            cboMaKhachHang.ValueMember = "KhachHangID";
            cboMaKhachHang.SelectedIndex = -1;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Đã giao");
            cboTrangThai.Items.Add("Chưa xử lí");
            cboTrangThai.Items.Add("Đang vận chuyển");
            cboTrangThai.Items.Add("Đã hủy");
            cboTrangThai.Items.Add("Chưa thanh toán");
            cboTrangThai.SelectedIndex = 4;

            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = searchUtil.RemoveDiacritics(txtTimKiem.Text.Trim().ToLower());
            List<DonHang> danhSachDonHang = bus.GetAll();
            List<DonHang> ketQua = new List<DonHang>();

            foreach (DonHang dh in danhSachDonHang)
            {
                string ma = searchUtil.RemoveDiacritics(dh.DonHangID.ToLower());
                string khach = searchUtil.RemoveDiacritics(dh.KhachHangID.ToLower());
                string ghiChu = searchUtil.RemoveDiacritics((dh.GhiChu ?? "").ToLower());

                if (ma.Contains(keyword) || khach.Contains(keyword) || ghiChu.Contains(keyword))
                {
                    ketQua.Add(dh);
                }
            }

            dgvDonHang.DataSource = ketQua;
        }

        private void dgvDonHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDonHang.Rows[e.RowIndex];

                txtMaDonHang.Text = row.Cells["DonHangID"].Value.ToString();
                cboMaKhachHang.SelectedValue = row.Cells["KhachHangID"].Value.ToString();
                txtMaNhanVien.Text = row.Cells["NhanVienID"].Value.ToString();
                dtpNgayDat.Value = Convert.ToDateTime(row.Cells["NgayDat"].Value);

                string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "";

                for (int i = 0; i < cboTrangThai.Items.Count; i++)
                {
                    if (cboTrangThai.Items[i].ToString().Equals(trangThai, StringComparison.OrdinalIgnoreCase))
                    {
                        cboTrangThai.SelectedIndex = i;
                        break;
                    }
                }

                bool isLocked = trangThai == "Đã giao";
                cboMaKhachHang.Enabled = !isLocked;
                cboTrangThai.Enabled = !isLocked;
                txtGhiChu.ReadOnly = isLocked;

                txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();

                string maDonHang = row.Cells["DonHangID"].Value.ToString();
                frmQL_ChiTietDonHang frmChiTiet = new frmQL_ChiTietDonHang();
                frmChiTiet.DonHangID = maDonHang;
                frmChiTiet.ShowDialog();

                LoadData();
            }




        }
    }
}
