using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;

namespace Barcode_Scanner.Helper
{
    public class BillStockItemGeneratorModel : BillModel
    {
        public string attn { get; set; }
        public ICollection<Table> table { get; set; }
        public class Table
        {
            public string no { get; set; }
            public string orderNo { get; set; }
            public string customerName { get; set; }
        }
    }
    public class BillStockItemGenerator : BillGenerato<BillStockItemGeneratorModel>
    {
        public BillStockItemGenerator(BillStockItemGeneratorModel model) : base(model)
        {
            billName = "ใบคุม \"ใบฝากสินค้า\"";
            settingFooter = new SettingFooter()
            {
                hasRecieDate = true,
                hasReciver = true
            };
        }

        public override void CreateBody()
        {
            document.Add(new Paragraph("ATTN : " + _model.attn).SetBold());
            document.Add(new Paragraph("Remark : " + _model.remark).SetBold());
            Image checkBox = new Image(ImageDataFactory.Create(GetBytesFromFCheckbox()));
            document.Add(new Paragraph().Add(checkBox).Add(" Send  ").SetBold().SetFontSize(18));

            //table bill
            float[] width = {3, 3, 3 };
            var table = new Table(width);
            table.SetBold();
            table.SetWidthPercent(100);
            table.SetTextAlignment(TextAlignment.CENTER);
            //th
            Cell[] theads = {
                    new Cell().Add("No."),
                    new Cell().Add("Order No."),
                    new Cell().Add("Customer Name")
                };
            foreach (var t in theads)
            {
                table.AddCell(t);
            }
            //tr
            foreach (var item in _model.table)
            {
                table.AddCell(new Cell().Add(item.no));
                table.AddCell(new Cell().Add(item.orderNo));
                table.AddCell(new Cell().Add(item.customerName));
            }

            table.AddCell(new Cell(1, 3).Add("Total Order = จำนวน Order ที่ส่งเอกสารทั้งหมด"));
            document.Add(table);
        }
    }
}