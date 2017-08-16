using iText.Kernel.Pdf;
using iText.Layout;
using System.IO;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font;
using System.Drawing;

namespace Barcode_Scanner.Helper
{
    public class BillEMSGenerator
    {
        public static byte[] Create()
        {
            byte[] bytes;
            using(var st = new MemoryStream())
            {
                var pdf = new PdfDocument(new PdfWriter(st));
                var document = new Document(pdf);
                PdfFont font = PdfFontFactory.CreateFont("c:/windows/fonts/THSarabunNew.ttf", PdfEncodings.IDENTITY_H, true);
                document.SetFont(font);
                document.SetBold();
                document.SetFontSize(18);
                document.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                var table = new Table(new float[] { 1,3,2,2,2,2});
                table.SetWidthPercent(100);
                //tr1
                table.AddCell(new Cell(1, 4).Add("รายงานลงรับไปรษณีย์ภัณฑ์").SetFontSize(22).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.Color.BLACK, 0, 0)));
                var cell = new Cell(1, 2).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                cell.SetFontSize(20);
                cell.Add(new Paragraph("No. E17-00001"));
                cell.Add(new Paragraph("วันที่ 5/15/2017"));
                cell.Add(new Paragraph("ใบฝากส่ง..................").SetFontSize(18));
                cell.SetBorderLeft(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.Color.BLACK, 0, 0));
                table.AddCell(cell);
                //tr2
                cell = new Cell(1, 6);
                var p = new Paragraph("ลงทะเบียน     ");
                iText.Layout.Element.Image checkBox = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(GetBytesFromFCheckbox()));
                p.Add(checkBox).Add("     ด่วนพิเศษ     ")
                    .Add(checkBox).Add("     ธุรกิจตอบรับ     ")
                    .Add(checkBox).Add("     พัศดุ     ")
                    .Add(checkBox).Add("     อื่นๆ.......");
                cell.Add(p);
                p = new Paragraph("บริษัท...............SCG Plastics..........................................................").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
                cell.Add(p.SetPaddingLeft(10));
                p = new Paragraph("รหัสบริษัท-หน่วยงาน.........0480-18020 (collection).......").Add("       รหัสค่าใช่จ่าย............541001............");
                cell.Add(p);
                table.AddCell(cell);

                //th
                cell = new Cell();
                p = new Paragraph("ลำดับที่");
                cell.Add(p);
                p = new Paragraph("No."); 
                cell.Add(p).SetFontSize(14);
                table.AddCell(cell);

                table.AddCell(new Cell().Add("นามผู้รับ").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.BOTTOM).SetFontSize(14));

                cell = new Cell().SetFontSize(14); 
                p = new Paragraph("ปลายทาง");
                cell.Add(p);
                p = new Paragraph("DESTINATION");
                cell.Add(p);
                table.AddCell(cell);

                cell = new Cell().SetFontSize(14);
                p = new Paragraph("เลขทะเบียนที่") ;
                cell.Add(p);
                p = new Paragraph("REGUSTER NO.");
                cell.Add(p);
                table.AddCell(cell);

                table.AddCell(new Cell().Add("ค่าไปรษณียากร	").SetFontSize(14));

                table.AddCell(new Cell().Add("หมายเหตุ").SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.BOTTOM).SetFontSize(14));

                string[] company = {
                    "หาดใหญ่","ซี.พี.สหอุตสาหกรรม","มิตซุยไฮยีน แมททีเรียลส์","รวมสยามธุรกิจ"
                    ,"แพนเอเซีย","ไทยอินซานเทค","สยามพัฒนพงศ์","เวิลด์ปิกเม้นท์","ไฮแกสเค็ท พลาสติก",
                    "ไทยเอฟเวอร์ พลาสติก","ฟู่ อี้ เอ็นเตอร์ไพรส์","แอนวิล","ยูบิส","ซันสตาร์","ออมโนวา","ยูไนเต็ด แพ็ค",
                    "วีเอสแอล","ริเก้น","นิโซ","เพ็ทฟอร์ม","ซันชายน์","โคนี ซันไรส์","ไดอิชิ แพคเกจจิ้ง","โพลี พลาส",
                    "เดลี่กุ๊กแวร์","ไทรเดอร์","เอ็นเอส-สยามยูไนเต็ดสตีล","เนชั่นแนลสตาร์ชแอนด์เคมิเคิล (ระยอง)","ไทย พี เอ ซี อินดัสตรี",
                    "ไทยวินิเทค"
                };
                for(int i = 1; i <= company.Length; i++)
                {
                    for (int r = 0; r < 6; r++)
                    {
                        if (r == 0)
                        {
                            cell = new Cell().Add(i.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        }else if (r == 1)
                        {
                            cell = new Cell().Add(company[i-1]).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
                        }
                        else
                        {
                            cell = new Cell();
                        }
                        table.AddCell(cell);
                    }
                }
                table.AddCell(new Cell());
                table.AddCell(new Cell());
                table.AddCell(new Cell());
                table.AddCell(new Cell().Add("รวมเป็นเงิน"));
                table.AddCell(new Cell());
                table.AddCell(new Cell());
                //tr
                cell = new Cell(1, 6).SetFontSize(14).SetBorderTop(new iText.Layout.Borders.DoubleBorder(2));
                p = new Paragraph("หมายเหตุ : กรุณากรอกเลข Tracking และส่งกลับที่หน่วยงาน Domestic Collection  อาคาร 100 ปี ชั้น 2");
                cell.Add(p);
                p = new Paragraph("เจ้าพนักงานผู้รับ...................................").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                cell.Add(p);
                p = new Paragraph("ผู้ฝากส่ง...................................").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                cell.Add(p);
                table.AddCell(cell);
                document.Add(table);

                document.Close();
                bytes = st.ToArray();
            }
            return bytes;
            
        }

        private static byte[] GetBytesFromFCheckbox()
        {
            byte[] bytes;
            using (Bitmap img = new Bitmap(10, 10))
            {
                using (Graphics drawing = Graphics.FromImage(img))
                {
                    drawing.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, 10, 10));
                    Pen blackPen = new Pen(Color.Black, 3);
                    Rectangle rect = new Rectangle(0, 0, 10, 10);
                    drawing.DrawRectangle(blackPen, rect);
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        bytes = ms.ToArray();
                    }
                }
            }
            return bytes;
        }
    }
}