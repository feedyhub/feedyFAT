using PdfSharp.Pdf;
using System.IO;
using System.IO.Packaging;
using System.Windows.Controls;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using PdfSharp.Xps;


namespace FeedyWPF.Models
{
    class PdfExport
    {
        public PdfExport(Page page)
        {
            PdfDocument d = new PdfDocument();
            

            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            var doc = new XpsDocument(package);
            
            //XpsDocument xpsd = new XpsDocument("iksPS", FileAccess.ReadWrite);
            //System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            //xw.Write(doc.GetFixedDocumentSequence());
            //xpsd.Close();


            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(page);
            doc.Close();
            package.Close();

            var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
            
            XpsConverter.Convert(pdfXpsDoc,"JUHUUUU.pdf",0,false);
        }
       
    }
}
