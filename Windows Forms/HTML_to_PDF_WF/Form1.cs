using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTML_to_PDF_WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            //Set Blink viewport size
            blinkConverterSettings.ViewPortSize = new System.Drawing.Size(1280, 0);

            //Assign Blink converter settings to HTML converter
            htmlConverter.ConverterSettings = blinkConverterSettings;

            //Convert URL to PDF document
            PdfDocument document = htmlConverter.Convert("https://www.syncfusion.com");

            //Create file stream
            FileStream stream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew);

            //Save the document into stream
            document.Save(stream);

            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;

            //Close the document
            document.Close();
            stream.Dispose();
        }
    }
}
