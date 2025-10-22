using DTO_QuanLyVatTu;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace GUI_QuanLyVatTu
{
    public class PDFExporter
    {
        public string XuatHoaDonPDF(InHoaDon hoaDon, string filePath)
        {
            try
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Font
                BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontTitle = new Font(baseFont, 18, Font.BOLD);
                Font fontHeader = new Font(baseFont, 12, Font.BOLD);
                Font fontNormal = new Font(baseFont, 11, Font.NORMAL);
                Font fontSmall = new Font(baseFont, 10, Font.NORMAL);

                // Tiêu đề công ty
                Paragraph company = new Paragraph("CÔNG TY TNHH VẬT TƯ ABC", fontTitle);
                company.Alignment = Element.ALIGN_CENTER;
                document.Add(company);

                Paragraph address = new Paragraph("Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM\nĐiện thoại: (028) 1234 5678 - Email: info@abccompany.com", fontSmall);
                address.Alignment = Element.ALIGN_CENTER;
                document.Add(address);

                document.Add(new Paragraph(" "));

                // Tiêu đề hóa đơn
                Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", fontTitle);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" "));

                // Thông tin hóa đơn
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 30, 70 });

                AddCell(infoTable, "Mã hóa đơn:", fontHeader);
                AddCell(infoTable, hoaDon.InHoaDonID, fontNormal);
                AddCell(infoTable, "Mã đơn hàng:", fontHeader);
                AddCell(infoTable, hoaDon.DonHangID, fontNormal);
                AddCell(infoTable, "Mã nhân viên:", fontHeader);
                AddCell(infoTable, hoaDon.NhanVienID, fontNormal);
                AddCell(infoTable, "Ngày in:", fontHeader);
                AddCell(infoTable, hoaDon.NgayIn.ToString("dd/MM/yyyy HH:mm"), fontNormal);
                AddCell(infoTable, "Trạng thái:", fontHeader);
                AddCell(infoTable, hoaDon.TrangThai, fontNormal);

                document.Add(infoTable);
                document.Add(new Paragraph(" "));

                // Bảng chi tiết vật tư
                PdfPTable detailTable = new PdfPTable(6);
                detailTable.WidthPercentage = 100;
                detailTable.SetWidths(new float[] { 5, 35, 15, 15, 15, 15 });

                // Header bảng
                AddCell(detailTable, "STT", fontHeader, true);
                AddCell(detailTable, "Tên vật tư", fontHeader, true);
                AddCell(detailTable, "Đơn vị tính", fontHeader, true);
                AddCell(detailTable, "Số lượng", fontHeader, true);
                AddCell(detailTable, "Đơn giá", fontHeader, true);
                AddCell(detailTable, "Thành tiền", fontHeader, true);

                // Dữ liệu mẫu (thay thế bằng dữ liệu thực tế từ database)
                AddCell(detailTable, "1", fontNormal);
                AddCell(detailTable, "Vật tư mẫu 1", fontNormal);
                AddCell(detailTable, "Cái", fontNormal);
                AddCell(detailTable, "2", fontNormal);
                AddCell(detailTable, "1,000,000", fontNormal);
                AddCell(detailTable, "2,000,000", fontNormal);

                AddCell(detailTable, "2", fontNormal);
                AddCell(detailTable, "Vật tư mẫu 2", fontNormal);
                AddCell(detailTable, "Hộp", fontNormal);
                AddCell(detailTable, "1", fontNormal);
                AddCell(detailTable, "500,000", fontNormal);
                AddCell(detailTable, "500,000", fontNormal);

                document.Add(detailTable);
                document.Add(new Paragraph(" "));

                // Tổng tiền
                PdfPTable totalTable = new PdfPTable(2);
                totalTable.WidthPercentage = 100;
                totalTable.SetWidths(new float[] { 70, 30 });

                AddCell(totalTable, "Tổng tiền:", fontHeader);
                AddCell(totalTable, hoaDon.TongTien.ToString("N0") + " VNĐ", fontHeader);

                document.Add(totalTable);
                document.Add(new Paragraph(" "));

                // Ghi chú
                if (!string.IsNullOrEmpty(hoaDon.GhiChu))
                {
                    Paragraph ghiChu = new Paragraph("Ghi chú: " + hoaDon.GhiChu, fontNormal);
                    document.Add(ghiChu);
                    document.Add(new Paragraph(" "));
                }

                // Chân trang
                Paragraph footer = new Paragraph("Cảm ơn quý khách! Hẹn gặp lại!", fontNormal);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);

                document.Close();
                return null; // Thành công
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void AddCell(PdfPTable table, string text, Font font, bool isHeader = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 5;
            if (isHeader)
            {
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            }
            cell.BorderWidth = 0.5f;
            table.AddCell(cell);
        }
    }
}