using DTO_QuanLyVatTu;
using DAL_QuanLyVatTu;
namespace QuanLyVatTu.Test;

public class DALKhachHang
{
    private DAL_KhachHang dal;
    [SetUp]
    public void Setup()
    {
        dal = new DAL_KhachHang();
    }


    [Test]
    public void GenerateID_TaoMaMoiDungDinhDang()
    {
        string newID = dal.GenerateID();
        Assert.IsTrue(newID.StartsWith("KH"));
        Assert.AreEqual(5, newID.Length);
    }
    [Test]
    public void Update_khachHangKhongTonTai()
    {
        var kh = new KhachHang
        {
            KhachHangID = "KH999", // ID giả định không tồn tại
            HoTen = "Nguyễn Văn F",
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com",
            DiaChi = "Huế",
            NgayTao = DateTime.Now,
            GhiChu = "jahsha"
        };

        string result = dal.Update(kh);

        Assert.AreEqual("Không tìm thấy khách hàng.", result);
    }
    [Test]
    public void Update_KhachHangTonTai_ThanhCong()
    {
        var kh = new KhachHang
        {
            KhachHangID = "KH009", // PHẢI tồn tại trong DB
            HoTen = "Khách Hàng Đổi Tên",
            DiaChi = "Đà Nẵng",
            SoDienThoai = "0909090909",
            Email = "test@gmail.com",
            NgayTao = DateTime.Now,
            GhiChu = "Test Update"
        };

        string result = dal.Update(kh);

        Assert.AreEqual("Cập nhật thành công", result, "Update thành công nên phải trả về chuỗi xác nhận.");
    }
    [Test]
    public void Delete_KhachHangTonTai_ThanhCong()
    {
       
        string idToDelete = "KH057"; 
        string result = dal.Delete(idToDelete);

        Assert.AreEqual("Xóa thành công", result);
    }



}
