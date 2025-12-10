using NUnit.Framework;
using Moq;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BLL_QuanLyVatTu.Tests
{
    [TestFixture]
    public class BUSNhaCungCapTests
    {
        private Mock<DAL_NhaCungCap> _mockDal;
        private BUSNhaCungCap _busNhaCungCap;

        [SetUp]
        public void Setup()
        {
            _mockDal = new Mock<DAL_NhaCungCap>();
            _busNhaCungCap = new BUSNhaCungCap();

            // Inject mock DAL vào BUS bằng Reflection
            var dalField = typeof(BUSNhaCungCap).GetField("dal",
                BindingFlags.NonPublic | BindingFlags.Instance);
            dalField.SetValue(_busNhaCungCap, _mockDal.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsListFromDAL()
        {
            // Arrange
            var expectedList = new List<NhaCungCap>
            {
                new NhaCungCap
                {
                    NhaCungCapID = "NCC001",
                    TenNhaCungCap = "Công ty A",
                    SoDienThoai = "0901234567",
                    Email = "contact@companya.com",
                    DiaChi = "123 Đường ABC"
                },
                new NhaCungCap
                {
                    NhaCungCapID = "NCC002",
                    TenNhaCungCap = "Công ty B",
                    SoDienThoai = "0912345678",
                    Email = "info@companyb.com",
                    DiaChi = "456 Đường XYZ"
                }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(expectedList);

            // Act
            var result = _busNhaCungCap.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("NCC001", result[0].NhaCungCapID);
            Assert.AreEqual("Công ty A", result[0].TenNhaCungCap);
            _mockDal.Verify(dal => dal.SelectAll(), Times.Once);
        }

        [Test]
        public void Add_WithValidNhaCungCap_ReturnsNullForSuccess()
        {
            // Arrange
            var newNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC003",
                TenNhaCungCap = "Công ty C",
                SoDienThoai = "0923456789",
                Email = "sales@companyc.com",
                DiaChi = "789 Đường DEF",
                NgayTao = DateTime.Now,
                GhiChu = "Ghi chú test"
            };

            var existingList = new List<NhaCungCap>
            {
                new NhaCungCap { TenNhaCungCap = "Công ty A" },
                new NhaCungCap { TenNhaCungCap = "Công ty B" }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);
            _mockDal.Setup(dal => dal.Insert(It.IsAny<NhaCungCap>())).Returns((string)null);

            // Act
            var result = _busNhaCungCap.Add(newNhaCungCap);

            // Assert
            Assert.IsNull(result); // Success returns null
            _mockDal.Verify(dal => dal.Insert(newNhaCungCap), Times.Once);
        }

        [Test]
        public void Add_WithDuplicateTenNhaCungCap_ReturnsErrorMessage()
        {
            // Arrange
            var newNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC003",
                TenNhaCungCap = "Công ty A", // Trùng tên
                SoDienThoai = "0923456789",
                Email = "sales@companyc.com",
                DiaChi = "789 Đường DEF",
                NgayTao = DateTime.Now
            };

            var existingList = new List<NhaCungCap>
            {
                new NhaCungCap { TenNhaCungCap = "Công ty A" },
                new NhaCungCap { TenNhaCungCap = "Công ty B" }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);

            // Act
            var result = _busNhaCungCap.Add(newNhaCungCap);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Tên nhà cung cấp đã tồn tại"));
            _mockDal.Verify(dal => dal.Insert(It.IsAny<NhaCungCap>()), Times.Never);
        }

        [Test]
        public void Add_WithInvalidPhoneNumber_ReturnsErrorMessage()
        {
            // Arrange
            var newNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC003",
                TenNhaCungCap = "Công ty C",
                SoDienThoai = "123", // Số điện thoại không hợp lệ
                Email = "sales@companyc.com",
                DiaChi = "789 Đường DEF",
                NgayTao = DateTime.Now
            };

            var existingList = new List<NhaCungCap>();
            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);

            // Act
            var result = _busNhaCungCap.Add(newNhaCungCap);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Số điện thoại không hợp lệ"));
            _mockDal.Verify(dal => dal.Insert(It.IsAny<NhaCungCap>()), Times.Never);
        }

        [Test]
        public void Add_WithInvalidEmail_ReturnsErrorMessage()
        {
            // Arrange
            var newNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC003",
                TenNhaCungCap = "Công ty C",
                SoDienThoai = "0923456789",
                Email = "invalid-email", // Email không hợp lệ
                DiaChi = "789 Đường DEF",
                NgayTao = DateTime.Now
            };

            var existingList = new List<NhaCungCap>();
            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);

            // Act
            var result = _busNhaCungCap.Add(newNhaCungCap);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Email không hợp lệ"));
            _mockDal.Verify(dal => dal.Insert(It.IsAny<NhaCungCap>()), Times.Never);
        }

        [Test]
        public void Add_WithEmptyRequiredFields_ReturnsErrorMessage()
        {
            // Arrange
            var newNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC003",
                TenNhaCungCap = "", // Trống
                SoDienThoai = "", // Trống
                Email = "", // Trống
                DiaChi = "", // Trống
                NgayTao = DateTime.Now
            };

            var existingList = new List<NhaCungCap>();
            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);

            // Act
            var result = _busNhaCungCap.Add(newNhaCungCap);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Tên nhà cung cấp không được để trống"));
            Assert.IsTrue(result.Contains("Số điện thoại không được để trống"));
            Assert.IsTrue(result.Contains("Email không được để trống"));
            Assert.IsTrue(result.Contains("Địa chỉ không được để trống"));
            _mockDal.Verify(dal => dal.Insert(It.IsAny<NhaCungCap>()), Times.Never);
        }

        [Test]
        public void Update_WithValidNhaCungCap_ReturnsNullForSuccess()
        {
            // Arrange
            var existingNhaCungCap = new NhaCungCap
            {
                NhaCungCapID = "NCC001",
                TenNhaCungCap = "Công ty A Updated",
                SoDienThoai = "0901234567",
                Email = "updated@companya.com",
                DiaChi = "123 Đường ABC Updated",
                NgayTao = DateTime.Now,
                GhiChu = "Ghi chú updated"
            };

            var existingList = new List<NhaCungCap>
            {
                new NhaCungCap { TenNhaCungCap = "Công ty A" },
                new NhaCungCap { TenNhaCungCap = "Công ty B" }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(existingList);
            _mockDal.Setup(dal => dal.Update(It.IsAny<NhaCungCap>())).Returns((string)null);

            // Act
            var result = _busNhaCungCap.Update(existingNhaCungCap);

            // Assert
            Assert.IsNull(result); // Success returns null
            _mockDal.Verify(dal => dal.Update(existingNhaCungCap), Times.Once);
        }

        [Test]
        public void Delete_WithValidID_ReturnsNullForSuccess()
        {
            // Arrange
            string nhaCungCapID = "NCC001";
            _mockDal.Setup(dal => dal.Delete(nhaCungCapID)).Returns((string)null);

            // Act
            var result = _busNhaCungCap.Delete(nhaCungCapID);

            // Assert
            Assert.IsNull(result); // Success returns null
            _mockDal.Verify(dal => dal.Delete(nhaCungCapID), Times.Once);
        }

        [Test]
        public void Delete_WhenFails_ReturnsErrorMessage()
        {
            // Arrange
            string nhaCungCapID = "NCC001";
            string errorMessage = "Không thể xóa vì có ràng buộc dữ liệu";
            _mockDal.Setup(dal => dal.Delete(nhaCungCapID)).Returns(errorMessage);

            // Act
            var result = _busNhaCungCap.Delete(nhaCungCapID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(errorMessage, result);
            _mockDal.Verify(dal => dal.Delete(nhaCungCapID), Times.Once);
        }

        [Test]
        public void GenerateID_WhenCalled_ReturnsGeneratedID()
        {
            // Arrange
            string expectedID = "NCC010";
            _mockDal.Setup(dal => dal.GenerateID()).Returns(expectedID);

            // Act
            var result = _busNhaCungCap.GenerateID();

            // Assert
            Assert.AreEqual(expectedID, result);
            _mockDal.Verify(dal => dal.GenerateID(), Times.Once);
        }

        [Test]
        public void Search_WithKeyword_ReturnsFilteredResults()
        {
            // Arrange
            var allNhaCungCap = new List<NhaCungCap>
            {
                new NhaCungCap
                {
                    TenNhaCungCap = "Công ty ABC Việt Nam",
                    SoDienThoai = "0901111111",
                    Email = "abc@company.com",
                    DiaChi = "Hà Nội"
                },
                new NhaCungCap
                {
                    TenNhaCungCap = "Công ty XYZ",
                    SoDienThoai = "0912222222",
                    Email = "xyz@company.com",
                    DiaChi = "Đà Nẵng"
                },
                new NhaCungCap
                {
                    TenNhaCungCap = "Công ty DEF",
                    SoDienThoai = "0923333333",
                    Email = "def@company.com",
                    DiaChi = "TPHCM"
                }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(allNhaCungCap);

            // Act
            var result = _busNhaCungCap.Search("abc"); // Tìm "abc" không dấu

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Công ty ABC Việt Nam", result[0].TenNhaCungCap);
        }

        [Test]
        public void Search_WithVietnameseSigns_ReturnsCorrectResults()
        {
            // Arrange
            var allNhaCungCap = new List<NhaCungCap>
            {
                new NhaCungCap
                {
                    TenNhaCungCap = "Công ty Điện Tử",
                    SoDienThoai = "0901111111",
                    Email = "dientu@company.com",
                    DiaChi = "Hà Nội"
                },
                new NhaCungCap
                {
                    TenNhaCungCap = "Công ty Dệt May",
                    SoDienThoai = "0912222222",
                    Email = "detmay@company.com",
                    DiaChi = "Hải Phòng"
                }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(allNhaCungCap);

            // Act - Tìm không dấu
            var result1 = _busNhaCungCap.Search("dien tu"); // "điện tử" không dấu
            var result2 = _busNhaCungCap.Search("det may"); // "dệt may" không dấu

            // Assert
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual("Công ty Điện Tử", result1[0].TenNhaCungCap);

            Assert.AreEqual(1, result2.Count);
            Assert.AreEqual("Công ty Dệt May", result2[0].TenNhaCungCap);
        }

        [Test]
        public void Search_WithEmptyKeyword_ReturnsAllResults()
        {
            // Arrange
            var allNhaCungCap = new List<NhaCungCap>
            {
                new NhaCungCap { TenNhaCungCap = "Công ty A" },
                new NhaCungCap { TenNhaCungCap = "Công ty B" }
            };

            _mockDal.Setup(dal => dal.SelectAll()).Returns(allNhaCungCap);

            // Act
            var result = _busNhaCungCap.Search("   "); // Khoảng trắng

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void RemoveVietnameseSigns_WhenCalled_RemovesAccentsCorrectly()
        {
            // Arrange
            string input = "Đây là tiếng Việt có dấu: á, à, ả, ã, ạ, ă, â, đ, é, è, ẻ, ẽ, ẹ, ê, í, ì, ỉ, ĩ, ị, ó, ò, ỏ, õ, ọ, ô, ơ, ú, ù, ủ, ũ, ụ, ư, ý, ỳ, ỷ, ỹ, ỵ";
            string expected = "Day la tieng Viet co dau: a, a, a, a, a, a, a, d, e, e, e, e, e, e, i, i, i, i, i, o, o, o, o, o, o, o, u, u, u, u, u, u, y, y, y, y, y";

            // Act
            var result = BUSNhaCungCap.RemoveVietnameseSigns(input);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}