using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;

namespace HTML_to_PDF_Azure_Functions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {

            MemoryStream ms = new MemoryStream();
            try
            {
                //Initialize HTML to PDF converter.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
                CefConverterSettings settings = new CefConverterSettings();
                //Assign WebKit settings to HTML converter.
                htmlConverter.ConverterSettings = settings;
                //Convert URL to PDF.
                PdfDocument document = htmlConverter.Convert("https://www.google.com/");
                //Save and close the PDF document.
                document.Save(ms);
                document.Close();
                ms.Position = 0;
            }

            catch (Exception ex)
            {
                //Create a new PDF document.
                PdfDocument document = new PdfDocument();
                //Add a page to the document.
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page.
                PdfGraphics graphics = page.Graphics;

                //Set the standard font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 5);
                //Draw the text.
                graphics.DrawString(ex.ToString(), font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                //Creating the stream object.
                ms = new MemoryStream();
                //Save the document into memory stream.
                document.Save(ms);
                //Close the document.
                document.Close(true);
                ms.Position = 0;
            }
            return new FileStreamResult(ms, "application/pdf");
        }
    }
}
