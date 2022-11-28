using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using AzureAppLinux_CoreSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppLinux_CoreSample.Controllers
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
        //To Export HTML to PDF
        public IActionResult ExportToPDF()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            BlinkConverterSettings settings = new BlinkConverterSettings();

            //Set command line arguments to run without sandbox.
            settings.CommandLineArguments.Add("--no-sandbox");

            settings.CommandLineArguments.Add("--disable-setuid-sandbox");

            //Assign WebKit settings to the HTML converter 
            htmlConverter.ConverterSettings = settings;

            //Convert HTML string to PDF
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

            //Save the document into stream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            stream.Position = 0;

            //Close the document
            document.Close(true);

            //Defining the ContentType for pdf file
            string contentType = "application/pdf";

            //Define the file name
            string fileName = "URL-to-PDF.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name
            return File(stream, contentType, fileName);
        }

    }
}
