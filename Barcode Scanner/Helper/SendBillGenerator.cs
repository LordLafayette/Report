using iText.IO.Image;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class SendBillGeneratorModel : BillModel
    {
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

    public class SendBillGenerator : BillGenerato<SendBillGeneratorModel>
    {
        public SendBillGenerator(SendBillGeneratorModel model) : base(model)
        {
            billName = "ใบนำส่งใบเสร็จรับเงิน";
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
            document.Add(new Paragraph("Remark : " + _model.remark).SetBold());

            Image checkBox = new Image(ImageDataFactory.Create(GetBytesFromFCheckbox()));
            document.Add(new Paragraph().Add(checkBox).Add(" Send").SetBold().SetFontSize(18));

            //table invoice
            float[] width = { 3, 3, 3, 3, 1 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("Receipt No."),
                    new Cell().Add("Document No."),
                    new Cell().Add("Receipt Date"),
                    new Cell().Add("Amount"),
                    new Cell().Add("Cur.")
                };
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            double totalAmount = 0;
            foreach (var item in _model.table)
            {
                table.AddCell(new Cell().Add(item.receiptNo));
                table.AddCell(new Cell().Add(item.documentNo));
                table.AddCell(new Cell().Add(item.receiptDate));
                table.AddCell(new Cell().Add(item.amount));
                table.AddCell(new Cell().Add(item.cur));
                totalAmount += double.Parse(item.amount);
            }

            table.AddCell(new Cell(1, 4).Add("Total Amount"));
            table.AddCell(new Cell().Add(totalAmount.ToString()).SetBorderBottom(new DoubleBorder(1)));
            table.AddCell(new Cell().SetBorderBottom(new DoubleBorder(1)));
            document.Add(table);
        }
    }
}