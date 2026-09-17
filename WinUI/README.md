# HTML to PDF Converter - WinUI 3 Sample

A Windows UI (WinUI 3) application that converts HTML and web pages to PDF documents using the [Syncfusion .NET HTML to PDF converter library](https://www.syncfusion.com/document-sdk/net-pdf-library/html-to-pdf).

## Overview

This sample demonstrates how to integrate the Syncfusion HTML to PDF converter into a WinUI 3 desktop application. Users can enter a URL and convert it to a PDF document with just a few clicks.

## Features

- 🌐 **URL to PDF Conversion**: Convert web pages from URLs directly to PDF
- 💻 **WinUI 3 Interface**: Modern and responsive user interface built with WinUI 3
- 📄 **High-Quality Output**: Produces high-fidelity PDF documents using Syncfusion library
- ⚡ **Fast Processing**: Efficient conversion with minimal overhead

## Requirements

- **.NET 8.0** or higher
- **Windows 10/11** (with Windows UI Runtime)
- **Visual Studio 2022** or later (recommended)
- **Syncfusion Essential Studio for .NET** - HTML to PDF conversion library

## Installation & Setup

### 1. Clone or Download the Project
```powershell
git clone <repository-url>
cd HTML-to-PDF-WinUI
```

### 2. Install Dependencies
The project uses NuGet for package management. Dependencies will be restored automatically when you open the solution.

Required NuGet packages:
- [Syncfusion.HtmlToPdfConverter.Net.Windows](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Net.Windows) - For HTML to PDF conversion

### 3. Configure Syncfusion License (if needed)
If you have a Syncfusion license, register it in your `App.xaml.cs` or `MainWindow.xaml.cs`:

```csharp
using Syncfusion.Licensing;

public App()
{
	// Register your Syncfusion license key
	SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

	this.InitializeComponent();
}
```

### 4. Build and Run
```powershell
# Using Visual Studio
# Open HTML-to-PDF-WinUI.slnx and press F5

# Or using CLI
dotnet build
dotnet run
```

## Usage

1. **Launch the Application**: Run the WinUI application
2. **Enter URL**: In the text box, enter the URL you want to convert (default: https://www.syncfusion.com)
3. **Convert to PDF**: Click the "Convert HTML to PDF" button
4. **View Status**: The application will display the conversion status and save location

### Example
- Default URL: `https://www.syncfusion.com`
- Custom URL: Enter any valid web URL (e.g., `https://www.example.com`)

## Resources

* **Product page:** [Syncfusion PDF Framework](https://www.syncfusion.com/document-sdk/net-pdf-library)
* **Documentation page:** [Syncfusion .NET PDF library](https://help.syncfusion.com/document-processing/pdf/pdf-library/net/overview)
* **Online demo:** [Syncfusion .NET PDF library - Online demos](https://document.syncfusion.com/demos/pdf/default)
* **Blog:** [Syncfusion .NET PDF library - Blog](https://www.syncfusion.com/blogs/category/pdf)
* **Knowledge Base:** [Syncfusion .NET PDF library - Knowledge Base](https://www.syncfusion.com/kb/windowsforms/pdf)
* **EBooks:** [Syncfusion .NET PDF library - EBooks](https://www.syncfusion.com/succinctly-free-ebooks)
* **FAQ:** [Syncfusion .NET PDF library - FAQ](https://www.syncfusion.com/faq/)

## Support and feedback

* For any other queries, reach our [Syncfusion support team](https://www.syncfusion.com/support/directtrac/incidents/newincident?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples) or post the queries through the [community forums](https://www.syncfusion.com/forums?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).
* Request new feature through [Syncfusion feedback portal](https://www.syncfusion.com/feedback?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).

## License

This is a commercial product and requires a paid license for possession or use. Syncfusion’s licensed software, including this component, is subject to the terms and conditions of [Syncfusion's EULA](https://www.syncfusion.com/eula/es/?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples). You can purchase a licnense [here](https://www.syncfusion.com/sales/products?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples) or start a free 30-day trial [here](https://www.syncfusion.com/account/manage-trials/start-trials?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples).

## About Syncfusion

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion has more than 26,000+ customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.

Today, we provide 1600+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [ASP.NET WebForms](https://www.syncfusion.com/jquery/aspnet-webforms-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [React](https://www.syncfusion.com/react-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)), mobile ([Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [WinUI(Preview)](https://www.syncfusion.com/winui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=github&utm_medium=listing&utm_campaign=github-docio-examples)). We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.

---

**Built with ❤️ using WinUI 3 and Syncfusion .NET Libraries**
