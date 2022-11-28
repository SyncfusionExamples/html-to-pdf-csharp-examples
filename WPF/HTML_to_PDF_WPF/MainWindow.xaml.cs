using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using System.Diagnostics;

namespace HTML_to_PDF_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            //Set Blink viewport size
            blinkConverterSettings.ViewPortSize = new System.Drawing.Size(1280, 0);

            //Assign Blink converter settings to HTML converter
            htmlConverter.ConverterSettings = blinkConverterSettings;

            // Convert URL to PDF document
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

            //Create file stream
            FileStream stream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew);

            //Save the document into stream
            document.Save(stream);

            //If the position is not set to '0' then the PDF will be empty
            stream.Position = 0;

            //Close the document
            document.Close();
            stream.Dispose();

            //This will open the PDF file so, the result will be seen in default PDF viewer
            Process.Start("HTML-to-PDF.pdf");
        }
    }
}
