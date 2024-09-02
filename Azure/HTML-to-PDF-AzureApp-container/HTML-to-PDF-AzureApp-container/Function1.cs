using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using System.Runtime.InteropServices;

namespace HTML_to_PDF_AzureApp_container
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string blinkBinariesPath = string.Empty;
            MemoryStream ms = null;

            try
            {
                // Attempt to initialize Blink binaries
                blinkBinariesPath = SetupBlinkBinaries();

                // Initialize the HTML to PDF converter with the Blink rendering engine.
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
                BlinkConverterSettings settings = new BlinkConverterSettings();

                // Set command line arguments to run without sandbox.
                settings.CommandLineArguments.Add("--no-sandbox");
                settings.CommandLineArguments.Add("--disable-setuid-sandbox");
                settings.BlinkPath = blinkBinariesPath;

                // Assign BlinkConverter settings to the HTML converter
                htmlConverter.ConverterSettings = settings;

                // Convert URL to PDF
                PdfDocument document = htmlConverter.Convert("https://www.google.com/");
                ms = new MemoryStream();

                // Save and close the PDF document  
                document.Save(ms);
                document.Close();
            }
            catch (Exception ex)
            {
                // Handle any exception by creating an error PDF document
                PdfDocument document = new PdfDocument();
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                // Draw the exception message in the PDF
                graphics.DrawString(ex.Message, font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

                ms = new MemoryStream();
                document.Save(ms);
                document.Close(true);
            }

            ms.Position = 0;
            return new FileStreamResult(ms, "application/pdf");
        }


        private static string SetupBlinkBinaries()

        {

            string blinkAppDir = Path.Combine("/home/site/wwwroot/runtimes/linux/native");

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
