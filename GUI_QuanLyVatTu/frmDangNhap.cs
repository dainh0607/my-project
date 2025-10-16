using BLL_QuanLyVatTu;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using Guna.UI2.WinForms;
using UTIL_PolyCafe;

namespace GUI_QuanLyVatTu
{
    public partial class frmDangNhap : Form
    {
        BUSNhanVien BUSNhanVien = new BUSNhanVien();
        BUSDangNhap BUSDangNhap = new BUSDangNhap();

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtEmail.Text = Properties.Settings.Default.SavedTaiKhoan;
            txtMatKhau.Text = Properties.Settings.Default.SavedMatKhau;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();

                BUSDangNhap busDangNhap = new BUSDangNhap();
                string ketQua = busDangNhap.KiemTraDangNhap(email, matKhau);
                if (ketQua != "Đăng nhập thành công!")
                {
                    MessageBox.Show(this, ketQua, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DAL_NhanVien dalNhanVien = new DAL_NhanVien();
                NhanVien nv = dalNhanVien.getNhanVien1(email, matKhau);

                if (nv == null)
                {
                    MessageBox.Show(this, "Tài khoản hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!nv.TinhTrang)
                {
                    MessageBox.Show(this, "Tài khoản đã ngưng hoạt động, vui lòng liên hệ quản lý.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                AuthUtil.user = nv;
                frmLoadding frmLoadding = new frmLoadding();
                frmLoadding.ShowDialog();

                frmHome formHome = new frmHome(nv);
                this.Hide();
                formHome.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Lỗi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
