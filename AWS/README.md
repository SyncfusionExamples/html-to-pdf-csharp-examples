# Convert HTML to PDF file in AWS using C#

The Syncfusion [HTML to PDF converter](https://www.syncfusion.com/pdf-framework/net/html-to-pdf) is a .NET library for converting webpages, SVG, MHTML, and HTML to PDF using C# with Blink rendering engine in AWS.

## Setting up the AWS Toolkit for Visual Studio

* You can create an AWS account by referring to this [link](https://aws.amazon.com/).
* Download and install the AWS Toolkit for Visual Studio, you can download the AWS toolkit from this [link](https://aws.amazon.com/visualstudio/).
* The Toolkit can be installed from Tools/Extension and updates options in Visual Studio. 

Refer to the following steps to convert HTML to PDF in AWS Lambda

* Create an AWS Lambda function to convert HTML to PDF and publish it to AWS.
* Invoke the AWS Lambda function in your main application using AWS SDKs.

## Steps to convert HTML to PDF in AWS Lambda

1. Create a new AWS Lambda project as follows.
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS1.png" alt="Aws1 image" width="100%" Height="Auto"/> 
 
2. In configuration window, name the project and select Create.
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS2.png" alt="Aws2 image" width="100%" Height="Auto"/> 

3. Select Blueprint as Empty Function and click Finish.
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS3.png" alt="Aws3 image" width="100%" Height="Auto"/>
4. Install the [Syncfusion.HtmlToPdfConverter.Net.Aws](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Net.Aws/) NuGet package as a reference to your AWS lambda project from [NuGet.org.](https://www.nuget.org/)
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS4.png" alt="Aws4 image" width="100%" Height="Auto"/>

5. Using the following namespaces in the Function.cs file.

   ```csharp

   using Syncfusion.HtmlConverter;
   using Syncfusion.Pdf;
   using System.IO;

   ```

6. Add the following code snippet in Function.cs to convert HTML to PDF document.

    ```csharp

            //Initialize HTML to PDF converter with Blink rendering engine.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            //Convert URL to PDF.
            PdfDocument document = htmlConverter.Convert(input);
            //Save the document into stream.
            MemoryStream memoryStream = new MemoryStream();
            //Save and Close the PDF Document.
            document.Save(memoryStream);
            document.Close(true);
            return Convert.ToBase64String(memoryStream.ToArray());

   ```

7. Right-click the project and select Publish to AWS Lambda. 
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS5.png" alt="Aws5 image" width="100%" Height="Auto"/>  

8. Create a new AWS profile in the Upload Lambda Function Window. After creating the profile, add a name for the Lambda function to publish. Then, click Next.   
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS6.png" alt="Aws6 image" width="100%" Height="Auto"/>    

9. In the Advanced Function Details window, specify the Role Name as based on AWS Managed policy. After selecting the role, click the Upload button to deploy your application.
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS7.png" alt="Aws7 image" width="100%" Height="Auto"/>  

10. After deploying the application, sign in your AWS account and you can see the published Lambda function in AWS console.
    <img src="HTML_to_PDF_Lambda\HTML_to_PDF_Lambda\HTML_to_PDF_images/AWS8.png" alt="Aws8 image" width="100%" Height="Auto"/>

## Steps to invoke the AWS Lambda function from the console application

1. Create a new console project.  
    <img src="HTML_to_PDF_console\HTML_to_PDF_console\HTML_to_PDF_images\AWS9.png" alt="Aws9 image" width="100%" Height="Auto"/>

2. In project configuration windows, name the project and select Create.
    <img src="HTML_to_PDF_console\HTML_to_PDF_console\HTML_to_PDF_images\AWS10.png" alt="Aws10 image" width="100%" Height="Auto"/> 

3. Install the [AWSSDK.Core](https://www.nuget.org/packages/AWSSDK.Core), [AWSSDK.Lambda](https://www.nuget.org/packages/AWSSDK.Lambda) and [Newtonsoft.Json package](https://www.nuget.org/packages/Newtonsoft.Json/13.0.2-beta3) as a reference to your main application from the [NuGet.org](https://www.nuget.org/).    
    <img src="HTML_to_PDF_console\HTML_to_PDF_console\HTML_to_PDF_images\AWS11.png" alt="Aws11 image" width="100%" Height="Auto"/> 
 
4. Include the following namespaces in Program.cs file.

   ```csharp

   using Amazon;
   using Amazon.Lambda;
   using Amazon.Lambda.Model;
   using Newtonsoft.Json;
   using System.IO;

   ```

5. Add the following code snippet in Program class to invoke the published AWS Lambda function using the function name and access keys.

   ```csharp

            //Create a new AmazonLambdaClient
            AmazonLambdaClient client = new AmazonLambdaClient("awsaccessKeyID", "awsSecreteAccessKey", RegionEndpoint.USEast1);
            //Create new InvokeRequest with the published function name
            InvokeRequest invoke = new InvokeRequest
            {
                FunctionName = "AwsLambdaFunctionHtmlToPdfConversion",
                InvocationType = InvocationType.RequestResponse,
                Payload = "\" https://www.google.co.in/ \""
            };
            //Get the InvokeResponse from client InvokeRequest
            InvokeResponse response = client.Invoke(invoke);
            //Read the response stream
            var stream = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(stream);
            var serilizer = new JsonSerializer();
            var responseText = serilizer.Deserialize(reader);
            //Convert Base64String into PDF document
            byte[] bytes = Convert.FromBase64String(responseText.ToString());
            FileStream fileStream = new FileStream("Sample.pdf", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fileStream);
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            System.Diagnostics.Process.Start("Sample.pdf");
   ```
 
   By executing the program, you will get the PDF document as follows. 
    <img src="HTML_to_PDF_console\HTML_to_PDF_console\HTML_to_PDF_images\AWS12.png" alt="Aws12 image" width="100%" Height="Auto"/>