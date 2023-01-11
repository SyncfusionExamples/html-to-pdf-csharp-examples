using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using System.IO;

namespace HTML_to_PDF_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create a new AmazonLambdaClient
            AmazonLambdaClient client = new AmazonLambdaClient("awsaccessKeyID", "awsSecreteAccessKey", RegionEndpoint.USEast1);

            //Create new InvokeRequest with the published function name
            InvokeRequest invoke = new InvokeRequest
            {
                FunctionName = "htmltopdf2",
                InvocationType = InvocationType.RequestResponse,
                Payload = "\" https://www.google.co.in/ \""
            };
            //Get the InvokeResponse from client InvokeRequest
            InvokeResponse response = client.Invoke(invoke);

            //Read the response stream
            var stream = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(stream);
            var serilizer = new JsonSerializer();
            var responseText = serilizer.Deserialize(reader);

            //Convert Base64String into PDF document
            byte[] bytes = Convert.FromBase64String(responseText.ToString());
            FileStream fileStream = new FileStream("Sample.pdf", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fileStream);
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            System.Diagnostics.Process.Start("Sample.pdf");
        }
    }
}
