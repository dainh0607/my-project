using NUnit.Framework;
using Moq;
using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;
using System.Collections.Generic;

namespace Test_QLVatTu
{
    [TestFixture]
    public class BUSDangNhapTests
    {
        private Mock<IDAL_NhanVien> _mockDal;
        private BUSDangNhap _bus;

        [SetUp]
        public void Setup()
        {
            _mockDal = new Mock<IDAL_NhanVien>();
            _bus = new BUSDangNhap(_mockDal.Object);
        }


        [Test]
        public void KiemTraDangNhap_AdminLogin_TraVeThongBaoAdmin()
        {
            string email = "admin@gmail.com";
            string rawPass = "admin123";
            string hashPass = _bus.MaHoaMD5(rawPass);

            var adminUser = new NhanVien
            {
                Email = email,
                MatKhau = hashPass,
                VaiTro = true,
                TinhTrang = true,
                ChucVu = "Admin",
                HoTen = "Nguyễn Văn Quản Lý"
            };

            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien> { adminUser });
            _mockDal.Setup(m => m.getNhanVien1(email, hashPass)).Returns(adminUser);

            var result = _bus.KiemTraDangNhap(email, rawPass);
            Assert.AreEqual("Đăng nhập thành công! (Admin)", result);
        }

        [Test]
        public void KiemTraDangNhap_UserLogin_TraVeThongBaoNhanVien()
        {
            string email = "user@gmail.com";
            string rawPass = "user123";
            string hashPass = _bus.MaHoaMD5(rawPass);

            var normalUser = new NhanVien
            {
                Email = email,
                MatKhau = hashPass,
                VaiTro = false,
                TinhTrang = true,
                ChucVu = "NhanVien",
                HoTen = "Trần Thị Nhân Viên"
            };

            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien> { normalUser });
            _mockDal.Setup(m => m.getNhanVien1(email, hashPass)).Returns(normalUser);
            var result = _bus.KiemTraDangNhap(email, rawPass);
            Assert.AreEqual("Đăng nhập thành công! (Nhân viên)", result);
        }

        [Test]
        public void KiemTraDangNhap_ChiEmailRong_TraVeLoiEmail()
        {
            var result = _bus.KiemTraDangNhap("", "123456");
            Assert.AreEqual("Email không được để trống!", result);
        }

        [Test]
        public void KiemTraDangNhap_ChiMatKhauRong_TraVeLoiMatKhau()
        {
            var result = _bus.KiemTraDangNhap("abc@gmail.com", "   ");
            Assert.AreEqual("Mật khẩu không được để trống!", result);
        }

        [TestCase("nguyenvana", "123")]
        [TestCase("test@yahoo.com", "123")]
        public void KiemTraDangNhap_EmailSaiDinhDang_TraVeLoi(string email, string pass)
        {
            var result = _bus.KiemTraDangNhap(email, pass);
            Assert.AreEqual("Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com", result);
        }

        [Test]
        public void KiemTraDangNhap_SaiMatKhau_TraVeLoi()
        {
            string email = "user@gmail.com";
            string passSai = "wrongpass";
            string hashPassSai = _bus.MaHoaMD5(passSai);
            var user = new NhanVien { Email = email, TinhTrang = true };
            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien> { user });
            _mockDal.Setup(m => m.getNhanVien1(email, hashPassSai)).Returns((NhanVien)null);

            var result = _bus.KiemTraDangNhap(email, passSai);
            Assert.AreEqual("Mật khẩu không hợp lệ!", result);
        }

        [Test]
        public void KiemTraDangNhap_EmailDungFormatNhungKhongTonTai_TraVeLoiEmailKhongHopLe()
        {
            string emailNhap = "chuaco@gmail.com";
            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien>
            {
                new NhanVien { Email = "nguoi_khac@gmail.com" }
            });
            var result = _bus.KiemTraDangNhap(emailNhap, "123456");

            Assert.AreEqual("Email không hợp lệ!", result);
        }

        [Test]
        public void KiemTraDangNhap_EmailChuHoa_HeThongTuDongHieu()
        {
            string emailInput = "ADMIN@GMAIL.COM";
            string emailDb = "admin@gmail.com"; 
            string pass = "123";
            string hash = _bus.MaHoaMD5(pass);
            var user = new NhanVien { Email = emailDb, VaiTro = true, TinhTrang = true };
            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien> { user });
            _mockDal.Setup(m => m.getNhanVien1(emailDb, hash)).Returns(user);
            var result = _bus.KiemTraDangNhap(emailInput, pass);

            Assert.IsTrue(result.Contains("Đăng nhập thành công"));
        }
    }
}