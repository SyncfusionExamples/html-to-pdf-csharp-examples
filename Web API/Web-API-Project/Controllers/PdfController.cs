using Microsoft.AspNetCore.Mvc;
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace Web_API_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PdfController> _logger;

        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/api/Pdf")]
        public IActionResult ConvertHTMLtoPDF()
        {
            try
            {
                const string fileDownloadName = "Output.pdf";
                const string contentType = "application/pdf";
                var stream = ExportWeatherForecastToPdf();
                stream.Position = 0;
                return File(stream, contentType, fileDownloadName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred while creating PDF file: {ex.Message}");
            }
        }

        private MemoryStream ExportWeatherForecastToPdf()
        {
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();

            //Initialize HTML to PDF converter.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
            //Set Blink viewport size.
            blinkConverterSettings.ViewPortSize =new Size(1280, 0);
            //Assign Blink converter settings to HTML converter.
            htmlConverter.ConverterSettings = blinkConverterSettings;
            //Convert URL to PDF document.
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            //Create memory stream.
            MemoryStream stream = new MemoryStream();
            //Save the document to memory stream.
            document.Save(stream);
            return new MemoryStream(stream.ToArray());
        }
    }
}
