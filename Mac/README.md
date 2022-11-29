# Convert HTML to PDF file in Mac using C#

The Syncfusion [HTML to PDF converter](https://www.syncfusion.com/pdf-framework/net/html-to-pdf) is a .NET library for converting webpages, SVG, MHTML, and HTML to PDF using C#. It is reliable and accurate. Using this library, you can convert HTML to PDF document in Mac.

## Steps to convert HTML to PDF in ASP.NET Core MVC

1. Create a new C# ASP.NET Core Web Application project.
   <img src="HTML_to_PDF_images/mac_step1.png" alt="Mac_step1" width="100%" Height="Auto"/>


2. Select the Target Framework of your project.
   <img src="HTML_to_PDF_images/mac_step2.png" alt="Mac_step2" width="100%" Height="Auto"/>

3. Configure your application and click Create.
   <img src="HTML_to_PDF_images/mac_step3.png" alt="Mac_step3" width="100%" Height="Auto"/>

4. Install the [Syncfusion.HtmlToPdfConverter.Net.Mac](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Net.Mac) NuGet package as reference to your .NET Standard applications from [NuGet.org](https://www.nuget.org/).
   <img src="HTML_to_PDF_images/mac_step4.png" alt="Mac_step4" width="100%" Height="Auto"/>

5. A default controller with name HomeController.cs gets added on creation of ASP.NET MVC project. Include the following namespaces in that HomeController.cs file.

    ```csharp

   using Syncfusion.Pdf;
   using Syncfusion.HtmlConverter;
   using System.IO;
   using Microsoft.AspNetCore.Hosting;

   ```

6. A default action method named Index will be present in HomeController.cs. Right click on Index method and select Go To View where you will be directed to its associated view page [Index.cshtml](Views\Home\Index.cshtml) file

   Add a new button in the Index.cshtml as shown below.

   ```csharp

   @{Html.BeginForm("ExportToPDF", "Home", FormMethod.Post);
   {
     <div>
      <input type="submit" value="Convert PDF" style="width:150px;height:27px" />
     </div>
   }
   Html.EndForm();
   }
   ```
   

7. Add a new action method ExportToPDF in [HomeController.cs](Controllers\HomeController.cs) file and include the below code snippet to convert HTML to PDF file and download it.

   ```csharp

        public IActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter.
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            //Convert URL to PDF.
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            //Download the PDF document in the browser
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
        }

   ```

8. Right click the project and select Build.
   <img src="HTML_to_PDF_images/mac_step5.png" alt="Mac_step5" width="100%" Height="Auto"/>

   `Note: Once the build succeeded, unzip the chromium.app file in bin folder (bin -> Debug ->net6.0 -> BlinkBinariesMac -> Chromium.app).`

9. Run the application.
   <img src="HTML_to_PDF_images/mac_step6.png" alt="Mac_step6" width="100%" Height="Auto"/>

   By executing the program, you will get the PDF document as follows.
    <img src="HTML_to_PDF_images/htmltopdfoutput.png" alt="HTMLTOPDF" width="100%" Height="Auto"/>
