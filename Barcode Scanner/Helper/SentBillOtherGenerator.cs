using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class SentBillOtherGeneratorModel : BillModel
    {
        public string contactPerson { get; set; }
        public string phone { get; set; }
        public ICollection<SentTable> sentTable { get; set; }
        public ICollection<ReciveTable> reciveTable { get; set; }
        public ICollection<SentReciveTable> sentReciveTable { get; set; }
        public class SentTable
        {
            public string no { get; set; }
            public string detail { get; set; }
        }
        public class ReciveTable
        {
            public string no { get; set; }
            public string detail { get; set; }
        }
        public class SentReciveTable
        {
            public string no { get; set; }
            public string detail { get; set; }
        }
    }
    public class SentBillOtherGenerator : BillGenerato<SentBillOtherGeneratorModel>
    {
        public SentBillOtherGenerator(SentBillOtherGeneratorModel model) : base(model)
        {
            billName = "ใบนำส่งเอกสารอื่นๆ";
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

            //--------------table sent
            float[] width = { 1,1};
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("No."),
                    new Cell().Add("Document Detail")
                };
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            foreach (var item in _model.sentTable)
            {
                table.AddCell(new Cell().Add(item.no));
                table.AddCell(new Cell().Add(item.detail));
            }

            table.AddCell(new Cell(1, 3).Add("Total Document  = จำนวนเอกสารที่ส่งให้ลูกค้า"));
            document.Add(table);

            //table----------------recive
            document.Add(new Paragraph().Add(checkBox).Add(" Receive  ").SetBold().SetFontSize(18));
            table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            foreach (var item in _model.reciveTable)
            {
                table.AddCell(new Cell().Add(item.no));
                table.AddCell(new Cell().Add(item.detail));
            }

            table.AddCell(new Cell(1, 3).Add("Total Document  = จำนวนเอกสารที่รับกลับ"));
            document.Add(table);

            //table----------------recive/send
            document.Add(new Paragraph().Add(checkBox).Add(" Send/Receive  ").SetBold().SetFontSize(18));
            table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            foreach (var item in _model.sentReciveTable)
            {
                table.AddCell(new Cell().Add(item.no));
                table.AddCell(new Cell().Add(item.detail));
            }

            table.AddCell(new Cell(1, 3).Add("Total Document  = จำนวนเอกสารที่ส่งและรับกลับ"));
            document.Add(table);
        }
    }
}