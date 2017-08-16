using iText.IO.Image;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class CheckBillGeneratorModel:BillModel
    {
        public ICollection<Table> table { get; set; }
        public class Table
        {
            public string advanceReceiptNo { get; set; }
            public string receiptDate { get; set; }
            public string amount { get; set; }
            public string cur { get; set; }
        }
    }
    public class CheckBillGenerator : BillGenerato<CheckBillGeneratorModel>
    {
        public CheckBillGenerator(CheckBillGeneratorModel model) : base(model)
        {
            billName = "ใบคุมเอกสารรับเช็ค";
            settingFooter = new SettingFooter()
            {
                hasIssue = true,
                hasReciver = true,
                hasChequeNo=true,
                hasChuqueAmount=true,
                hasChuqueDate=true,
            };
        }

        public override void CreateBody()
        {
            //body//
            document.Add(new Paragraph("Customer name :  " + _model.customerName).SetBold());
            document.Add(new Paragraph("Customer code : " + _model.customerCode).SetBold());
            document.Add(new Paragraph("Remark : " + _model.remark).SetBold());

            //table invoice
            float[] width = { 3, 3, 3, 1 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("Advance receipt No."),
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
                table.AddCell(new Cell().Add(item.advanceReceiptNo));
                table.AddCell(new Cell().Add(item.receiptDate));
                table.AddCell(new Cell().Add(item.amount));
                table.AddCell(new Cell().Add(item.cur));
                totalAmount += double.Parse(item.amount);
            }

            table.AddCell(new Cell(1, 3).Add("Total Amount"));
            table.AddCell(new Cell().Add(totalAmount.ToString()).SetBorderBottom(new DoubleBorder(1)));
            table.AddCell(new Cell().SetBorderBottom(new DoubleBorder(1)));
            document.Add(table);
        }
    }
}