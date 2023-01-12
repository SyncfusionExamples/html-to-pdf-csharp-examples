using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using HTML_to_PDF_Lambda_Docker_Container.Helper;

namespace HTML_to_PDF_Lambda_Docker_Container.Tests;

public class FunctionTest
{
    [Fact]
    public void HtmlToPDFFunction()
    {
        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        string filePath = Environment.OSVersion.Platform == PlatformID.Unix ? string.Concat(path.Substring(5), @"/") : string.Concat(path.Substring(6), @"\");

        var html = File.ReadAllText($"{filePath}/HtmlSample.html");
        byte[] base64 = null;
        base64 = AWSHelper.RunLambdaFunction(html).Result;

        FileStream file = new FileStream($"{filePath}/file{DateTime.Now.Ticks}.pdf", FileMode.Create, FileAccess.Write);
        var ms = new MemoryStream(base64);
        ms.WriteTo(file);
        file.Close();
        ms.Close();
    }
}