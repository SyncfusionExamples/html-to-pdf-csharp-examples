using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_to_PDF_Lambda_Docker_Container.Helper
{
    public class AWSHelper
    {
        public static async Task<byte[]> RunLambdaFunction(string html)
        {
            try
            {
                var AwsAccessKeyId = "AccessKeyId";
                var AwsSecretAccessKey = "SecretAccessKey";

                AmazonLambdaClient client = new AmazonLambdaClient(AwsAccessKeyId, AwsSecretAccessKey, Amazon.RegionEndpoint.USEast1);
                InvokeRequest invoke = new InvokeRequest
                {
                    FunctionName = "Functionname",
                    InvocationType = InvocationType.RequestResponse,
                    Payload = Newtonsoft.Json.JsonConvert.SerializeObject(html)
                };
                //Get the InvokeResponse from client InvokeRequest
                InvokeResponse response = await client.InvokeAsync(invoke);

                //Read the response stream
                Console.WriteLine($"Response: {response.LogResult}");
                Console.WriteLine($"Response: {response.StatusCode}");
                Console.WriteLine($"Response: {response.FunctionError}");
                var stream = new StreamReader(response.Payload);
                JsonReader reader = new JsonTextReader(stream);
                var serilizer = new JsonSerializer();
                var responseText = serilizer.Deserialize(reader);

                //Convert Base64String into PDF document
                return Convert.FromBase64String(responseText.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occured HTMLToPDFHelper: {ex}");
            }
            return Convert.FromBase64String("");
        }
    }
}
