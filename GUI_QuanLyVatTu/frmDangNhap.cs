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

        public frmDangNhap()
        {
            InitializeComponent();




        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            txtEmail.Text = Properties.Settings.Default.SavedTaiKhoan;
            txtMatKhau.Text = Properties.Settings.Default.SavedMatKhau;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtEmail.Text;
            string password = txtMatKhau.Text;
            NhanVien nv = BUSNhanVien.DangNhap(username, password);
            if (nv == null)
            {
                MessageBox.Show(this, "Tài khoản hoặc mật khẩu không chính xác");
                return;
            }
            // Kiểm tra trạng thái tài khoản
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
            Properties.Settings.Default.Save(); // Ghi nhớ mật khẩu
            AuthUtil.user = nv;
            frmLoadding frmLoadding = new frmLoadding();
            frmLoadding.ShowDialog();
            frmHome formHome = new frmHome(nv); // Pass the required 'NhanVien' parameter
            this.Hide();
            formHome.ShowDialog();
            this.Show();
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
