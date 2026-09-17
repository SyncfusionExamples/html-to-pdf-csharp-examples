using Microsoft.UI.Xaml;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.IO;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HTML_to_PDF_WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text;
            try
            {
                HtmlToPdfConverter converter = new HtmlToPdfConverter();

                BlinkConverterSettings settings = new BlinkConverterSettings();

                // Optional settings
                settings.ViewPortSize = new Syncfusion.Drawing.Size(1280, 0);

                converter.ConverterSettings = settings;

                PdfDocument document = converter.Convert(url);

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output.pdf");

                using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                document.Save(stream);
                document.Close(true);

                StatusText.Text = $"PDF saved to: {filePath}";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                StatusText.Text = ex.ToString();

                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
