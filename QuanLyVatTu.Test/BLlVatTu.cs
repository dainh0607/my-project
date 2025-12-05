using BLL_QuanLyVatTu;
using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace QuanLyVatTu.Test
{
    public class Tests
    {
        private BUSVatTu bus;
        
        [SetUp]
        public void Setup()
        {
            bus = new BUSVatTu();

        }

        [Test]
        public void Add_ThiếuTenVatTu()
        {

            var vt = new VatTu
            {
                VatTuID = bus.GenerateID(),
                TenVatTu = "", 
                LoaiVatTuID = "LVT001",
                DonGia = 100000,
                SoLuongTon = 10,
                NhaCungCapID = "NCC001",
                NgayTao = DateTime.Now,
                GhiChu = "Thiếu tên",
                TrangThaiID = "TT001"
            };

            string result = bus.Add(vt);
            Assert.AreEqual("Tên vật tư không được để trống.", result);
        }
        [Test]
        public void Add_ThiếuLoaiVatTu()
        {
            var vt = new VatTu
            {
                VatTuID = bus.GenerateID(),
                TenVatTu = "Cát xây",
                LoaiVatTuID = "",
                DonGia = 100000,
                SoLuongTon = 10,
                NhaCungCapID = "NCC001",
                NgayTao = DateTime.Now,
                GhiChu = "Thiếu loại",
                TrangThaiID = "TT001"
            };

            string result = bus.Add(vt);
            Assert.AreEqual("Loại vật tư không được để trống.", result);
        }
        [Test]
        public void Add_DonGiaAm()
        {
            var vt = new VatTu
            {
                VatTuID = bus.GenerateID(),
                TenVatTu = "Gạch",
                LoaiVatTuID = "LVT002",
                DonGia = -5000,
                SoLuongTon = 100,
                NhaCungCapID = "NCC003",
                NgayTao = DateTime.Now,
                GhiChu = "Đơn giá âm",
                TrangThaiID = "TT001"
            };

            string result = bus.Add(vt);
            Assert.AreEqual("Đơn giá phải lớn hơn 0.", result);
        }
        [Test]
        public void Update_VatTuTonTai()
        {
            var vt = new VatTu
            {
                VatTuID = "VT003", // ✅ vật tư đã có
                TenVatTu = "Thép Pomina D10 - sửa",
                LoaiVatTuID = "LVT003",
                DonGia = 210000,
                SoLuongTon = 250,
                NhaCungCapID = "NCC002",
                NgayTao = DateTime.Now,
                GhiChu = "Cập nhật đơn giá",
                TrangThaiID = "TT001"
            };

            string result = bus.Update(vt);
            Assert.IsTrue(result == null || result.Contains("thành công"));
        }

        [Test]
        public void Update_VatTuKhongTonTai()
        {
            var vt = new VatTu
            {
                VatTuID = "VT999", 
                TenVatTu = "Vật tư ảo",
                LoaiVatTuID = "LVT001",
                DonGia = 100000,
                SoLuongTon = 10,
                NhaCungCapID = "NCC001",
                NgayTao = DateTime.Now,
                GhiChu = "Không tồn tại",
                TrangThaiID = "TT001"
            };

            string result = bus.Update(vt);
            Assert.AreEqual("Không tìm thấy vật tư cần sửa.", result);
        }
    }
}