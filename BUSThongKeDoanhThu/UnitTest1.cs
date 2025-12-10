using NUnit.Framework;
using BLL_QuanLyVatTu;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;

namespace Test_QuanLyVatTu
{
    [TestFixture]
    public class BUSThongKeDoanhThuTests
    {
        private Mock<DAL_ThongKeDoanhThu> _mockDalThongKe;
        private Mock<DAL_NhanVien> _mockDalNhanVien;
        private Mock<DAL_KhachHang> _mockDalKhachHang;
        private BUSThongKeDoanhThu _busThongKe;

        [SetUp]
        public void Setup()
        {
            _mockDalThongKe = new Mock<DAL_ThongKeDoanhThu>();
            _mockDalNhanVien = new Mock<DAL_NhanVien>();
            _mockDalKhachHang = new Mock<DAL_KhachHang>();
            _busThongKe = new BUSThongKeDoanhThu();

            var dalThongKeField = typeof(BUSThongKeDoanhThu).GetField("dalThongKe",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dalNhanVienField = typeof(BUSThongKeDoanhThu).GetField("dalNhanVien",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dalKhachHangField = typeof(BUSThongKeDoanhThu).GetField("dalKhachHang",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            dalThongKeField.SetValue(_busThongKe, _mockDalThongKe.Object);
            dalNhanVienField.SetValue(_busThongKe, _mockDalNhanVien.Object);
            dalKhachHangField.SetValue(_busThongKe, _mockDalKhachHang.Object);
        }

        [Test]
        [Description("Kiểm tra lấy tất cả thống kê doanh thu thành công")]
        public void SelectAll_KhiGoiMethod_PhaiTraVeDanhSachThongKe()
        {
            // Arrange
            var expectedList = new List<ThongKeDoanhThu>
            {
                new ThongKeDoanhThu
                {
                    DonHangID = "DH001",
                    ChiTietDonHangID = "CTDH001",
                    KhachHangID = "KH001",
                    NhanVienID = "NV001",
                    NgayDat = DateTime.Now,
                    DonGia = 1000000,
                    PhuongThucThanhToan = "Tiền mặt",
                    TrangThai = "Hoàn thành",
                    GhiChu = "Giao hàng thành công"
                }
            };

            _mockDalThongKe.Setup(dal => dal.SelectAll()).Returns(expectedList);

            // Act
            var result = _busThongKe.SelectAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("DH001", result[0].DonHangID);
            _mockDalThongKe.Verify(dal => dal.SelectAll(), Times.Once);
        }

        [Test]
        [Description("Kiểm tra thống kê theo điều kiện với đầy đủ tham số")]
        public void ThongKeTheoDieuKien_KhiCoDayDuThamSo_PhaiGoiDalVoiDungThamSo()
        {
            // Arrange
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);
            var nhanVienID = "NV001";
            var khachHangID = "KH001";
            var trangThai = "Hoàn thành";
            var phuongThuc = "Tiền mặt";

            var expectedList = new List<ThongKeDoanhThu>();
            _mockDalThongKe.Setup(dal => dal.SelectByFilter(
                fromDate, toDate, nhanVienID, khachHangID, trangThai, phuongThuc))
                .Returns(expectedList);

            // Act
            var result = _busThongKe.ThongKeTheoDieuKien(
                fromDate, toDate, nhanVienID, khachHangID, trangThai, phuongThuc);

            // Assert
            Assert.IsNotNull(result);
            _mockDalThongKe.Verify(dal => dal.SelectByFilter(
                fromDate, toDate, nhanVienID, khachHangID, trangThai, phuongThuc), Times.Once);
        }

        [Test]
        [Description("Kiểm tra thống kê theo điều kiện với tham số rỗng")]
        public void ThongKeTheoDieuKien_KhiThamSoRong_PhaiGoiDalVoiThamSoRong()
        {
            // Arrange
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);
            var expectedList = new List<ThongKeDoanhThu>();

            _mockDalThongKe.Setup(dal => dal.SelectByFilter(
                fromDate, toDate, null, null, null, null))
                .Returns(expectedList);

            // Act
            var result = _busThongKe.ThongKeTheoDieuKien(
                fromDate, toDate, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            _mockDalThongKe.Verify(dal => dal.SelectByFilter(
                fromDate, toDate, null, null, null, null), Times.Once);
        }

        [Test]
        [Description("Kiểm tra lấy danh sách nhân viên trả về DataTable đúng cấu trúc")]
        public void GetNhanVienList_KhiGoiMethod_PhaiTraVeDataTableDungCauTruc()
        {
            // Arrange
            var nhanVienList = new List<NhanVien>
            {
                new NhanVien { NhanVienID = "NV001", HoTen = "Nguyễn Văn A" },
                new NhanVien { NhanVienID = "NV002", HoTen = "Trần Thị B" }
            };

            _mockDalNhanVien.Setup(dal => dal.selectAll()).Returns(nhanVienList);

            // Act
            var result = _busThongKe.GetNhanVienList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DataTable", result.GetType().Name);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual("NhanVienID", result.Columns[0].ColumnName);
            Assert.AreEqual("HoTen", result.Columns[1].ColumnName);
            Assert.AreEqual("NV001", result.Rows[0]["NhanVienID"]);
            Assert.AreEqual("Nguyễn Văn A", result.Rows[0]["HoTen"]);
        }

        [Test]
        [Description("Kiểm tra lấy danh sách khách hàng trả về DataTable đúng cấu trúc")]
        public void GetKhachHangList_KhiGoiMethod_PhaiTraVeDataTableDungCauTruc()
        {
            // Arrange
            var khachHangList = new List<KhachHang>
            {
                new KhachHang { KhachHangID = "KH001", HoTen = "Lê Văn C" },
                new KhachHang { KhachHangID = "KH002", HoTen = "Phạm Thị D" }
            };

            _mockDalKhachHang.Setup(dal => dal.SelectAll()).Returns(khachHangList);

            // Act
            var result = _busThongKe.GetKhachHangList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DataTable", result.GetType().Name);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual("KhachHangID", result.Columns[0].ColumnName);
            Assert.AreEqual("HoTen", result.Columns[1].ColumnName);
            Assert.AreEqual("KH001", result.Rows[0]["KhachHangID"]);
            Assert.AreEqual("Lê Văn C", result.Rows[0]["HoTen"]);
        }

        [Test]
        [Description("Kiểm tra danh sách nhân viên rỗng")]
        public void GetNhanVienList_KhiDanhSachRong_PhaiTraVeDataTableRong()
        {
            // Arrange
            var emptyList = new List<NhanVien>();
            _mockDalNhanVien.Setup(dal => dal.selectAll()).Returns(emptyList);

            // Act
            var result = _busThongKe.GetNhanVienList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Rows.Count);
            Assert.AreEqual(2, result.Columns.Count);
        }

        [Test]
        [Description("Kiểm tra thống kê với ngày bắt đầu lớn hơn ngày kết thúc")]
        public void ThongKeTheoDieuKien_KhiFromDateLonHonToDate_PhaiXuLyDuoc()
        {
            // Arrange
            var fromDate = new DateTime(2024, 12, 31);
            var toDate = new DateTime(2024, 1, 1);
            var expectedList = new List<ThongKeDoanhThu>();

            _mockDalThongKe.Setup(dal => dal.SelectByFilter(
                fromDate, toDate, null, null, null, null))
                .Returns(expectedList);

            // Act
            var result = _busThongKe.ThongKeTheoDieuKien(
                fromDate, toDate, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            _mockDalThongKe.Verify(dal => dal.SelectByFilter(
                fromDate, toDate, null, null, null, null), Times.Once);
        }

        [Test]
        [Description("Kiểm tra xử lý khi DAL ném exception")]
        public void SelectAll_KhiDalNemException_PhaiNemException()
        {
            // Arrange
            _mockDalThongKe.Setup(dal => dal.SelectAll())
                .Throws(new Exception("Lỗi kết nối database"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _busThongKe.SelectAll());
            Assert.AreEqual("Lỗi kết nối database", exception.Message);
        }
    }

    [TestFixture]
    public class DAL_ThongKeDoanhThuTests
    {
        private DAL_ThongKeDoanhThu _dalThongKe;

        [SetUp]
        public void Setup()
        {
            _dalThongKe = new DAL_ThongKeDoanhThu();
        }

        [Test]
        [Description("Kiểm tra phương thức SelectAll không trả về null")]
        public void SelectAll_KhiGoiMethod_KhongTraVeNull()
        {
            // Act
            var result = _dalThongKe.SelectAll();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [Description("Kiểm tra SelectByFilter với tất cả tham số")]
        public void SelectByFilter_VoiTatCaThamSo_KhongTraVeNull()
        {
            // Arrange
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _dalThongKe.SelectByFilter(
                fromDate, toDate, "NV001", "KH001", "Hoàn thành", "Tiền mặt");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [Description("Kiểm tra SelectByFilter với chuỗi rỗng")]
        public void SelectByFilter_VoiChuoiRong_KhongTraVeNull()
        {
            // Arrange
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);

            // Act
            var result = _dalThongKe.SelectByFilter(
                fromDate, toDate, "", "", "", "");

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [Description("Kiểm tra SelectByFilter với khoảng thời gian hợp lệ")]
        public void SelectByFilter_VoiKhoangThoiGianHopLe_KhongTraVeNull()
        {
            // Arrange
            var fromDate = DateTime.MinValue;
            var toDate = DateTime.MaxValue;

            // Act
            var result = _dalThongKe.SelectByFilter(
                fromDate, toDate, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}