using iText.IO.Image;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class Type1BillGenerateModel : BillModel
    {
        public ICollection<Table> table { get; set; }
        public class Table
        {
            public string invoiceNo { get; set; }
            public string invoiceDate { get; set; }
            public string paymentTerm { get; set; }
            public string dueDate { get; set; }
            public string amount { get; set; }
            public string cur { get; set; }
        }
    }

    public class Type1BillGenerate : BillGenerato<Type1BillGenerateModel>
    {

        public Type1BillGenerate(Type1BillGenerateModel model) : base(model)
        {
            billName = "ใบคุมการส่งเอกสารวางบิล";
            settingFooter = new SettingFooter()
            {
                hasIssue = true,
                hasPaymentCheckbox = true,
                hasPaymentDue = true,
                hasRecieDate = true,
                hasReciver = true
            };
        }

        public override void CreateBody()
        {
            document.Add(new Paragraph("Customer name :  " + _model.customerName).SetBold());
            document.Add(new Paragraph("Customer code : " + _model.customerCode).SetBold());
            document.Add(new Paragraph("Remark : " + _model.remark).SetBold());
            document.Add(new Paragraph("Invoice").SetBold());

            //table invoice
            float[] width = { 3, 3, 3, 3, 3, 1 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("Invoice No."),
                    new Cell().Add("Invoice Date"),
                    new Cell().Add("Payment term"),
                    new Cell().Add("Due Date"),
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
                table.AddCell(new Cell().Add(item.invoiceNo));
                table.AddCell(new Cell().Add(item.invoiceDate));
                table.AddCell(new Cell().Add(item.paymentTerm));
                table.AddCell(new Cell().Add(item.dueDate));
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