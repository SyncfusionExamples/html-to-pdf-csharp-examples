using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FunctionApp_Linux_HTMLtoPDF
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log, FunctionContext executionContext)
        {
            string blinkBinariesPath = string.Empty;
            //Create a new PDF document.
            PdfDocument document;
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Creating the stream object.
            MemoryStream stream;
            try
            {
                blinkBinariesPath = SetupBlinkBinaries();              
                
                //Initialize the HTML to PDF converter with the Blink rendering engine.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
              
                //Set command line arguments to run without sandbox.
                settings.CommandLineArguments.Add("--no-sandbox");
                settings.CommandLineArguments.Add("--disable-setuid-sandbox");
                settings.BlinkPath = blinkBinariesPath;
                //Assign BlinkConverter settings to the HTML converter 
                htmlConverter.ConverterSettings = settings;
                //Convert URL to PDF
                 document = htmlConverter.Convert("http://www.syncfusion.com");
                stream = new MemoryStream();
                //Save and close the PDF document  
                document.Save(stream);
            }
            catch(Exception ex)
            {
                //Create a new PDF document.
                 document = new PdfDocument();
                //Add a page to the document.
                PdfPage page = document.Pages.Add();
                //Create PDF graphics for the page.
                PdfGraphics graphics = page.Graphics;

                //Set the standard font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
                //Draw the text.
                graphics.DrawString(ex.Message.ToString(), font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));
              
                stream = new MemoryStream();
                //Save the document into memory stream.
                document.Save(stream);
            
            }
           
            document.Close();
            stream.Position = 0;
            return new FileStreamResult(stream, "application/pdf");
        }
        private static string SetupBlinkBinaries( )
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = fileInfo.Directory.Parent.FullName;
            string blinkAppDir = Path.Combine(path, @"wwwroot");
            string tempBlinkDir = Path.GetTempPath();
            string chromePath = Path.Combine(tempBlinkDir, "chrome");
            if (!File.Exists(chromePath))
            {
                CopyFilesRecursively(blinkAppDir, tempBlinkDir);
                SetExecutablePermission(tempBlinkDir);
            }
            return tempBlinkDir;
        }
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Create all the directories from the source to the destination path.
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            //Copy all the files from the source path to the destination path.
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        [DllImport("libc", SetLastError = true, EntryPoint = "chmod")]
        internal static extern int Chmod(string path, FileAccessPermissions mode);
        private static void SetExecutablePermission(string tempBlinkDir)
        {
            FileAccessPermissions ExecutableFilePermissions = FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite | FileAccessPermissions.UserExecute |
            FileAccessPermissions.GroupRead | FileAccessPermissions.GroupExecute | FileAccessPermissions.OtherRead | FileAccessPermissions.OtherExecute;
            string[] executableFiles = new string[] { "chrome", "chrome_sandbox" };
            foreach (string executable in executableFiles)
            {
                var execPath = Path.Combine(tempBlinkDir, executable);
                if (File.Exists(execPath))
                {
                    var code = Function1.Chmod(execPath, ExecutableFilePermissions);
                    if (code != 0)
                    {
                        throw new Exception("Chmod operation failed");
                    }
                }
            }
        }
        [Flags]
        internal enum FileAccessPermissions : uint
        {
            OtherExecute = 1,
            OtherWrite = 2,
            OtherRead = 4,
            GroupExecute = 8,
            GroupWrite = 16,
            GroupRead = 32,
            UserExecute = 64,
            UserWrite = 128,
            UserRead = 256
        }
    }
}
