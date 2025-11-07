using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Drawing;
using Syncfusion.Pdf;

namespace HTMLtoPDF_Wolfi_Docker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {

        public IActionResult ConvertPDF()
        {
            try
            {
                //Initialize the HTML to PDF converter.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

                BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
                blinkConverterSettings.CommandLineArguments.Add("--disable-gpu");
                blinkConverterSettings.BlinkPath = "/usr/lib/chromium";
                //Assign Blink converter settings to HTML converter
                htmlConverter.ConverterSettings = blinkConverterSettings;

                //Convert URL to PDF
                PdfDocument document = htmlConverter.Convert("https://www.google.com");
               MemoryStream fileStream = new MemoryStream();
                //Save and close the PDF document.
                document.Save(fileStream);
                document.Close(true);
                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "application/pdf") {FileDownloadName="HtmlToPdf_Output.pdf" };
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }
        }
    }
}
