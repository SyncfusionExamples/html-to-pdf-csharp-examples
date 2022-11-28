// See https://aka.ms/new-console-template for more information

using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

//Initialize HTML to PDF converter
HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

//Set Blink viewport size
blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);

//Assign Blink converter settings to HTML converter
htmlConverter.ConverterSettings = blinkConverterSettings;

//Convert URL to PDF document
PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

//Create a filesteam
FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);

//Save and close the PDF document
document.Save(fileStream);
document.Close(true);
