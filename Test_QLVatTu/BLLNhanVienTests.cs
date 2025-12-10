using NUnit.Framework;
using Moq;
using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;
using System.Collections.Generic;

namespace Test_QLVatTu
{
    [TestFixture]
    public class BLLNhanVienTests
    {
        private Mock<IDAL_NhanVien> _mockDal;
        private BUSNhanVien _bus;
        private NhanVien _validNhanVien;

        [SetUp]
        public void Setup()
        {
            _mockDal = new Mock<IDAL_NhanVien>();

            _bus = new BUSNhanVien(_mockDal.Object);
            _validNhanVien = new NhanVien
            {
                NhanVienID = "NV001",
                HoTen = "Nguyen Van A",
                ChucVu = "NhanVien",
                SoDienThoai = "0912345678",
                Email = "test@gmail.com",
                MatKhau = "123456",
                VaiTro = false,
                TinhTrang = true
            };
        }

        [Test]
        public void InsertNhanVien_MatKhauPhaiDuocMaHoaTruocKhiLuu()
        {
            string rawPass = "123456";
            _validNhanVien.MatKhau = rawPass;
            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien>());
            _mockDal.Setup(m => m.Insert(It.IsAny<NhanVien>())).Returns((string)null);
            _bus.InsertNhanVien(_validNhanVien);

            _mockDal.Verify(m => m.Insert(It.Is<NhanVien>(nv =>
                nv.MatKhau != rawPass && nv.MatKhau.Length > 20
            )), Times.Once, "Lỗi: Mật khẩu gửi xuống DAL vẫn là mật khẩu thô chưa mã hóa!");
        }

        [Test]
        public void DangNhap_MatKhauPhaiDuocMaHoaTruocKhiGuiXuongDAL()
        {
            string email = "test@gmail.com";
            string rawPass = "123456";
            _bus.DangNhap(email, rawPass);
            _mockDal.Verify(m => m.getNhanVien1(email, It.Is<string>(pass =>
                pass != rawPass
            )), Times.Once, "Lỗi: BLL đang gửi mật khẩu thô xuống DAL để check đăng nhập!");
        }

        [Test]
        public void InsertNhanVien_DuLieuHopLe_TraVeNull() 
        {
            _mockDal.Setup(m => m.selectAll()).Returns(new List<NhanVien>());
            _mockDal.Setup(m => m.Insert(It.IsAny<NhanVien>())).Returns((string)null);

            var result = _bus.InsertNhanVien(_validNhanVien);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Thêm thành công phải không có thông báo lỗi");
            _mockDal.Verify(m => m.Insert(It.IsAny<NhanVien>()), Times.Once);
        }

        [Test]
        public void InsertNhanVien_BoTrongTen_TraVeLoi()
        {
            _validNhanVien.HoTen = "";
            var result = _bus.InsertNhanVien(_validNhanVien);
            Assert.AreEqual("Vui lòng điền họ tên.", result);
        }

        [Test]
        public void InsertNhanVien_TenChuaSo_TraVeLoiFormat()
        {
            _validNhanVien.HoTen = "Nguyen Van A 123";
            var result = _bus.InsertNhanVien(_validNhanVien);
            Assert.AreEqual("Tên người dùng chỉ bao gồm chữ.", result);
        }

        [TestCase("091234567a", "Số điện thoại chỉ bao gồm số.")]
        [TestCase("09123456", "Số điện thoại phải có đủ 10 số.")]
        [TestCase("091234567899", "Số điện thoại đã vượt quá 10 số.")]
        [TestCase("0112345678", "Đầu số điện thoại phải khớp với các nhà mạng di động (03, 05, 07, 08, 09, 02).")]
        public void InsertNhanVien_SDTKhongHopLe_TraVeLoiTuongUng(string sdtInput, string messageMongDoi)
        {
            _validNhanVien.SoDienThoai = sdtInput;
            var result = _bus.InsertNhanVien(_validNhanVien);
            Assert.AreEqual(messageMongDoi, result);
        }

        [Test]
        public void InsertNhanVien_EmailSaiFormat_TraVeLoiEmail()
        {
            _validNhanVien.Email = "abc@yahoo.com";
            var result = _bus.InsertNhanVien(_validNhanVien);
            Assert.AreEqual("Email phải có đuôi @gmail.com.", result);
        }

        [Test]
        public void InsertNhanVien_TrungEmail_TraVeLoiTrung()
        {
            var listTonTai = new List<NhanVien> { new NhanVien { Email = "test@gmail.com", SoDienThoai = "0988888888" } };
            _mockDal.Setup(m => m.selectAll()).Returns(listTonTai);
            var result = _bus.InsertNhanVien(_validNhanVien);

            Assert.AreEqual("Nhân viên này đã tồn tại trong hệ thống.", result);
        }

        [Test]
        public void UpdateNhanVien_ThongTinHopLe_ThanhCong()
        {
            _mockDal.Setup(m => m.updateNhanVien(It.IsAny<NhanVien>()));

            var result = _bus.UpdateNhanVien(_validNhanVien);

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void DeleteNhanVien_NhanVienDangCoDonHang_ChanXoa()
        {
            string maNV = "NV_VIP";

            _mockDal.Setup(m => m.KiemTraLienKetDuLieu(maNV)).Returns(true);
            var result = _bus.DeleteNhanVien(maNV);
            Assert.AreEqual("Không thể xóa nhân viên này vì đã phát sinh đơn hàng/nghiệp vụ.", result);

            _mockDal.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void DeleteNhanVien_NhanVienMoi_XoaThanhCong()
        {
            string maNV = "NV_NEW";
            _mockDal.Setup(m => m.KiemTraLienKetDuLieu(maNV)).Returns(false);
            _mockDal.Setup(m => m.Delete(maNV)).Returns((string)null);

            var result = _bus.DeleteNhanVien(maNV);

            Assert.IsNull(result);
            _mockDal.Verify(m => m.Delete(maNV), Times.Once);
        }

        [Test]
        public void SearchNhanVien_TraVeDanhSachKetQua()
        {
            var fakeList = new List<NhanVien> { _validNhanVien };
            _mockDal.Setup(m => m.SelectBySql(It.IsAny<string>(), It.IsAny<List<object>>()))
                    .Returns(fakeList);

            var result = _bus.SearchNhanVien("Nguyen");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("NV001", result[0].NhanVienID);
        }

        [Test]
        public void TaoMaNhanVienTuDong_TraVeMaTiepTheo()
        {
            _mockDal.Setup(m => m.generateMaNhanVien()).Returns("NV010");

            var result = _bus.TaoMaNhanVienTuDong();

            Assert.AreEqual("NV010", result);
        }
    }
}