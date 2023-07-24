using HtmlToPDFSample.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;

namespace HtmlToPDFSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ExportToPDF()
        {
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            MemoryStream stream = new MemoryStream();

            try
            {
                //Initialize HTML to PDF converter.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
                BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
                //Set command line arguments to run without the sandbox. 
                blinkConverterSettings.CommandLineArguments.Add("--no-sandbox");
                blinkConverterSettings.CommandLineArguments.Add("--disable-setuid-sandbox");
                //Set Blink viewport size.
                blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);
                //Assign Blink converter settings to HTML converter.
                htmlConverter.ConverterSettings = blinkConverterSettings;
                //Convert URL to PDF document.
                document = htmlConverter.Convert("https://www.google.com");
            }
            catch (Exception ex)
            {
                // Set the page size.
                document.PageSettings.Size = PdfPageSize.A4;
                //Add a page to the document.
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for the page.
                PdfGraphics graphics = page.Graphics;
                //Set the font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                //Draw the text.
                graphics.DrawString(ex.Message, font, PdfBrushes.Black, new Syncfusion.Drawing.RectangleF(0, 200, 400, 100));

            }

            //Save and close the document. 
            document.Save(stream);
            document.Close();
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HtmlOutput.pdf");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}