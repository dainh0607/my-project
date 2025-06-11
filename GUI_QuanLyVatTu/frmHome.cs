using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GUI_QuanLyVatTu
{
    public partial class frmHome : Form
    {
        private NhanVien currentUser;
        private Form currentFormChild;


        public frmHome(NhanVien user)
        {
            InitializeComponent();
            currentUser = user;

            pnlSubMenu.Visible = false;
            pnlSubMenu2.Visible = false;

            this.Load += Form1_Load;

            // Bật DoubleBuffer cho panelHienThi để tránh flicker
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelHienThi, new object[] { true });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Hover btnQuanLy
            btnQuanLy.MouseEnter += btnQuanLy_MouseEnter;
            btnQuanLy.MouseLeave += btnQuanLy_MouseLeave;
            pnlSubMenu.MouseEnter += pnlSubMenu_MouseEnter;
            pnlSubMenu.MouseLeave += pnlSubMenu_MouseLeave;

            // Hover btnBanHang
            btnBanHang.MouseEnter += btnBanHang_MouseEnter;
            btnBanHang.MouseLeave += btnBanHang_MouseLeave;
            pnlSubMenu2.MouseEnter += pnlSubMenu2_MouseEnter;
            pnlSubMenu2.MouseLeave += pnlSubMenu2_MouseLeave;

            RoundRightCorners(btnBanHang, 20);

            // Phân quyền
            PhanQuyen();
        }

        private void PhanQuyen()
        {
            if (currentUser.VaiTro) // Quản lý
            {
                btnQuanLy.Visible = true;
                btnQuanLyPhieuBan.Visible = true;
                btnTaoPhieuBan.Visible = true;
                btnLoaiVatTu.Visible = true;
                btnNhanVien.Visible = true;
                btnNhaCungCap.Visible = true;
                btnHoaDon.Visible = true;
                btnKhachHang.Visible = true;
                btnBaoCaoThongKe.Visible = true;
                pnlSubMenu.Visible = false;
                pnlSubMenu2.Visible = false;
            }
            else // Nhân viên
            {
                btnQuanLy.Visible = false;
                btnQuanLyPhieuBan.Visible = true;
                btnTaoPhieuBan.Visible = true;
                btnLoaiVatTu.Visible = false;
                btnNhanVien.Visible = false;
                btnNhaCungCap.Visible = false;
                btnHoaDon.Visible = false;
                btnKhachHang.Visible = false;
                btnBaoCaoThongKe.Visible = false;
                pnlSubMenu.Visible = false;
                pnlSubMenu2.Visible = true;
            }
        }

        private void btnQuanLy_MouseEnter(object sender, EventArgs e)
        {
            pnlSubMenu.Visible = true;
        }

        private async void btnQuanLy_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(200);
            if (!btnQuanLy.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu.Visible = false;
            }
        }

        private void pnlSubMenu_MouseEnter(object sender, EventArgs e)
        {
            pnlSubMenu.Visible = true;
        }

        private async void pnlSubMenu_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(40);
            if (!btnQuanLy.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu.Visible = false;
            }
        }

        private void RoundRightCorners(Guna.UI2.WinForms.Guna2Button btn, int radius)
        {
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int w = btn.Width;
                int h = btn.Height;

                path.StartFigure();
                path.AddLine(0, 0, w - radius, 0);
                path.AddArc(w - radius, 0, radius, radius, 270, 90);
                path.AddLine(w, radius, w, h - radius);
                path.AddArc(w - radius, h - radius, radius, radius, 0, 90);
                path.AddLine(w - radius, h, 0, h);
                path.AddLine(0, h, 0, 0);
                path.CloseFigure();

                btn.Region?.Dispose();
                btn.Region = new Region(path);
            }
        }

        private void openChildForm(Form formChild)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }

            currentFormChild = formChild;
            formChild.TopLevel = false;
            formChild.FormBorderStyle = FormBorderStyle.None;
            formChild.Dock = DockStyle.Fill;
            panelHienThi.Controls.Clear();
            panelHienThi.Controls.Add(formChild);
            panelHienThi.Tag = formChild;
            formChild.BringToFront();
            formChild.Show();
        }

        private void btnTaoPhieuBan_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_DonHang());
        }

        private void btnBanHang_MouseEnter(object sender, EventArgs e)
        {
            pnlSubMenu2.Visible = true;
        }

        private async void btnBanHang_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(200);
            if (!btnBanHang.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu2.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu2.Visible = false;
            }
        }

        private void pnlSubMenu2_MouseEnter(object sender, EventArgs e)
        {
            pnlSubMenu2.Visible = true;
        }

        private async void pnlSubMenu2_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(40);
            if (!btnBanHang.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu2.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu2.Visible = false;
            }
        }

        private void btnLoaiVatTu_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_LoaiVatTu());
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_NhaCungCap());
        }

        private void btnDonHang_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_HoaDon());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_KhachHang());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_NhanVien());
        }

        private void btnQuanLyPhieuBan_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_ChiTietDonHang());
        }

        private void btnVatTu_Click(object sender, EventArgs e)
        {
            openChildForm(new frmQL_VatTu());
        }

        private void Home_Load(object sender, EventArgs e)
        {
        }

    }
}
