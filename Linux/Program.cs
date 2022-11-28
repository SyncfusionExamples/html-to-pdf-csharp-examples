using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using System;

namespace Linux_HTML_to_PDF_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize HTML to PDF converter
			HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

			BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
		
			blinkConverterSettings.CommandLineArguments.Add("--no-sandbox");
            	
			blinkConverterSettings.CommandLineArguments.Add("--disable-setuid-sandbox");

			//Assign Blink converter settings to HTML converter
			htmlConverter.ConverterSettings = blinkConverterSettings;

			//Convert URL to PDF
			PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

			FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
            
			//Save and close the PDF document 
			document.Save(fileStream);
		
			document.Close(true);
        }
    }
}
