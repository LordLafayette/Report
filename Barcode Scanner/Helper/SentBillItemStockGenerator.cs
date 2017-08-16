using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class SentBillItemStockGeneratorModel : BillModel
    {
        public string contactPerson { get; set; }
        public string phone { get; set; }
        public ICollection<Table> table { get; set; }
        public class Table
        {
            public string no { get; set; }
            public string dpNo { get; set; }
            public string invoiceNo { get; set; }
            public string invoiceDate { get; set; }
        }
    }

    public class SentBillItemStockGenerator : BillGenerato<SentBillItemStockGeneratorModel>
    {
        public SentBillItemStockGenerator(SentBillItemStockGeneratorModel model) : base(model)
        {
            billName = "ใบนำส่งใบส่งคืนสินค้าฝาก";
            settingFooter = new SettingFooter()
            {
                hasIssue = true,
                hasRecieDate = true,
                hasReciver = true
            };
        }

        public override void CreateBody()
        {
            document.Add(new Paragraph("Customer name :  " + _model.customerName).SetBold());
            document.Add(new Paragraph("Customer code : " + _model.customerCode).SetBold());

            var contactName = new Text("Contact Person : " + _model.contactPerson).SetTextAlignment(TextAlignment.RIGHT);
            var phone = new Text("Telephone : " + _model.phone).SetTextAlignment(TextAlignment.RIGHT);
            document.Add(new Paragraph(contactName).Add(phone).SetBold());

            document.Add(new Paragraph("Remark : " + _model.remark).SetBold());
            Image checkBox = new Image(ImageDataFactory.Create(GetBytesFromFCheckbox()));
            document.Add(new Paragraph().Add(checkBox).Add(" Send  ").SetBold().SetFontSize(18));

            //table invoice
            float[] width = { 1,3, 3, 3 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("No."),
                    new Cell().Add("DP No."),
                    new Cell().Add("Invoice No."),
                    new Cell().Add("Invoice Date")
                };
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            foreach (var item in _model.table)
            {
                table.AddCell(new Cell().Add(item.no));
                table.AddCell(new Cell().Add(item.dpNo));
                table.AddCell(new Cell().Add(item.invoiceNo));
                table.AddCell(new Cell().Add(item.invoiceDate));
            }

            table.AddCell(new Cell(1, 3).Add("Total DP = จำนวน DP ที่ส่งให้ลูกค้า"));
            document.Add(table);
        }
    }
}