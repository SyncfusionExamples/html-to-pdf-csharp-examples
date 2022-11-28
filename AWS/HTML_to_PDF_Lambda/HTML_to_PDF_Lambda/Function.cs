using Amazon.Lambda.Core;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HTML_to_PDF_Lambda;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        //Initialize HTML to PDF converter with Blink rendering engine.
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

        //Convert URL to PDF.
        PdfDocument document = htmlConverter.Convert(input);

        //Save the document into stream.
        MemoryStream memoryStream = new MemoryStream();

        //Save and Close the PDFDocument.
        document.Save(memoryStream);
        document.Close(true);

        return Convert.ToBase64String(memoryStream.ToArray());
    }
}
