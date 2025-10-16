using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using UTIL_QuanLyVatTu;

namespace GUI_QuanLyVatTu
{
    public partial class frmQL_ChiTietDonHang : Form
    {
        BUSChiTietDonHang busChiTietDonHang = new BUSChiTietDonHang();
        BUSVatTu busVatTu = new BUSVatTu();
        private DataTable dtChiTietPhieu = new DataTable();

        public string DonHangID { get; set; }


        public frmQL_ChiTietDonHang()
        {
            InitializeComponent();
        }

       
        private void LoadData()
        {
            dgvChiTietDonHang.DataSource = busChiTietDonHang.GetAll();
            dgvChiTietDonHang.ClearSelection();
        }

        private void LoadComboBoxTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Đã thanh toán");
            cboTrangThai.Items.Add("Chờ thanh toán");
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadVatTu()
        {
            var listVatTu = busVatTu.GetAll();
            dgvChiTietVatTu.DataSource = listVatTu;
        }

        private void ResetForm()
        {
            txtMaChiTietDonHang.Text = busChiTietDonHang.GenerateID();
            txtMaDonHang.Text = DonHangID ?? "";
            txtMaVatTu.Text = "";
            txtSoLuong.Text = "";
            txtDonGia.Text = "";
            cboTrangThai.SelectedIndex = 0;
            txtTimKiem.Text = "";

            dgvChiTietDonHang.ClearSelection();
            dgvChiTietVatTu.ClearSelection();
            dgvChiTietDonHang.ClearSelection();
        }

        private ChiTietDonHang GetFormData()
        {
            return new ChiTietDonHang
            {
                ChiTietDonHangID = txtMaChiTietDonHang.Text.Trim(),
                DonHangID = txtMaDonHang.Text.Trim(),
                VatTuID = txtMaVatTu.Text.Trim(),
                SoLuong = int.TryParse(txtSoLuong.Text.Trim(), out int sl) ? sl : 1,
                DonGia = decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg) ? dg : 0,
                TrangThai = cboTrangThai.SelectedIndex == 0
            };
        }

        private void SetReadOnlyFields()
        {
            txtMaChiTietDonHang.ReadOnly = true;
            txtMaDonHang.ReadOnly = true;
            txtMaVatTu.ReadOnly = true;
            txtDonGia.ReadOnly = true;
        }

        private void TinhTongDonGia()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvChiTietDonHang.Rows)
            {
                if (row.Cells["SoLuong"].Value != null && row.Cells["DonGia"].Value != null)
                {
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);
                    tongTien += soLuong * donGia;
                }
            }
            txtDonGia.Text = tongTien.ToString("N0");
        }


        private void dgvChiTietDonHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvChiTietDonHang.Rows[e.RowIndex];
                txtMaChiTietDonHang.Text = row.Cells["ChiTietDonHangID"].Value.ToString();
                txtMaDonHang.Text = row.Cells["DonHangID"].Value.ToString();
                txtMaVatTu.Text = row.Cells["VatTuID"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
                cboTrangThai.SelectedIndex = (bool)row.Cells["TrangThai"].Value ? 0 : 1;
            }
        }

        private void dgvChiTietVatTu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvChiTietVatTu.Rows[e.RowIndex];
                string vatTuID = row.Cells["VatTuID"].Value.ToString();

                foreach (DataGridViewRow r in dgvChiTietDonHang.Rows)
                {
                    if (r.Cells["VatTuID"].Value?.ToString() == vatTuID)
                    {
                        MessageBox.Show("Vật tư đã tồn tại!", "Thông báo");
                        return;
                    }
                }

                ChiTietDonHang ct = new ChiTietDonHang
                {
                    ChiTietDonHangID = busChiTietDonHang.GenerateID(),
                    DonHangID = DonHangID,
                    VatTuID = vatTuID,
                    SoLuong = 1,
                    DonGia = decimal.TryParse(row.Cells["DonGia"].Value.ToString(), out decimal dg) ? dg : 0,
                    TrangThai = false
                };

                string result = busChiTietDonHang.Add(ct);
                if (result == null)
                {
                    MessageBox.Show("Thêm vật tư thành công!", "Thông báo");
                    LoadData();
                    TinhTongDonGia();
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result, "Thông báo lỗi");
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var ct = GetFormData();
            var result = busChiTietDonHang.Add(ct);
            if (result == null)
            {
                MessageBox.Show("Thêm chi tiết đơn hàng thành công!", "Thông báo");
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (dgvChiTietDonHang.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvChiTietDonHang.SelectedRows)
                {
                    dgvChiTietDonHang.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vật tư cần xóa!", "Thông báo");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa toàn bộ vật tư đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                busChiTietDonHang.Delete(txtMaChiTietDonHang.Text);
                MessageBox.Show("Đã xóa toàn bộ vật tư!", "Thông báo");
                LoadData();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
            LoadVatTu();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadVatTu();
                return;
            }

            string keywordNoDiacritics = searchUtil.RemoveDiacritics(keyword).ToLower();
            var listVatTu = busVatTu.GetAll().Where(v =>
                searchUtil.RemoveDiacritics(v.VatTuID).ToLower().Contains(keywordNoDiacritics) ||
                searchUtil.RemoveDiacritics(v.TenVatTu).ToLower().Contains(keywordNoDiacritics) ||
                searchUtil.RemoveDiacritics(v.GhiChu).ToLower().Contains(keywordNoDiacritics)
            ).ToList();

            dgvChiTietVatTu.DataSource = listVatTu;
        }

        private void frmQL_ChiTietDonHang_Load(object sender, EventArgs e)
        {
            LoadComboBoxTrangThai();
            LoadVatTu();
            
            SetReadOnlyFields();

            txtMaDonHang.Text = DonHangID ?? "";
            LoadData();
        }
        private decimal LayDonGiaTheoVatTu(string maVatTu)
        {
            foreach (DataGridViewRow row in dgvChiTietVatTu.Rows)
            {
                if (row.Cells["VatTuID"].Value != null && row.Cells["VatTuID"].Value.ToString() == maVatTu)
                {
                    decimal donGia;
                    if (decimal.TryParse(row.Cells["DonGia"].Value.ToString(), out donGia))
                        return donGia;
                }
            }
            return 0;
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaChiTietDonHang.Text))
            {
                MessageBox.Show("Vui lòng chọn chi tiết đơn hàng cần sửa!", "Thông báo");
                return;
            }

            ChiTietDonHang ct = GetFormData();

            decimal donGiaGoc = LayDonGiaTheoVatTu(ct.VatTuID);
            ct.DonGia = donGiaGoc * ct.SoLuong;

            string result = busChiTietDonHang.Update(ct);
            if (result == null)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                LoadData();
                TinhTongDonGia();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result, "Thông báo lỗi");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvChiTietDonHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvChiTietVatTu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
