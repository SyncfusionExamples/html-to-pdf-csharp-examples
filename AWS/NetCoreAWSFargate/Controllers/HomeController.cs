using Microsoft.AspNetCore.Mvc;
using NetCoreAWSFargate.Models;
using System.Diagnostics;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using System.IO;

namespace NetCoreAWSFargate.Controllers
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
        public IActionResult BlinkToPDF()
        {
            //Initialize HTML to PDF converter.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Set command line arguments to run without the sandbox.
            settings.CommandLineArguments.Add("--no-sandbox");
            settings.CommandLineArguments.Add("--disable-setuid-sandbox");
            //Set Blink viewport size.
            settings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);
            //Assign Blink settings to the HTML converter.
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF document.
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            //Create the memory stream.
            MemoryStream stream = new MemoryStream();
            //Save the document to the memory stream.
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "BlinkLinuxDockerAWSFargate.pdf");
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