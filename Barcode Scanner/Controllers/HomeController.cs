using Barcode_Scanner.Helper;
using ceTe.DynamicPDF.Rasterizer;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using Spire.Barcode;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Barcode_Scanner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength < 1)
            {
                return View((object)"Plase upload some file");
            }
            System.Drawing.Image image = null;
            if (file.ContentType.ToLower().Contains("pdf"))
            {
                using(PdfRasterizer rasterizer = new PdfRasterizer(new InputPdf(file.InputStream)))
                {
                    image = new Bitmap(new MemoryStream(rasterizer.Draw(ImageFormat.Png, ImageSize.Dpi72)[0]));
                }
            }
            else
            {
                image = new Bitmap(file.InputStream);
            }
            
            string[] data = BarcodeScanner.Scan((Bitmap)image);
            image.Dispose();

            return View((object)data.FirstOrDefault() );
        }

        public FileResult CreatePdf(int id)
        {
            var master = new BillModel()
            {
                billNo = "O17-0230-00002",
                customerCode = "20002",
                customerName = "จีรวัตร ชูอรรถ",
                date = "8/16/2017",
                issueBy = "Lafayette",
                issueDate = "7/8/2018",
                method = "POST"
            };
            var serializedParent = JsonConvert.SerializeObject(master);

            byte[] bytes= null;
            switch (id){
                case 1:
                    Type1BillGenerateModel model = JsonConvert.DeserializeObject<Type1BillGenerateModel>(serializedParent);
                    model.table = new List<Type1BillGenerateModel.Table>(10);
                    bytes = new Type1BillGenerate(model).Create();
                    break;
                case 2:
                    CheckBillGeneratorModel model1 = JsonConvert.DeserializeObject<CheckBillGeneratorModel>(serializedParent);
                    model1.table = new List<CheckBillGeneratorModel.Table>(10);
                    bytes = new CheckBillGenerator(model1).Create();
                    break;
                case 3:
                    SendBillGeneratorModel model2 = JsonConvert.DeserializeObject<SendBillGeneratorModel>(serializedParent);
                    model2.table = new List<SendBillGeneratorModel.Table>(10);
                    bytes = new SendBillGenerator(model2).Create();
                    break;
                case 4:
                    SentBillSignalGeneratorModel model3 = JsonConvert.DeserializeObject<SentBillSignalGeneratorModel>(serializedParent);
                    model3.table = new List<SentBillSignalGeneratorModel.Table>(10);
                    bytes = new SentBillSignalGenerator(model3).Create();
                    break;
                case 5:
                    SentBillItemStockGeneratorModel model4 = JsonConvert.DeserializeObject<SentBillItemStockGeneratorModel>(serializedParent);
                    model4.table = new List<SentBillItemStockGeneratorModel.Table>(10);
                    bytes = new SentBillItemStockGenerator(model4).Create();
                    break;
                case 6:
                    SentBillOtherGeneratorModel model5 = JsonConvert.DeserializeObject<SentBillOtherGeneratorModel>(serializedParent);
                    model5.sentReciveTable = new List<SentBillOtherGeneratorModel.SentReciveTable>(5);
                    model5.sentTable = new List<SentBillOtherGeneratorModel.SentTable>(5);
                    model5.reciveTable = new List<SentBillOtherGeneratorModel.ReciveTable>(5);
                    bytes = new SentBillOtherGenerator(model5).Create();
                    break;
                case 7:
                    BillStockItemGeneratorModel model6 = (BillStockItemGeneratorModel)master;
                    model6.table = new List<BillStockItemGeneratorModel.Table>(10);
                    bytes = new BillStockItemGenerator(model6).Create();
                    break;
                case 8:
                    bytes = BillEMSGenerator.Create();
                    break;
            }
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet,id.ToString()+".pdf");
        }
    }
}