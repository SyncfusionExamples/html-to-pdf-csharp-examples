using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;
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
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                byte[] pdfBytes = HtmlToPdfConvert("<p>Hello world</p>");
                return new FileStreamResult(new MemoryStream(pdfBytes), "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), "An error occurred while converting HTML to PDF.");
                return new ContentResult
                {
                    Content = $"Error: {ex.Message}",
                    ContentType = "text/plain",
                    StatusCode = StatusCodes.Status500InternalServerError
                };

            }

        }
        public byte[] HtmlToPdfConvert(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                throw new ArgumentException("HTML text cannot be null or empty.", nameof(htmlText));
            }

            try
            {
                // Setup Blink binaries and install necessary packages
                var blinkBinariesPath = SetupBlinkBinaries();
                InstallLinuxPackages();

                // Initialize HTML to PDF converter
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

                // Display settings
                BlinkConverterSettings settings = new BlinkConverterSettings
                {
                    BlinkPath = blinkBinariesPath,
                    ViewPortSize = new Syncfusion.Drawing.Size(1024, 768), // Set your desired viewport size
                    Margin = new PdfMargins
                    {
                        Top = 20, // Set your desired margins
                        Left = 20,
                        Right = 20,
                        Bottom = 20
                    }
                };
                htmlConverter.ConverterSettings = settings;

                // Convert HTML to PDF
                using (var memoryStream = new MemoryStream())
                {
                    PdfDocument document = htmlConverter.Convert(htmlText, null);
                    document.Save(memoryStream);
                    document.Close(true);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }
        #region Install dependencies.
        private static void InstallLinuxPackages()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return;
            }
            FileAccessPermissions ExecutableFilePermissions = FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite | FileAccessPermissions.UserExecute |
           FileAccessPermissions.GroupRead | FileAccessPermissions.GroupExecute | FileAccessPermissions.OtherRead | FileAccessPermissions.OtherExecute;
            string assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //Install the dependencies packages for HTML to PDF conversion in Linux
            string shellFilePath = Path.Combine(assemblyDirectory);
            string tempBlinkDir = Path.GetTempPath();
            string dependenciesPath = Path.Combine(tempBlinkDir, "dependenciesInstall.sh");
            if (!File.Exists(dependenciesPath))
            {

                CopyFilesRecursively(shellFilePath, tempBlinkDir);
                var execPath = Path.Combine(tempBlinkDir, "dependenciesInstall.sh");
                if (File.Exists(execPath))
                {
                    var code = Chmod(execPath, ExecutableFilePermissions);
                    if (code != 0)
                    {
                        throw new Exception("Chmod operation failed");
                    }
                }

                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c " + execPath,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }
        #endregion

        #region Setup Blink Binaries

        private static string SetupBlinkBinaries()
        {
            string assemblyDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "runtimes", "linux", "native");
            string blinkAppDir = Path.Combine(assemblyDirectory);
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
            FileAccessPermissions ExecutableFilePermissions =
                FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite | FileAccessPermissions.UserExecute |
                FileAccessPermissions.GroupRead | FileAccessPermissions.GroupExecute | FileAccessPermissions.OtherRead |
                FileAccessPermissions.OtherExecute;
            string[] executableFiles = new string[] { "chrome", "chrome_sandbox" };
            foreach (string executable in executableFiles)
            {
                var execPath = Path.Combine(tempBlinkDir, executable);
                if (File.Exists(execPath))
                {
                    var code = Chmod(execPath, ExecutableFilePermissions);
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

        #endregion
    }
}
