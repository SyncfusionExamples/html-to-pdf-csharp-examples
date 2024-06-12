using HTML_to_PDF_Azure_app_service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace HTML_to_PDF_Azure_app_service.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);

            CefConverterSettings cefConverterSettings = new CefConverterSettings();

            //Set Blink viewport size.
            cefConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);

            //Assign Blink converter settings to HTML converter.
            htmlConverter.ConverterSettings = cefConverterSettings;

            //Convert URL to PDF document.
            PdfDocument document = htmlConverter.Convert("https://www.google.com");

            //Create memory stream.
            MemoryStream stream = new MemoryStream();

            //Save and close the document. 
            document.Save(stream);
            document.Close();
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
