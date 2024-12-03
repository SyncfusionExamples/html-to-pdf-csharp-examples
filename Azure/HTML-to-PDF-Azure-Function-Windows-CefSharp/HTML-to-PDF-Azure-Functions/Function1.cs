using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

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

        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string blinkBinariesPath = string.Empty;
            MemoryStream ms = null;
            try
            {

                //Initialize the HTML to PDF converter with the Blink rendering engine.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
                //Convert URL to PDF
                PdfDocument document = htmlConverter.Convert("https://www.google.com/");

                ms = new MemoryStream();
                //Save and close the PDF document  
                document.Save(ms);
                document.Close();
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
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
                //Draw the text.
                graphics.DrawString(ex.Message, font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                //Creating the stream object.
                ms = new MemoryStream();
                //Save the document into memory stream.
                document.Save(ms);
                //Close the document.
                document.Close(true);

            }
            ms.Position = 0;
            return new FileStreamResult(ms, "application/pdf");
        }
    }
}
