using BLL_QuanLyVatTu;
using DAL_QuanLyVatTu;
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
    public partial class frmQL_HoaDon : Form
    {
        private BUSHoaDon busHoaDon = new BUSHoaDon();

        public frmQL_HoaDon()
        {
            InitializeComponent();

            LoadComboDonHang();
            LoadComboThanhToan();
            LoadData();
        }


        private void LoadData()
        {
            var data = busHoaDon.SelectAll();
            dgvHoaDon.AutoGenerateColumns = true;
            dgvHoaDon.DataSource = null;
            dgvHoaDon.DataSource = data;
        }


        private void LoadComboDonHang()
        {
            DAL_DonHang dalDonHang = new DAL_DonHang();
            var list = dalDonHang.SelectAll();
            cboDonHangID.DataSource = list;
            cboDonHangID.DisplayMember = "DonHangID";
            cboDonHangID.ValueMember = "DonHangID";
            cboDonHangID.SelectedIndex = -1;

            cboDonHangID.SelectedIndexChanged -= cboDonHangID_SelectedIndexChanged;
            cboDonHangID.SelectedIndexChanged += cboDonHangID_SelectedIndexChanged;
        }

        private void LoadComboThanhToan()
        {
            cboThanhToan.DataSource = DAL_HoaDon.PaymentMethods;
        }

        private void cboDonHangID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDonHangID.SelectedItem is DonHang selectedDonHang)
            {
                txtKhachHangID.Text = selectedDonHang.KhachHangID ?? "";
                return;
            }
            if (cboDonHangID.SelectedItem is DataRowView drv)
            {
                txtKhachHangID.Text = drv["KhachHangID"]?.ToString() ?? "";
                return;
            }
            txtKhachHangID.Clear();
        }


        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon()
            {
                HoaDonID = busHoaDon.GenerateID(),
                DonHangID = cboDonHangID.SelectedValue.ToString(),
                KhachHangID = txtKhachHangID.Text,
                TongTien = decimal.Parse(txtTongTien.Text),
                NgayThanhToan = dtpNgayThanhToan.Value,
                PhuongThucThanhToan = cboThanhToan.Text
            };

            string result = busHoaDon.Add(hd);
            if (result == null)
            {
                MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoaDonID.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn cập nhật hóa đơn này?",
                                                   "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            HoaDon hd = new HoaDon()
            {
                HoaDonID = txtHoaDonID.Text,
                DonHangID = cboDonHangID.SelectedValue.ToString(),
                KhachHangID = txtKhachHangID.Text,
                TongTien = decimal.Parse(txtTongTien.Text),
                NgayThanhToan = dtpNgayThanhToan.Value,
                PhuongThucThanhToan = cboThanhToan.Text
            };

            string result = busHoaDon.Update(hd);
            if (result == null)
            {
                MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoaDonID.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            string result = busHoaDon.Delete(txtHoaDonID.Text);
            if (result == null)
            {
                MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoiHoaDon_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }

        private void ClearForm()
        {
            txtHoaDonID.Clear();
            txtKhachHangID.Clear();
            txtTongTien.Clear();
            if (cboDonHangID.Items.Count > 0) cboDonHangID.SelectedIndex = 0;
            if (cboThanhToan.Items.Count > 0) cboThanhToan.SelectedIndex = 0;
            dtpNgayThanhToan.Value = DateTime.Now;

            dgvHoaDon.ClearSelection();
        }

        private void btnTimKiemHoaDon_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            var list = busHoaDon.SelectAll();
            var result = list.FindAll(hd =>
                hd.HoaDonID.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                hd.KhachHangID.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            dgvHoaDon.DataSource = null;
            dgvHoaDon.DataSource = result;
        }


        private void dtpNgayThanhToan_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvHoaDon_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];
                txtHoaDonID.Text = row.Cells["HoaDonID"].Value?.ToString();
                cboDonHangID.SelectedValue = row.Cells["DonHangID"].Value?.ToString();
                txtKhachHangID.Text = row.Cells["KhachHangID"].Value?.ToString();
                txtTongTien.Text = row.Cells["TongTien"].Value?.ToString();
                dtpNgayThanhToan.Value = Convert.ToDateTime(row.Cells["NgayThanhToan"].Value);
                cboThanhToan.Text = row.Cells["PhuongThucThanhToan"].Value?.ToString();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            //Mở form In Hóa Đơn
            frmInHoaDon frmInHD = new frmInHoaDon();
            frmInHD.ShowDialog();
        }
    }
}
