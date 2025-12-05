using BLL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace QuanLyVatTu.Test;

public class BllDonHang
{
    private BUSDonHang bus;
    [SetUp]
    public void Setup()
    {
        bus = new BUSDonHang();
    }

    [Test]
    public void ThemDonHang_KhachHangIDRong_TraVeLoi()
    {
        var dh = new DonHang { KhachHangID = "", NhanVienID = "NV001" };
        var result = bus.Add(dh);
        Assert.AreEqual("Khách hàng không được để trống.", result);
    }

    // Kiểm tra thêm đơn hàng khi thiếu mã nhân viên
    [Test]
    public void ThemDonHang_NhanVienIDRong_TraVeLoi()
    {
        var dh = new DonHang { KhachHangID = "KH001", NhanVienID = "" };
        var result = bus.Add(dh);
        Assert.AreEqual("Nhân viên không được để trống.", result);
    }

    // Kiểm tra thêm đơn hàng với dữ liệu hợp lệ
    [Test]
    public void ThemDonHang_DuLieuHopLe_ThanhCong()
    {
        var dh = new DonHang
        {
            DonHangID = bus.GenerateID(),
            KhachHangID = "KH003",
            NhanVienID = "NV001",
            NgayDat = DateTime.Now,
            TrangThai = "Chưa xử lí",
            GhiChu = "Test đơn hàng"
        };

        var result = bus.Add(dh);
        Assert.IsNull(result);
    }

    // Kiểm tra cập nhật đơn hàng khi thiếu mã đơn hàng
    [Test]
    public void CapNhatDonHang_MaDonHangRong_TraVeLoi()
    {
        var dh = new DonHang { DonHangID = "", KhachHangID = "KH001", NhanVienID = "NV001" };
        var result = bus.Update(dh);
        Assert.AreEqual("Mã đơn hàng không hợp lệ.", result);
    }

    // Kiểm tra xóa đơn hàng khi thiếu mã đơn hàng
    [Test]
    public void XoaDonHang_MaDonHangRong_TraVeLoi()
    {
        var result = bus.Delete("");
        Assert.AreEqual("Mã đơn hàng không hợp lệ.", result);
    }

    // Kiểm tra lấy danh sách đơn hàng
    [Test]
    public void LayDanhSachDonHang_TraVeList()
    {
        var result = bus.GetAll();
        Assert.IsInstanceOf<List<DonHang>>(result);
        Assert.GreaterOrEqual(result.Count, 0);
    }

    // Kiểm tra lấy đơn hàng theo mã
    [Test]
    public void LayDonHangTheoID_TraVeDonHang()
    {
        var result = bus.GetByID("DH001"); // giả sử DH001 tồn tại trong DB test
        Assert.IsNotNull(result);
        Assert.AreEqual("DH001", result.DonHangID);
    }

    // Kiểm tra sinh mã đơn hàng tự động
    [Test]
    public void SinhMaDonHang_TraVeChuoiKhongRong()
    {
        var id = bus.GenerateID();
        Assert.IsNotEmpty(id);
        Assert.That(id, Does.StartWith("DH"));
    }
}
