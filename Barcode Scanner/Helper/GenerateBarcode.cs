using Spire.Barcode;
using System.Drawing;
using System.IO;

namespace Barcode_Scanner.Helper
{
    public class GenerateBarcode
    {
        public static byte[] Crate(string data)
        {
            BarCodeGenerator generator = new BarCodeGenerator(new BarcodeSettings()
            {
                Type = BarCodeType.Code128,
                Data = data,
                ShowTextOnBottom = true
            });
            byte[] byteImage = null;
            using (var image = generator.GenerateImage())
            {
                //crop for remove licens
                Rectangle cropRect = new Rectangle(0, 22, 522, 78);
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                    using (var ms = new MemoryStream())
                    {
                        target.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byteImage = ms.ToArray();
                    }
                }
            }
            return byteImage;
        }
    }
}