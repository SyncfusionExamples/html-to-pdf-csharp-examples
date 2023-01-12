## AWS Lambda with NET 6 container image

**Steps to convert HTML to PDF in AWS Lambda with NET 6 container image**

Step 1: Create a new AWS Lambda project with Tests as follows.
<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda1.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 2: Set a project name and select the location.
<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda2.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 3: Select Blueprint as .NET 6 (Container Image) Function and click Finish.
<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda3.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 4: Install the [Syncfusion.HtmlToPdfConverter.Net.Aws](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Net.Aws/) and [AWSSDK.Lambda](https://www.nuget.org/packages/AWSSDK.Lambda) NuGet packages as a reference to your AWS lambda project from [NuGet.org](https://www.nuget.org/).
<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda4.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 5: Using the following namespaces in the [Function.cs](HTML_to_PDF_Lambda_Docker_Container/Function.cs) file.

```csharp

using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;

```

Step 6: Add the following code sample in the [Function.cs](HTML_to_PDF_Lambda_Docker_Container/Function.cs) to create a PDF document.

```csharp

public string FunctionHandler(string input, ILambdaContext context)
{
   //Initialize HTML to a PDF converter with the Blink rendering engine.
   HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
       
   BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
   blinkConverterSettings.BlinkPath = Path.GetFullPath("BlinkBinariesAws");
   blinkConverterSettings.CommandLineArguments.Add("--no-sandbox");
   blinkConverterSettings.CommandLineArguments.Add("--disable-setuid-sandbox");
   blinkConverterSettings.AdditionalDelay = 3000;
   htmlConverter.ConverterSettings = blinkConverterSettings;
 
   //Convert the HTML string to PDF.
   PdfDocument document = htmlConverter.Convert(input, PathToFile());        
 
   //Save the document into a stream.
   MemoryStream memoryStream = new MemoryStream();
   //Save and close the PDFDocument.
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

```

Step 7: Create a new folder name as Helper and add a new class as [AWSHelper.cs](HTML_to_PDF_Lambda_Docker_Container/Helper/AWSHelper.cs) file. Add the following namespaces and code samples in the AWSHelper class to invoke the published AWS Lambda function using the function name and access keys.

```csharp

Using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;

public class AWSHelper
{
   public static async Task<byte[]> RunLambdaFunction(string html)
   {
      try
      {
         var AwsAccessKeyId = "awsaccessKeyID";
         var AwsSecretAccessKey = "awsSecretAccessKey";
 
         AmazonLambdaClient client = new AmazonLambdaClient(AwsAccessKeyId, AwsSecretAccessKey, Amazon.RegionEndpoint.USEast1);
         InvokeRequest invoke = new InvokeRequest
         {
            FunctionName = "AWSLambdaDockerContainer",
            InvocationType = InvocationType.RequestResponse,
            Payload = Newtonsoft.Json.JsonConvert.SerializeObject(html)
         };
         //Get the InvokeResponse from the client InvokeRequest.
         InvokeResponse response = await client.InvokeAsync(invoke);
 
         //Read the response stream.
         Console.WriteLine($"Response: {response.LogResult}");
         Console.WriteLine($"Response: {response.StatusCode}");
         Console.WriteLine($"Response: {response.FunctionError}");
         var stream = new StreamReader(response.Payload);
         JsonReader reader = new JsonTextReader(stream);
         var serilizer = new JsonSerializer();
         var responseText = serilizer.Deserialize(reader);
 
         //Convert Base64String into a PDF document.
         return Convert.FromBase64String(responseText.ToString());
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Exception Occured HTMLToPDFHelper: {ex}");
      }
   return Convert.FromBase64String("");
   }
}

```

Step 8: Right-click the project and select **Publish to AWS Lambda**.

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda5.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 9: Create a new AWS profile in the Upload Lambda Function Window. After creating the profile, add a name for the Lambda function to publish. Then, click **Next**.  

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda6.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 10: In the Advanced Function Details window, specify the **Role Name** as based on AWS Managed policy. After selecting the role, click the Upload button to deploy your application.

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda7.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>
<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda8.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 11: After deploying the application, Sign in to your AWS account, and you can see the published Lambda function in the AWS console.

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda9.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

**Steps to invoke the AWS Lambda function from the Test application**:

Step 12: Add the following code to invoke the AWS lambda function with the HTML string from the [FunctionTest.cs](HTML_to_PDF_Lambda_Docker_Container.Tests/FunctionTest.cs) file.

```csharp

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

```

Step 13: Right click the test application and select **Run Tests**.

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda10.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>

Step 14: By executing the program, you will get the PDF document as follows.

<img src="HTML_to_PDF_Lambda_Docker_Container/htmlconversion_images/awslambda11.png" alt="Convert HTMLToPDF AWS Step11" width="100%" Height="Auto"/>