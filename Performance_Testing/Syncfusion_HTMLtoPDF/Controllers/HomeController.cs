using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion_HTMLtoPDF.Models;
using System.Diagnostics;
using System.IO;

namespace Syncfusion_HTMLtoPDF.Controllers
{
    public class HomeController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ConvertToPdf()
        {

            string fileName = "HtmlSample_Thousand";
            //string fileName = "HtmlSample";
            //string fileName = "HtmlSample_FiveHundred";
            //string fileName = "HtmlSample_Hundred";
            double totalTime = 0;
            string ConsoleLog = "";
            MemoryStream stream = null;
            for (int i = 0; i < 5; i++)
            {
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
                BlinkConverterSettings settings = new BlinkConverterSettings();
                settings.AdditionalDelay = 0;
                settings.Margin.All = 0;
                settings.Scale = 1.0f;
                htmlConverter.ConverterSettings = settings;
                string htmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath,"Data", fileName + ".html");
                Stopwatch watch = new Stopwatch();
                watch.Start();
                //Convert URL to PDF
                PdfDocument document = htmlConverter.Convert(htmlFilePath);
                watch.Stop();
                totalTime += watch.Elapsed.TotalSeconds;
                ConsoleLog+=(i + 1) + " conversion time:" + watch.Elapsed.TotalSeconds+"\n";
               
                if (i == 0)
                {
                    using (stream = new MemoryStream())
                    {
                        document.Save(stream);
                        document.Close(true);
                    }
                }
            }
            double averageTime = totalTime / 5;
            ConsoleLog+="Average conversion time:" + averageTime+"\n";
            System.IO.File.WriteAllText(Path.Combine(_hostingEnvironment.WebRootPath, "Data", fileName + ".txt"), ConsoleLog);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
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
