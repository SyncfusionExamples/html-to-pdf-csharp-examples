using AWSElasticBeanstalkSample.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Diagnostics;

namespace AWSElasticBeanstalkSample.Controllers
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
            try
            {
                //Initialize HTML to PDF converter. 
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
                BlinkConverterSettings settings = new BlinkConverterSettings();
                //Set Blink viewport size.
                settings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);
                //Assign Blink settings to the HTML converter.
                htmlConverter.ConverterSettings = settings;
                //Convert URL to PDF document.
                PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
                //Create memory stream.
                MemoryStream stream = new MemoryStream();
                //Save the document to memory stream. 
                document.Save(stream);
                return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "BlinkAWSElasticBeanstalk.pdf");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }
            return View("Index");
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