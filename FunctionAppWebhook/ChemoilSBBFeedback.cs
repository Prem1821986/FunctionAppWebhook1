using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionAppWebhook.Model;
using System.Text;
using System.Net;

namespace FunctionAppWebhook
{
    public static class ChemoilSBBFeedback
    {
        [FunctionName("ChemoilSBBFeedback")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation("Data received from SBB - " + requestBody);
            log.LogInformation("C# HTTP trigger function processed a request.");
            var isData = HMACVerify.VerifySec(req, log);

            if(isData.Result == false)
            {
                log.LogInformation("Incoming data is unauthorized.");
                return new UnauthorizedResult();
            }

            log.LogInformation("Success: incoming data authenticated.");
            string status = req.Query["Geplant"];
            //dynamic data = JsonConvert.DeserializeObject(requestBody);

            WebhookEvent sBBFeedbackBL = JsonConvert.DeserializeObject<WebhookEvent>(requestBody.ToString());
            log.LogInformation("serializing data");
            dynamic temp = JsonConvert.SerializeObject(sBBFeedbackBL.data);
            byte[] datatobesent = Encoding.Default.GetBytes(temp);

            string uri = "https://localhost:3001/SBBChemoil/PostStatusFromSBB";

            WebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = datatobesent.Length;

            using var newStream = request.GetRequestStream();
            newStream.Write(datatobesent, 0, datatobesent.Length);

            HttpWebResponse res = null;
            try
            {
                log.LogInformation("calling API.");
                res = (HttpWebResponse)request.GetResponse();
                //string responseMessage = string.IsNullOrEmpty(status)
                //? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response." : $"{res}";
                //log.LogInformation(responseMessage);
                return new OkObjectResult(200);
            }
            catch(Exception e)
            {
                log.LogInformation(e.Message);
                var result = new ObjectResult(e.Message);
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
        }
    }
}
