using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HTML_to_PDF_Blazor.Data
{
    public class ExportService
    {
        public MemoryStream CreatePdf()
        {
            //Initialize HTML to PDF converter.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
            //Set Blink viewport size.
            blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);
            //Assign Blink converter settings to HTML converter.
            htmlConverter.ConverterSettings = blinkConverterSettings;
            //Convert URL to PDF document.
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            //Create memory stream.
            MemoryStream stream = new MemoryStream();
            //Save the document to memory stream.
            document.Save(stream);
            return stream;
        }
    }
}
