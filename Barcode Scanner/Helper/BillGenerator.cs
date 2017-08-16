using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Barcode_Scanner.Helper
{
    public abstract class BillGenerato<T> where T: BillModel
    {
        protected T _model { get; set; }
        protected PdfDocument pdf { get; set; }
        protected Document document { get; set; }
        protected SettingFooter settingFooter { get; set; }
        protected string billName = "";
        private PdfFont _font;
        protected PdfFont font
        {
            get
            {
                if (_font == null)
                {
                    _font = PdfFontFactory.CreateFont(Properties.Resources.THSarabunNew, PdfEncodings.IDENTITY_H, true);
                }
                return _font;
            }
        }

        private SolidBorder _noBorder;
        protected SolidBorder noBorder {
            get {
                if (_noBorder == null)
                {
                    _noBorder = new SolidBorder(iText.Kernel.Colors.Color.BLACK, 0, 0);
                }
                return _noBorder;
            }
        }
        
        protected BillGenerato(T model)
        {
            _model = model;
        }

        protected const int DEFAULT_FONT_SIZE = 13;
        //protected const string LOGO_PATH = @"c:\scg.jpg";
        //protected const string FONT_PATH = @"fonts\THSarabunNew.ttf";

        public abstract void CreateBody();

        public byte[] Create()
        {
            
            byte[] bytes;
            using (var st = new MemoryStream())
            {
                //ini
                pdf = new PdfDocument(new PdfWriter(st));
                document = new Document(pdf);
                document.SetFont(font);
                document.SetFontSize(DEFAULT_FONT_SIZE);
                //header
                CreateHeader();
                //body
                CreateBody();
                //footer
                Createfooter();
                //barcode
                var barCode = new iText.Layout.Element.Image(ImageDataFactory.Create(GenerateBarcode.Crate("sasdj123DASCAj231sf")));
                var p = new Paragraph().Add(barCode);
                document.Add(p);
                //Close document
                document.Close();
                bytes = st.ToArray();
            }
            return bytes;
        }

        protected void  CreateHeader()
        {
            float[] columnWidths = { 2, 3.5f, 4 };
           
            Table headTable = new Table(columnWidths);
            headTable.SetBold();
            headTable.SetWidthPercent(100);
            headTable.SetFixedLayout();

            //cell1
            var cell = new Cell();
            byte[] byteLogo;
            using(var ms = new MemoryStream())
            {
                Properties.Resources.SCG.Save(ms, ImageFormat.Png);
                byteLogo = ms.ToArray();
            }
            var logo = new iText.Layout.Element.Image(ImageDataFactory.Create(byteLogo));
            logo.SetWidth(100);
            cell.Add(logo);
            cell.SetBorder(noBorder);
            headTable.AddCell(cell);
            //cell2
            cell = new Cell();
            cell.Add(new Paragraph("SCG Performance Chemicals Co.,LTD."));
            cell.Add(new Paragraph("1 Siam Cement Road Bangsue"));
            cell.Add(new Paragraph("Bangkok 10800"));
            cell.SetFontSize(16);
            cell.SetBorder(noBorder);
            headTable.AddCell(cell);
            //cell3
            cell = new Cell();
            cell.Add(new Paragraph(billName));
            if (!String.IsNullOrEmpty(_model.method))
                cell.Add(new Paragraph("Method : " + _model.method));
            cell.Add(new Paragraph("Date : " + _model.date));
            cell.Add(new Paragraph("No." + _model.billNo));
            cell.SetBorder(noBorder);
            cell.SetTextAlignment(TextAlignment.RIGHT);
            headTable.AddCell(cell);

            document.Add(headTable);
        }

        protected byte[] GetBytesFromFCheckbox()
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
                        bytes=ms.ToArray();
                    }
                }
            }
            return bytes;
        }



        protected void Createfooter()
        {
            var footerTable = new Table(new float[] { 1, 1 });
            footerTable.SetWidthPercent(100);
            //left
            var cell = new Cell().SetVerticalAlignment(VerticalAlignment.BOTTOM).SetBorder(noBorder); ;
            if (settingFooter.hasIssue)
            {
                cell.Add(new Paragraph("Issue by : " + _model.issueBy));
                cell.Add(new Paragraph("Issue Date : " + _model.issueDate));
                cell.Add(new Paragraph("CCR-F-0081"));
            }
            footerTable.AddCell(cell);
            //right
            cell = new Cell().SetTextAlignment(TextAlignment.RIGHT).SetBorder(noBorder);
            cell.Add(new Paragraph("**Please sign and return to us**"));

            if (settingFooter.hasReciver)
                cell.Add(new Paragraph("Receiver : ...............").SetBold());

            if (settingFooter.hasRecieDate)
                cell.Add(new Paragraph("Receive Date : ...............").SetBold());

            if (settingFooter.hasPaymentDue)
                cell.Add(new Paragraph("Payment Due: ...............").SetBold());

            if(settingFooter.hasChequeNo)
                cell.Add(new Paragraph("Cheque No. : ...............").SetBold());

            if (settingFooter.hasChequeNo)
                cell.Add(new Paragraph("Cheque Date : ...............").SetBold());

            if (settingFooter.hasChequeNo)
                cell.Add(new Paragraph("Cheque Amount : ...............").SetBold());

            if (settingFooter.hasPaymentCheckbox)
            {
                var p = new Paragraph("Payment By : ").SetBold();
                // create checkbox image
                iText.Layout.Element.Image checkBox = new iText.Layout.Element.Image(ImageDataFactory.Create(GetBytesFromFCheckbox()));
                p.Add(checkBox);
                p.Add(" Tranfer ");
                p.Add(checkBox);
                p.Add(" Cheq");
                cell.Add(p);

                p = new Paragraph().SetBold();
                p.Add(checkBox);
                p.Add(" Other...............");
                cell.Add(p);
            }
            footerTable.AddCell(cell);

            document.Add(footerTable);
        }
    }
}