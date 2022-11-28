using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTML_to_PDF_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ConvertHTMLToPDF()
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            //Set Blink viewport size
            blinkConverterSettings.ViewPortSize = new System.Drawing.Size(1280, 0);

            //Assign Blink converter settings to HTML converter
            htmlConverter.ConverterSettings = blinkConverterSettings;

            //Convert URL to PDF document
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

            //Create memory stream
            MemoryStream stream = new MemoryStream();

            //Save the document to memory stream
            document.Save(stream);
            
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}