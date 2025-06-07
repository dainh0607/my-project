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
    public partial class frmQL_HoaDon : Form
    {
        private BUSHoaDon busHoaDon = new BUSHoaDon();

        public frmQL_HoaDon()
        {
            InitializeComponent();
        }
        private void frmQL_HoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            ResetForm();
        }
        private void LoadHoaDon()
        {
            guna2DataGridView1.DataSource = busHoaDon.GetAll();
        }
        private void TaoMaHoaDon()
        {
            txtMaHoaDon.Text = busHoaDon.GenerateID();
        }
        private void ResetForm()
        {
            TaoMaHoaDon();
            txtDonHang.Clear();
            cboThanhToan.Text = "";
            guna2DateTimePicker1.Value = DateTime.Now;
            txtDonHang.Focus();
            txtMaHoaDon.Enabled = true;
            txtTimKiemHoaDon.Clear();
        }


        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon
            {
                HoaDonID = txtMaHoaDon.Text.Trim(),
                DonHangID = txtDonHang.Text.Trim(),
                TongTien = 0,
                NgayThanhToan = guna2DateTimePicker1.Value,
                PhuongThucThanhToan = cboThanhToan.Text.Trim()
            };

            string result = busHoaDon.Add(hd);
            if (result == null)
            {
                MessageBox.Show("Thêm hóa đơn thành công!");
                LoadHoaDon();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result);
            }
        }

        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                txtMaHoaDon.Text = row.Cells["HoaDonID"].Value.ToString();
                txtDonHang.Text = row.Cells["DonHangID"].Value.ToString();
                guna2DateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgayThanhToan"].Value);
                cboThanhToan.Text = row.Cells["PhuongThucThanhToan"].Value.ToString();
            }
            txtMaHoaDon.Enabled = false;
        }

        private void btnSuaHoaDon_Click(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon
            {
                HoaDonID = txtMaHoaDon.Text.Trim(),
                DonHangID = txtDonHang.Text.Trim(),
                TongTien = 0,
                NgayThanhToan = guna2DateTimePicker1.Value,
                PhuongThucThanhToan = cboThanhToan.Text.Trim()
            };

            string result = busHoaDon.Update(hd);
            if (result == null)
            {
                MessageBox.Show("Cập nhật hóa đơn thành công!");
                LoadHoaDon();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result);
            }
        }

        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            string id = txtMaHoaDon.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string result = busHoaDon.Delete(id);
                if (result == null)
                {
                    MessageBox.Show("Xóa hóa đơn thành công!");
                    LoadHoaDon();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result);
                }
            }
        }

        private void btnLamMoiHoaDon_Click(object sender, EventArgs e)
        {
            BUSHoaDon bus = new BUSHoaDon();

            ResetForm();
        }

        private void btnTimKiemHoaDon_Click(object sender, EventArgs e)
        {
            BUSHoaDon bus = new BUSHoaDon();
            string keyword = txtTimKiemHoaDon.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                return;
            }

            var ketQua = bus.Search(keyword);
            guna2DataGridView1.DataSource = ketQua;
        }

        private void dtpNgayThanhToan_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}