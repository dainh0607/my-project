using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVatTu.Test
{
    public class BLLLoaiVatTu
    {
        
        private BUSLoaiVatTu bus;
        [SetUp]
        public void Setup()
        {
            bus = new BUSLoaiVatTu();
        }
        [Test]
        public void SinhMaID_PhaiBatDauBangLVT()
        {
            string id = bus.GenerateID();
            Assert.IsTrue(id.StartsWith("LVT"), "Mã sinh ra phải bắt đầu bằng 'LVT'");
            Assert.AreEqual(6, id.Length, "Mã phải có đúng 6 ký tự");
        }

        [Test]
        public void LayDanhSach_PhaiTraVeDanhSachLoaiVatTu()
        {
            var result = bus.GetAll();
            Assert.IsNotNull(result, "Danh sách không được null");
            Assert.IsInstanceOf<List<LoaiVatTu>>(result, "Phải là danh sách LoaiVatTu");
        }

        [Test]
        public void ThemLoaiVatTu_KhongCoTen_PhaiBaoLoi()
        {
            var loai = new LoaiVatTu
            {
                LoaiVatTuID = "LVT999",
                TenLoaiVatTu = "",
                NgayTao = DateTime.Now,
                GhiChu = "Test"
            };

            string result = bus.Add(loai);
            Assert.AreEqual("Tên loại vật tư không được để trống.", result, "Phải báo lỗi khi tên bị trống");
        }

        [Test]
        public void ThemVaXoaLoaiVatTu_PhaiThanhCong()
        {
            var loai = new LoaiVatTu
            {
                LoaiVatTuID = bus.GenerateID(),
                TenLoaiVatTu = "Vật tư test",
                NgayTao = DateTime.Now,
                GhiChu = "Thêm rồi xóa"
            };

            string addResult = bus.Add(loai);
            Assert.AreEqual("Success", addResult, "Thêm vật tư phải thành công");

            string deleteResult = bus.Delete(loai.LoaiVatTuID);
            Assert.AreEqual("Success", deleteResult, "Xóa vật tư phải thành công");
        }

        [Test]
        public void CapNhatLoaiVatTu_PhaiThanhCong()
        {
            var loai = new LoaiVatTu
            {
                LoaiVatTuID = "LVT001", // giả định tồn tại
                TenLoaiVatTu = "Xi măng sửa",
                NgayTao = DateTime.Now,
                GhiChu = "Cập nhật test"
            };

            string result = bus.Update(loai);
            Assert.AreEqual("Success", result, "Cập nhật phải thành công");
        }

        [Test]
        public void TimKiem_PhaiTraVeKetQuaPhuHop()
        {
            var result = bus.Search("Gạch");
            Assert.IsNotNull(result, "Kết quả tìm kiếm không được null");
            Assert.IsTrue(result.Exists(x => x.TenLoaiVatTu.Contains("Gạch", StringComparison.OrdinalIgnoreCase)), "Phải có kết quả chứa từ 'Gạch'");
        }
    }
}
