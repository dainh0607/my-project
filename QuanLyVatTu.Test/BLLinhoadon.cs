using BLL_QuanLyVatTu;
using BUS_QuanLyVatTu;
using DTO_QuanLyVatTu;  
namespace QuanLyVatTu.Test;

public class BLLinhoadon
{
    private BUS_InHoaDon bus;
    [SetUp]
    public void Setup()
    {
        bus = new BUS_InHoaDon();
    }

    [Test]
    public void CapNhat_HoaDonTonTai_ThanhCong()
    {
        var hd = new InHoaDon
        {
            InHoaDonID = "HD001",    
            DonHangID = "DH001",
            NhanVienID = "NV001",
            TongTien = 500000,
            NgayIn = DateTime.Now,
            TrangThai = "Đã in"
        };

        string result = bus.CapNhatHoaDon(hd);

        Assert.IsNull(result);
    }

    [Test]
    public void CapNhat_IDTrong_ThatBai()
    {
        var hd = new InHoaDon
        {
            InHoaDonID = "",
            DonHangID = "DH001",
            NhanVienID = "NV001",
            TongTien = 100000,
            TrangThai = "Đã in"
        };

        string result = bus.CapNhatHoaDon(hd);

        Assert.AreEqual("ID hóa đơn không được để trống", result);
    }



    [Test]
    public void Xoa_HoaDonDaIn()
    {
        string result = bus.XoaHoaDon("4");

        Assert.IsNotNull(result);
    }

    [Test]
    public void Xoa_HoaDonKhongTonTai_ThatBai()
    {
        string result = bus.XoaHoaDon("HD999");

        Assert.IsNotNull( result);
    }

  

    [Test]
    public void ThemHoaDon_HopLe_ThanhCong()
    {
        var hd = new InHoaDon
        {
          
            DonHangID = "DH002",
            NhanVienID = "NV001",
            TongTien = 300000,
            NgayIn = DateTime.Now,
            TrangThai = "Chờ in"
        };

        string result = bus.ThemHoaDon(hd);

        Assert.IsNull(result);
    }
}
