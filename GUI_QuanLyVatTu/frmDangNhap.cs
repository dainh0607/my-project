using BLL_QuanLyVatTu;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using Guna.UI2.WinForms;
using UTIL_PolyCafe;
using System;
using System.Windows.Forms;

namespace GUI_QuanLyVatTu
{
    public partial class frmDangNhap : Form
    {
        private readonly BUSNhanVien _busNhanVien;
        private readonly BUSDangNhap _busDangNhap;

        public frmDangNhap()
        {
            InitializeComponent();

            IDAL_NhanVien dal = new DAL_NhanVien();

            _busNhanVien = new BUSNhanVien(dal);
            _busDangNhap = new BUSDangNhap(dal);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtEmail.Text = Properties.Settings.Default.SavedTaiKhoan;
            txtMatKhau.Text = Properties.Settings.Default.SavedMatKhau;

            // Tự động check vào ô nhớ mật khẩu nếu đã có dữ liệu lưu
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                chkGhiNhoMatKhau.Checked = true;
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();

                // 3. Sử dụng _busDangNhap đã khai báo ở trên (Không new lại)
                string ketQua = _busDangNhap.KiemTraDangNhap(email, matKhau);

                // 4. SỬA LỖI LOGIC SO SÁNH:
                // Vì kết quả trả về có thể là "Đăng nhập thành công! (Admin)" nên dùng Contains
                if (!ketQua.Contains("Đăng nhập thành công"))
                {
                    MessageBox.Show(this, ketQua, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 5. SỬA LỖI KIẾN TRÚC: Gọi BUS để lấy thông tin (Thay vì gọi DAL trực tiếp)
                // Hàm DangNhap trong BUSNhanVien sẽ lo việc mã hóa mật khẩu trước khi tìm
                NhanVien nv = _busNhanVien.DangNhap(email, matKhau);

                // Kiểm tra null để an toàn (dù KiemTraDangNhap đã pass)
                if (nv == null)
                {
                    MessageBox.Show(this, "Có lỗi khi lấy thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!nv.TinhTrang)
                {
                    MessageBox.Show(this, "Tài khoản đã ngưng hoạt động, vui lòng liên hệ quản lý.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xử lý lưu mật khẩu
                if (chkGhiNhoMatKhau.Checked)
                {
                    Properties.Settings.Default.SavedTaiKhoan = txtEmail.Text;
                    Properties.Settings.Default.SavedMatKhau = txtMatKhau.Text;
                }
                else
                {
                    Properties.Settings.Default.SavedTaiKhoan = "";
                    Properties.Settings.Default.SavedMatKhau = "";
                }
                Properties.Settings.Default.Save();

                // Lưu session và chuyển form
                AuthUtil.user = nv;

                // Hiệu ứng Loading (Tùy chọn)
                frmLoadding frmLoadding = new frmLoadding();
                frmLoadding.ShowDialog();

                // Mở Form Home
                frmHome formHome = new frmHome(nv);
                this.Hide();
                formHome.ShowDialog();
                this.Show(); // Hiện lại form đăng nhập sau khi đăng xuất
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void chkHienThiMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = chkHienThiMatKhau.Checked ? '\0' : '*';
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản lý để cập nhật lại thông tin mật khẩu!");
        }
    }
}