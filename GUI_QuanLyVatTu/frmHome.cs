using DTO_QuanLyVatTu;
using Microsoft.VisualBasic.ApplicationServices;

namespace GUI_QuanLyVatTu
{
    public partial class frmHome : Form
    {
        private NhanVien currentUser;
        public frmHome(NhanVien user)
        {
            InitializeComponent();
            currentUser = user;

            // Ẩn submenu ban đầu
            pnlSubMenu.Visible = false;

            // Gán sự kiện khi form load xong
            this.Load += Form1_Load;
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

            pnlSubMenu.Visible = false;
            pnlSubMenu2.Visible = false;

            RoundRightCorners(btnBanHang, 20);
            // Gọi hàm phân quyền
            PhanQuyen();
        }

        private void PhanQuyen()
        {
            if (currentUser.VaiTro) // Quản lý
            {
                // Hiện tất cả chức năng
                btnQuanLy.Visible = true;
                btnQuanLyPhieuBan.Visible = true;
                btnTaoPhieuBan.Visible = true;
                btnLoaiVatTu.Visible = true;
                btnNhanVien.Visible = true;
                btnNhaCungCap.Visible = true;
                btnDonHang.Visible = true;
                btnKhachHang.Visible = true;
                btnBaoCaoThongKe.Visible = true;
                pnlSubMenu.Visible = false;
                pnlSubMenu2.Visible = false;
            }
            else // Nhân viên
            {
                // Chỉ hiện chức năng bán hàng
                btnQuanLy.Visible = false;
                btnQuanLyPhieuBan.Visible = true;
                btnTaoPhieuBan.Visible = true;
                btnLoaiVatTu.Visible = false;
                btnNhanVien.Visible = false;
                btnNhaCungCap.Visible = false;
                btnDonHang.Visible = false;
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
            await Task.Delay(200); // đợi xem chuột có đi vào pnlSubMenu không
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
            await Task.Delay(40); // đợi xem chuột có quay lại btnQuanLy không
            if (!btnQuanLy.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu.Visible = false;
            }
        }

        private void RoundRightCorners(Guna.UI2.WinForms.Guna2Button btn, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            int w = btn.Width;
            int h = btn.Height;

            path.StartFigure();
            // Top-left (vuông)
            path.AddLine(0, 0, w - radius, 0);
            // Top-right (bo)
            path.AddArc(w - radius, 0, radius, radius, 270, 90);
            // Bottom-right (bo)
            path.AddLine(w, radius, w, h - radius);
            path.AddArc(w - radius, h - radius, radius, radius, 0, 90);
            // Bottom-left (vuông)
            path.AddLine(w - radius, h, 0, h);
            path.AddLine(0, h, 0, 0);
            path.CloseFigure();

            btn.Region = new Region(path);
        }


        private void pnlSubMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {

        }

        private void pnlSubMenu2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void pnlSubMenu2_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(40); // đợi xem chuột có quay lại btnQuanLy không
            if (!btnQuanLy.Bounds.Contains(PointToClient(Cursor.Position)) &&
                !pnlSubMenu.Bounds.Contains(PointToClient(Cursor.Position)))
            {
                pnlSubMenu.Visible = false;
            }
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {

        }

        private void btnQuanLyPhieuBan_Click(object sender, EventArgs e)
        {

        }

        private void btnTaoPhieuBan_Click(object sender, EventArgs e)
        {

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

        private void Home_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }
    }
}
