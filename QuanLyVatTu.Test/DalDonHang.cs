using DAL_QuanLyVatTu;
using DTO_QuanLyVatTu;

namespace QuanLyVatTu.Test;

public class DalDonHang
{
    private DAL_DonHang dal;
    [SetUp]
    public void Setup()
    {
        dal = new DAL_DonHang();
    }

    [Test]
    // Kiểm tra lấy tất cả đơn hàng
    
    public void SelectAll_TraVeDanhSachDonHang()
    {
        var list = dal.SelectAll();
        
        Assert.IsInstanceOf<List<DonHang>>(list);
        Assert.GreaterOrEqual(list.Count, 0); // có thể rỗng nếu DB chưa có dữ liệu
    }

    // Kiểm tra thêm đơn hàng mới
    [Test]
    public void Insert_ThemDonHangHopLe_KhongLoi()
    {
        var dh = new DonHang
        {
            DonHangID = dal.GenerateID(),
            KhachHangID = "KH001",   // giả sử KH001 tồn tại
            NhanVienID = "NV001",   // giả sử NV001 tồn tại
            NgayDat = DateTime.Now,
            TrangThai = "Chưa xử lí",
            GhiChu = "Đơn hàng test"
        };

        var result = dal.Insert(dh);
        Assert.IsNull(result); // null nghĩa là không có lỗi
    }

    // Kiểm tra cập nhật đơn hàng
    [Test]
    public void Update_CapNhatDonHangHopLe_KhongLoi()
    {
        var dh = dal.GetByID("DH001"); // giả sử DH001 tồn tại trong DB test
        Assert.IsNotNull(dh);

        dh.TrangThai = "Đã giao";
        var result = dal.Update(dh);

        Assert.IsNull(result);
        var updated = dal.GetByID("DH001");
        Assert.AreEqual("Đã giao", updated.TrangThai);
    }

    // Kiểm tra xóa đơn hàng
    [Test]
    public void Delete_XoaDonHangHopLe_KhongLoi()
    {
        var id = dal.GenerateID();
        var dh = new DonHang
        {
            DonHangID = id,
            KhachHangID = "KH001",
            NhanVienID = "NV001",
            NgayDat = DateTime.Now,
            TrangThai = "Chưa xử lí",
            GhiChu = "Đơn hàng để test xóa"
        };
        dal.Insert(dh);

        var result = dal.Delete(id);
        Assert.IsNull(result);

        var deleted = dal.GetByID(id);
        Assert.IsNull(deleted);
    }

    // Kiểm tra sinh mã đơn hàng tự động
    [Test]
    public void GenerateID_TraVeMaMoiBatDauBangDH()
    {
        var id = dal.GenerateID();
        Assert.IsNotEmpty(id);
        Assert.That(id, Does.StartWith("DH"));
    }

    // Kiểm tra lấy đơn hàng theo ID
    [Test]
    public void GetByID_TraVeDonHangKhiTonTai()
    {
        var dh = dal.GetByID("DH001"); // giả sử DH001 tồn tại
        Assert.IsNotNull(dh);
        Assert.AreEqual("DH001", dh.DonHangID);
    }
}
