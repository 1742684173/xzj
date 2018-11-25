using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xzj.utils
{
    class PDFHelpler
    {

        public static void imageToPDF(Bitmap[] bms, string fn)
        {
            // System.Drawing.Image image = System.Drawing.Image.FromFile(@"C:\Users\Administrator\Desktop\e.jpg");
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(fn, FileMode.Create));

            doc.Open();
            // iTextSharp.text.Image.GetInstance();
            for (int i = 0; i < bms.Length; i++) {
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(bms[i], System.Drawing.Imaging.ImageFormat.Bmp);
                pdfImage.ScalePercent(50);
                doc.Add(pdfImage);
            }
            doc.Close();
        }

        public static bool ToPdf(DataTable dt, string fn)
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(fn, FileMode.Create));
            document.Open();
            BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 10, iTextSharp.text.Font.NORMAL);


            PdfPTable table = new PdfPTable(dt.Columns.Count);
            // for (int m = 0; m < 10; m++)
            // {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt.Columns[j].ToString(), fontChinese));
                       
                    }
                }
                
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        table.AddCell(new Phrase(dt.Rows[i][j].ToString(), fontChinese));
                    }
                
            }
            // }
            
            document.Add(table);
            document.Close();
            return true;
        }
    }
}
