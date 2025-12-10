using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace QuanLyVatTu.Test;

public class buskhachhang
{
    private BUSKhachHang bus;

    [SetUp]
    public void Setup()
    {
        bus = new BUSKhachHang();
    }

    // ====== TEST THÊM ======
    [Test]
    public void Add_HopLe()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn A",
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com",
            DiaChi = "Hồ Chí Minh",
            NgayTao = DateTime.Now,
            GhiChu = "Khách hàng mới"
        };

        string result = bus.Add(kh);

        Assert.AreEqual(string.Empty, result); // thành công => trả về ""
    }

    [Test]
    public void Add_ThieuThongTin()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "", // thiếu tên
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com",
            DiaChi = "Hồ Chí Minh",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Vui lòng điền đầy đủ thông tin.", result);
    }

    [Test]
    public void Add_TenKhongHopLe()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyen123", // sai định dạng
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com",
            DiaChi = "Hà Nội",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Tên người dùng chỉ bao gồm chữ.", result);
    }

    [Test]
    public void Add_SoDienThoaiKhongHopLe()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn B",
            SoDienThoai = "09abc12345", // chứa chữ
            Email = "abc@gmail.com",
            DiaChi = "Hà Nội",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Số điện thoại chỉ bao gồm số.", result);
    }

    [Test]
    public void Add_SoDienThoaiQuaNgan()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn C",
            SoDienThoai = "09876", // quá ngắn
            Email = "abc@gmail.com",
            DiaChi = "Đà Nẵng",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Số điện thoại phải có đủ 10 số.", result);
    }
    public void Add_SoDienThoaiQuaDai()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn C",
            SoDienThoai = "098764324323452", // quá dài
            Email = "abc@gmail.com",
            DiaChi = "Đà Nẵng",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Số điện thoại đã vượt quá 10 số.", result);
    }

    [Test]
    public void Add_EmailSaiDinhDang()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn D",
            SoDienThoai = "0987654321",
            Email = "abc@yahoo.com", // sai định dạng
            DiaChi = "Hà Nội",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Add(kh);

        Assert.AreEqual("Email phải có đuôi @gmail.com.", result);
    }

    // ====== TEST UPDATE ======
    [Test]
    public void Update_HopLe()
    {
        var kh = new KhachHang
        {
            KhachHangID = bus.GenerateID(),
            HoTen = "Nguyễn Văn E",
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com",
            DiaChi = "Huế",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Update(kh);

        Assert.AreEqual(string.Empty, result); // thành công => ""
    }

 
    [Test]
    public void Update_khachHangKhongTonTai()
    {
        var kh = new KhachHang
        {
            KhachHangID = "KH999",
            HoTen = "Nguyễn Văn F",
            SoDienThoai = "0987654321",
            Email = "abc@gmail.com", 
            DiaChi = "Huế",
            NgayTao = DateTime.Now,
            GhiChu = ""
        };

        string result = bus.Update(kh);

        Assert.AreEqual("Không tìm thấy khách h cần sửa.", result);
    }

    // ====== TEST DELETE ======
    [Test]
    public void Delete_HopLe()
    {
        string id = bus.GenerateID();
        string result = bus.Delete(id);

        // tuỳ DAL.Delete bạn viết thế nào, giả sử trả về "" khi thành công
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Delete_KhongTonTai()
    {
        string id = "KH999999"; // giả sử không tồn tại
        string result = bus.Delete(id);

        // tuỳ DAL.Delete bạn viết thế nào, giả sử trả về "Không tìm thấy"
        Assert.AreEqual("Không tìm thấy", result);
    }


}



