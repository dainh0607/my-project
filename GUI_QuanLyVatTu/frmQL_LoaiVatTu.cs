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
    public partial class frmQL_LoaiVatTu : Form
    {
        BUSLoaiVatTu busLoai = new BUSLoaiVatTu();
        List<DTO_LoaiVatTu> danhSachLoai = new List<DTO_LoaiVatTu>();
        public frmQL_LoaiVatTu()
        {
            InitializeComponent();
        }

        void LamMoi()
        {
            danhSachLoai = busLoai.GetAll()
                .Select(x => new DTO_LoaiVatTu
                {
                    LoaiVatTuID = x.LoaiVatTuID,
                    TenLoaiVatTu = x.TenLoaiVatTu,
                    NgayTao = x.NgayTao,
                    GhiChu = x.GhiChu
                }).ToList();
            dgvLoaiVatTu.DataSource = danhSachLoai;

            txtLoaiVatTu.Text = busLoai.GenerateID();
            txtTenLoaiVatTu.Clear();
            txtGhiChu.Clear();
            dtpNgayTao.Value = DateTime.Now;
            txtTimKiem.Clear();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var loai = new DTO_LoaiVatTu
            {
                LoaiVatTuID = txtLoaiVatTu.Text,
                TenLoaiVatTu = txtTenLoaiVatTu.Text.Trim(),
                NgayTao = dtpNgayTao.Value,
                GhiChu = txtGhiChu.Text.Trim()
            };

            string addResult = busLoai.Add(loai);
            if (addResult == "Success")
            {
                MessageBox.Show("Thêm thành công!");
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Kiểm tra lại dữ liệu.");
            }
        }

        private void frmQL_LoaiVatTu_Load(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var loai = new DTO_LoaiVatTu
            {
                LoaiVatTuID = txtLoaiVatTu.Text,
                TenLoaiVatTu = txtTenLoaiVatTu.Text.Trim(),
                NgayTao = dtpNgayTao.Value,
                GhiChu = txtGhiChu.Text.Trim()
            };

            string updateResult = busLoai.Update(loai);
            if (updateResult == "Success")
            {
                MessageBox.Show("Cập nhật thành công!");
                LamMoi();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maLoai = txtLoaiVatTu.Text;

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xoá loại vật tư này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string deleteResult = busLoai.Delete(maLoai);
                if (deleteResult == "Success")
                {
                    MessageBox.Show("Xoá thành công!");
                    LamMoi();
                }
                else
                {
                    MessageBox.Show("Xoá thất bại!");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LamMoi(); // Hiển thị lại toàn bộ
                return;
            }

            var ketQua = busLoai.Search(keyword);
            dgvLoaiVatTu.DataSource = ketQua;
        }

        private void dgvLoaiVatTu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLoaiVatTu.Rows[e.RowIndex];

                txtLoaiVatTu.Text = row.Cells["LoaiVatTuID"].Value.ToString();
                txtTenLoaiVatTu.Text = row.Cells["TenLoaiVatTu"].Value.ToString();
                dtpNgayTao.Value = Convert.ToDateTime(row.Cells["NgayTao"].Value);
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();

                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }

        }
    }
}
