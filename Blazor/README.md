##### Example: Blazor

# Convert HTML to PDF file in Blazor using C#

The Syncfusion&reg; HTML to PDF converter is a .NET library used to convert HTML or web pages to PDF document in Blazor application.

## Steps to convert HTML to PDF in Blazor application

1. Create a new C# Blazor Server application project. Select Blazor App from the template and click the Next button.
   <img src="HTML_to_PDF_Blazor/htmlconversion_images/blazor_step1.png" alt="Blazor_step1" width="100%" Height="Auto"/>

   In the project configuration window, name your project and select Create.
   <img src="HTML_to_PDF_Blazor/htmlconversion_images/blazor_step2.png" alt="Blazor_step2" width="100%" Height="Auto"/>
   <img src="HTML_to_PDF_Blazor/htmlconversion_images/blazor_step3.png" alt="Blazor_step3" width="100%" Height="Auto"/>

2. Install the [Syncfusion.HtmlToPdfConverter.Net.Windows](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Net.Windows/) NuGet package as a reference to your Blazor Server application from [NuGet.org](https://www.nuget.org/).
   <img src="HTML_to_PDF_Blazor/htmlconversion_images/blazor_step_nuget.png" alt="Blazor_step_nuget" width="100%" Height="Auto"/>

3. Create a new class file named ExportService under Data folder and include the following namespaces in the file.

   ```csharp
   using Syncfusion.HtmlConverter;
   using Syncfusion.Pdf;
   using System.IO;
   ```

4. Add the following code to convert HTML to PDF document in [ExportService](HTML_to_PDF_Blazor/Data/ExportService.cs) class.

   ```csharp
   public MemoryStream CreatePdf(string url)
   {
      //Initialize HTML to PDF converter.
      HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();      
      //Convert URL to PDF document.
      PdfDocument document = htmlConverter.Convert(url);
      //Create memory stream.
      MemoryStream stream = new MemoryStream();
      //Save the document to memory stream.
      document.Save(stream);
      return stream;
   }
   ```

5. Register your service in the ConfigureServices method available in the Startup.cs class as follows.

   ```csharp
   /// <summary>
   /// Register your ExportService 
   /// </summary>
   public void ConfigureServices(IServiceCollection services)
   {
      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddSingleton<WeatherForecastService>();
      services.AddSingleton<ExportService>();
   }
   ```

6. Inject ExportService into FetchData.razor using the following code.

   ```csharp
   @inject ExportService exportService
   @inject Microsoft.JSInterop.IJSRuntime JS
   @inject NavigationManager NavigationManager
   @using  System.IO;
   ```

7. Create a button in the FetchData.razor using the following code.

   ```csharp
   <button class="btn btn-primary" @onclick="@ExportToPdf">Export to PDF</button>
   ```

8. Add the ExportToPdf method in FetchData.razor page to call the export service.

   ```csharp
   @code {
   private string currentUrl;
   /// <summary>
   /// Get the current URL
   /// </summary>
   protected override void OnInitialized()
   {
      currentUrl = NavigationManager.Uri;
   }
   }
   @functions
   {
      /// <summary>
      /// Create and download the PDF document
      /// </summary>
      protected async Task ExportToPdf()
      {
         ExportService exportService = new ExportService();
         using (MemoryStream excelStream = exportService.CreatePdf(currentUrl))
         {
            await JS.SaveAs("HTML-to-PDF.pdf", excelStream.ToArray());
         }
      }
   }
   ```

9. Create a class file with FileUtil name and add the following code to invoke the JavaScript action to download the file in the browser.

   ```csharp
   public static class FileUtil
   {
       public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
       => js.InvokeAsync<object>(
           "saveAsFile",
           filename,
           Convert.ToBase64String(data));
   }
   ```

10. Add the following JavaScript function in the _Host.cshtml available under the Pages folder.

    ```csharp
    <script type="text/javascript">
        function saveAsFile(filename, bytesBase64) {
            if (navigator.msSaveBlob) {
                //Download document in Edge browser
                var data = window.atob(bytesBase64);
                var bytes = new Uint8Array(data.length);
                for (var i = 0; i < data.length; i++) {
                    bytes[i] = data.charCodeAt(i);
                }
                var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
                navigator.msSaveBlob(blob, filename);
            }
            else {
                var link = document.createElement('a');
                link.download = filename;
                link.href = "data:application/octet-stream;base64," + bytesBase64;
                document.body.appendChild(link); // Needed for Firefox
                link.click();
                document.body.removeChild(link);
            }
        }
    </script>
    ```

    By executing the program, you will get the following output in the browser.
    <img src="HTML_to_PDF_Blazor/htmlconversion_images/blazor_step4.png" alt="Blazor_step4" width="100%" Height="Auto"/>
    Click the Export to PDF button, and you will get the PDF document with the following output.
    <img src="HTML_to_PDF_Blazor/htmlconversion_images/HtmlBlazorOutput.png" alt="HTMLTOPDF" width="100%" Height="Auto"/>
