using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class SentBillSignalGeneratorModel : BillModel
    {
        public string contactPerson { get; set; }
        public string phone { get; set; }
        public ICollection<Table> table { get; set; }
        public class Table
        {
            public string receiptNo { get; set; }
            public string documentNo { get; set; }
            public string receiptDate { get; set; }
            public string amount { get; set; }
            public string cur { get; set; }
        }
    }

    public class SentBillSignalGenerator : BillGenerato<SentBillSignalGeneratorModel>
    {
        public SentBillSignalGenerator(SentBillSignalGeneratorModel model) : base(model)
        {
            billName = "ใบนำส่งเอกสารเซ็นบิล";
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

            var contactTable = new Table(new float[] { 1, 1 });
            contactTable.SetWidthPercent(100);
            var noborder = new SolidBorder(Color.BLACK, 0, 0);
            var cell = new Cell().Add("Contact Person : " + _model.contactPerson).SetBorder(noBorder);
            contactTable.AddCell(cell).SetBold();
            cell = new Cell().Add("Telephone : " + _model.phone).SetBorder(noBorder);
            contactTable.AddCell(cell).SetBold();
            document.Add(contactTable);

            var p = new Paragraph("Remark : " + _model.remark).SetBold();
            document.Add(p);
            p = new Paragraph().SetPaddingLeft(20);
            Image checkBox = new Image(ImageDataFactory.Create(GetBytesFromFCheckbox()));
            p.Add(checkBox).Add(" Send  ");
            p.Add(checkBox).Add(" Receive  ");
            p.Add(checkBox).Add(" Send/Receive").SetFontSize(18);
            document.Add(p);
            //table invoice
            float[] width = { 1,3,3 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("No."),
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
                table.AddCell(new Cell().Add(item.receiptNo));
                table.AddCell(new Cell().Add(item.documentNo));
                table.AddCell(new Cell().Add(item.receiptDate));
            }

            table.AddCell(new Cell(1, 3).Add("Total Invoice = จำนวน Invoice ที่ไปเซ็นบิล"));
            document.Add(table);
        }
    }
}