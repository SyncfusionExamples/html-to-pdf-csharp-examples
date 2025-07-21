using Amazon.Lambda.Core;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HTML_to_PDF_Lambda_Docker_Container;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and returns both the upper and lower case version of the string.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        //Initialize HTML to PDF converter with Blink rendering engine.
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

        BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

        blinkConverterSettings.BlinkPath = Path.GetFullPath("BlinkBinariesAws");

        blinkConverterSettings.AdditionalDelay = 3000;

        htmlConverter.ConverterSettings = blinkConverterSettings;

        //Convert html string to PDF.
        PdfDocument document = htmlConverter.Convert(input, PathToFile());

        //Save the document into stream.
        MemoryStream memoryStream = new MemoryStream();

        //Save and Close the PDFDocument.
        document.Save(memoryStream);
        document.Close(true);

        string base64 = Convert.ToBase64String(memoryStream.ToArray());

        memoryStream.Close();
        memoryStream.Dispose();

        return base64;
    }
    public static string PathToFile()
    {
        string? path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        if (string.IsNullOrEmpty(path))
        {
            path = Environment.OSVersion.Platform == PlatformID.Unix ? @"/" : @"\";
        }
        return Environment.OSVersion.Platform == PlatformID.Unix ? string.Concat(path.Substring(5), @"/") : string.Concat(path.Substring(6), @"\");
    }
}

public record Casing(string Lower, string Upper);