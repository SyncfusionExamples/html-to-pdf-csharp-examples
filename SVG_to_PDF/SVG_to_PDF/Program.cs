//Initialize HTML to PDF converter
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
string svgFilePath = Path.GetFullPath("../../../Input.svg");
//Convert a SVG file to PDF with HTML converter
PdfDocument document = htmlConverter.Convert(svgFilePath);
FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
//Save and close the PDF document.
document.Save(fileStream);
document.Close(true);