using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVatTu.Test
{
    public class DALLoaiVatTu
    {
        private DAL_LoaiVatTu dal;
        [SetUp]
        public void Setup() 
        {
            dal = new DAL_LoaiVatTu();
        }

        [Test]
        public void SelectAll_PhaiTraVeDanhSach()
        {
            var result = dal.SelectAll();
            Assert.IsNotNull(result, "Danh sách không được null");
            Assert.IsInstanceOf<List<LoaiVatTu>>(result, "Phải là danh sách LoaiVatTu");
            Assert.IsTrue(result.Count > 0, "Danh sách phải có ít nhất 1 phần tử");
        }

        [Test]
        public void GenerateID_PhaiTraVeMaMoiDungDinhDang()
        {
            string id = dal.GenerateID();
            Assert.IsTrue(id.StartsWith("LVT"), "Mã phải bắt đầu bằng 'LVT'");
            Assert.AreEqual(6, id.Length, "Mã phải có 6 ký tự");
        }

        [Test]
        public void Insert_And_Delete_PhaiThanhCong()
        {
            var loai = new LoaiVatTu
            {
                LoaiVatTuID = dal.GenerateID(),
                TenLoaiVatTu = "Vật tư test DAL",
                NgayTao = DateTime.Now,
                GhiChu = "Test DAL"
            };

            string insertResult = dal.Insert(loai);
            Assert.AreEqual("Success", insertResult, "Thêm vật tư phải thành công");

            string deleteResult = dal.Delete(loai.LoaiVatTuID);
            Assert.AreEqual("Success", deleteResult, "Xóa vật tư phải thành công");
        }

        [Test]
        public void Update_PhaiThanhCong()
        {
            var loai = new LoaiVatTu
            {
                LoaiVatTuID = "LVT001", // giả định tồn tại
                TenLoaiVatTu = "Xi măng cập nhật",
                NgayTao = DateTime.Now,
                GhiChu = "Cập nhật từ DAL"
            };

            string result = dal.Update(loai);
            Assert.AreEqual("Success", result, "Cập nhật phải thành công");
        }

        [Test]
        public void SelectBySql_TimKiem_PhaiTraVeKetQua()
        {
            string sql = "SELECT * FROM LoaiVatTu WHERE TenLoaiVatTu LIKE @0";
            var args = new List<object> { "%Gạch%" };
            var result = dal.SelectBySql(sql, args, System.Data.CommandType.Text);

            Assert.IsNotNull(result, "Kết quả không được null");
            Assert.IsTrue(result.Exists(x => x.TenLoaiVatTu.Contains("Gạch")), "Phải có kết quả chứa từ 'Gạch'");
        }
    }
}
