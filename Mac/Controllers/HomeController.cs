using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HTML_to_PDF_Mac.Models;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HTML_to_PDF_Mac.Controllers;

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
    public IActionResult ExportToPDF()
    {
        //Initialize HTML to PDF converter.
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

        //Convert URL to PDF.
        PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
        MemoryStream stream = new MemoryStream();
        document.Save(stream);
        //Download the PDF document in the browser
        return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");

    }
}

