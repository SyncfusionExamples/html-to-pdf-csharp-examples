//Initialize HTML to PDF converter
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;

HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
string svgFilePath = Path.GetFullPath(@"../../../Input.svg");
//Convert the SVG file to Image
Image image = htmlConverter.ConvertToImage(svgFilePath);
byte[] imageByte = image.ImageData;
//Save the image
File.WriteAllBytes("Output.jpg", imageByte);