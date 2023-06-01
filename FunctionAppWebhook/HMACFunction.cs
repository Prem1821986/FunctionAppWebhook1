using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace FunctionAppWebhook
{
    public static class HMACFunction
    {
        [FunctionName("HMACFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
           // string status = req.Query["Geplant"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            name = name ?? data?.name;
           // status = status ?? data?.status;

            //sending data to API
            dynamic temp = JsonConvert.SerializeObject(data);
            byte[] datatobesent = Encoding.ASCII.GetBytes(temp);

            string uri = "https://localhost:44377/weatherforecast/Posthmac";

            WebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = datatobesent.Length;

            HttpWebResponse res = null;
            res = (HttpWebResponse)request.GetResponse();

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
