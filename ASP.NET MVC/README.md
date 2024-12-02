##### Example: ASP.NET MVC

# Convert HTML to PDF file in ASP.NET MVC using C#

The Syncfusion&reg; HTML to PDF converter is a .NET library used to convert HTML or web pages to PDF document in ASP.NET MVC application.  

## Steps to convert HTML to PDF document in ASP.NET MVC

1. Create a new C# ASP.NET Web Application (.NET Framework) project.
   <img src="HTML_to_PDF_MVC/htmlconversion_images/MVC_sample_creation_step1.png" alt="MVC sample creation step1" width="100%" Height="Auto"/>

2. In the project configuration windows, name your project and select Create.
   <img src="HTML_to_PDF_MVC/htmlconversion_images/MVC_sample_creation_step2.png" alt="MVC sample creation step1" width="100%" Height="Auto"/>
   <img src="HTML_to_PDF_MVC/htmlconversion_images/MVC_sample_creation_step3.png" alt="MVC sample creation step1" width="100%" Height="Auto"/>

2. Install [Syncfusion.HtmlToPdfConverter.AspNet.Mvc5](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.AspNet.Mvc5)  NuGet package as reference to your .NET applications from [NuGet.org](https://www.nuget.org/).
   <img src="HTML_to_PDF_MVC/htmlconversion_images/MVC_sample_creation_step4.png" alt="MVC sample creation step1" width="100%" Height="Auto"/>

3. Include the following namespaces in the [HomeController.cs](HTML_to_PDF_MVC/Controllers/HomeController.cs) file.

   ```csharp
   using Syncfusion.Pdf;
   using Syncfusion.HtmlConverter;
   using System.IO;
   ```   

4. Add a new button in the [Index.cshtml](HTML_to_PDF_MVC/Views/Home/Index.cshtml) as shown below.

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

5. Add a new action method named ExportToPDF in [HomeController.cs](HTML_to_PDF_MVC/Controllers/HomeController.cs) and include the below code snippet to convert HTML to PDF document.

   ```csharp
   //Initialize HTML to PDF conv erter.
   HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
   BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();
   //Set Blink viewport size.
   blinkConverterSettings.ViewPortSize = new System.Drawing.Size(1280, 0);
   //Assign Blink converter settings to HTML converter.
   htmlConverter.ConverterSettings = blinkConverterSettings;
   //Convert URL to PDF document.
   PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");
   //Create memory stream.
   MemoryStream stream = new MemoryStream();
   //Save the document to memory stream.
   document.Save(stream);
   return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTML-to-PDF.pdf");
   ```

   By executing the program, you will get the PDF document as follows.
   <img src="HTML_to_PDF_MVC/htmlconversion_images/htmltopdfoutput.png" alt="Output screenshot for HTML to PDF conversion" width="100%" Height="Auto"/>
