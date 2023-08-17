using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace HTML_to_PDF_Azure_Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            MemoryStream ms = new MemoryStream();
            try {
                //Initialize HTML to PDF converter.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
                WebKitConverterSettings settings = new WebKitConverterSettings();
                //Set WebKit path.
                settings.WebKitPath = Path.Combine(context.FunctionAppDirectory, "bin/runtimes/win-x64/native");
				//Assign WebKit settings to HTML converter.
                htmlConverter.ConverterSettings = settings;
                //Convert URL to PDF.
                PdfDocument document = htmlConverter.Convert("https://www.google.com");

                //Save and close the PDF document.
                document.Save(ms);
                document.Close();
                ms.Position = 0;
            }

            catch (Exception ex) {
                //Create a new PDF document.
                PdfDocument document = new PdfDocument();
                //Add a page to the document.
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page.
                PdfGraphics graphics = page.Graphics;

                //Set the standard font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 5);
                //Draw the text.
                graphics.DrawString(ex.ToString(), font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                //Creating the stream object.
                ms = new MemoryStream();
                //Save the document into memory stream.
                document.Save(ms);
                //Close the document.
                document.Close(true);
                ms.Position = 0;
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(ms.ToArray());
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
                FileName = "Sample.pdf"
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return response;
        }
    }
}
