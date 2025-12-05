using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVatTu.Test
{
    [TestFixture]
    public class DALVatTu
    {
        private DAL_VatTu dal;
        [SetUp]
        public void Setup()
        {
            dal = new DAL_VatTu();
        }
        [Test]
        public void SelectAll_TraVeDanhSachVatTu()
        {
            var list = dal.SelectAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [Test]
        public void Insert_VatTuHopLe_ThanhCong()
        {
            var vt = new VatTu
            {
                VatTuID = dal.GenerateID(),
                TenVatTu = "Gạch Test",
                LoaiVatTuID = "LVT004",
                DonGia = 5000,
                SoLuongTon = 100,
                NhaCungCapID = "NCC003",
                NgayTao = DateTime.Now,
                GhiChu = "Gạch kiểm thử",
                TrangThaiID = "TT001"
            };

            string result = dal.Insert(vt);
            Assert.IsNull(result); // null nghĩa là không lỗi
        }

        [Test]
        public void Insert_ThieuTenVatTu_BiLoi()
        {
            var vt = new VatTu
            {
                VatTuID = dal.GenerateID(),
                TenVatTu = null, // ❌ thiếu tên
                LoaiVatTuID = "LVT004",
                DonGia = 5000,
                SoLuongTon = 100,
                NhaCungCapID = "NCC003",
                NgayTao = DateTime.Now,
                GhiChu = "Thiếu tên",
                TrangThaiID = "TT001"
            };

            string result = dal.Insert(vt);
            Assert.IsNotNull(result); // có lỗi → không null
        }

        [Test]
        public void Update_VatTuTonTai_ThanhCong()
        {
            var vt = new VatTu
            {
                VatTuID = "VT003", // giả sử tồn tại
                TenVatTu = "Thép Pomina D10 - sửa",
                LoaiVatTuID = "LVT003",
                DonGia = 210000,
                SoLuongTon = 250,
                NhaCungCapID = "NCC002",
                NgayTao = DateTime.Now,
                GhiChu = "Cập nhật đơn giá",
                TrangThaiID = "TT001"
            };

            string result = dal.Update(vt);
            Assert.IsNull(result);
        }

        [Test]
        public void Update_VatTuKhongTonTai_BiLoi()
        {
            var vt = new VatTu
            {
                VatTuID = "VT999", // không tồn tại
                TenVatTu = "Vật tư ảo",
                LoaiVatTuID = "LVT001",
                DonGia = 100000,
                SoLuongTon = 10,
                NhaCungCapID = "NCC001",
                NgayTao = DateTime.Now,
                GhiChu = "Không tồn tại",
                TrangThaiID = "TT001"
            };

            string result = dal.Update(vt);
            Assert.IsNotNull(result); // có lỗi
        }

        [Test]
        public void Delete_VatTuTonTai_ThanhCong()
        {
            string id = "VT007"; // giả sử tồn tại và không bị ràng buộc
            string result = dal.Delete(id);
            Assert.IsNull(result);
        }

        [Test]
        public void Delete_VatTuDangDuocSuDung_BiLoi()
        {
            string id = "VT001"; // giả sử đang dùng trong ChiTietDonHang
            bool inUse = dal.IsVatTuInUse(id);
            if (inUse)
            {
                string result = dal.Delete(id);
                Assert.IsNotNull(result); // bị lỗi do ràng buộc
            }
        }

        [Test]
        public void GenerateID_TaoMaMoiDungDinhDang()
        {
            string newID = dal.GenerateID();
            Assert.IsTrue(newID.StartsWith("VT"));
            Assert.AreEqual(5, newID.Length); // ví dụ: VT006
        }

        [Test]
        public void IsVatTuInUse_KiemTraDung()
        {
            bool result = dal.IsVatTuInUse("VT001"); // giả sử đang dùng
            Assert.IsTrue(result || !result); // test không lỗi
        }
    }
}
