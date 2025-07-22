using HTML_to_PDF_Azure_app_service_docker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;

namespace HTML_to_PDF_Azure_app_service_docker.Controllers
{
    public class HomeController : Controller
    {
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
        public ActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter. 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Assign Blink settings to the HTML converter.
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF.
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            MemoryStream stream = new MemoryStream();
            //Save and close a PDF document. 
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "URL_to_PDF.pdf");
        }
    }
}