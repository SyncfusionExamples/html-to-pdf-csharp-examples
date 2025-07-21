using HtmlToPdfBlinkAzureAppLinux.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using Syncfusion.Pdf.Graphics;
using System.Runtime.InteropServices;

namespace HtmlToPdfBlinkAzureAppLinux.Controllers
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
        public ActionResult ExportToPDF()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            //Install the dependencies packages for HTML to PDF conversion in Linux
            string shellFilePath = $"{Path.GetDirectoryName(GetType().Assembly.Location)}/dependenciesInstall.sh";
            InstallDependecies(shellFilePath);
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
            htmlConverter.ConverterSettings = blinkConverterSettings;
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            MemoryStream outputStream = new MemoryStream();
            //Close the document
            document.Save(outputStream);
            document.Close(true);
            //Creates a FileContentResult object by using the file contents, content type, and file name
            return File(outputStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "AppserviceLinux.pdf");
        }
        private void InstallDependecies(string shellFilePath)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c " + shellFilePath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };
            process.Start();
            process.WaitForExit();
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