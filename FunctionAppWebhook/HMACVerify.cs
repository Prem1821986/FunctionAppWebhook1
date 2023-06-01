using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace FunctionAppWebhook
{
    public class HMACVerify
    {
        private const string DateTimeHeaderName = "x-chemoil-date";
        private const string ContentHashHeaderName = "x-chemoil-content-sha256";
        private const string HostHeaderName = "host";
        private const string AuthorizationHeaderName = "Authorization";
        private static TimeSpan MaxRequestAge = TimeSpan.FromSeconds(5);

        //Get secret from DB
        private string ChemoilWebhookTestSecret = HMACVerify.GetHMACSecret();


        public static async Task<bool> VerifySec(HttpRequest req, ILogger log)
        {
            bool match;
            try
            {
                var contentHash = req.Headers[ContentHashHeaderName].FirstOrDefault();
                if(await IsTamperedBody(req, contentHash))
                {
                    log.LogWarning($"Content Hash {contentHash} does not match");
                    return false;
                }

                var requestDate = req.Headers[DateTimeHeaderName].FirstOrDefault();
                if(IsReplayRequest(requestDate, MaxRequestAge, out var requestAge))
                {
                    if (requestAge == null)
                        log.LogWarning($"Possible replay attack: cannot parse request age from header {DateTimeHeaderName}");
                    else
                        log.LogWarning($"Possible replay attack: Request age {requestAge?.TotalSeconds} seconds exceeds the configured max request age of {MaxRequestAge.TotalSeconds} seconds");
                    return false;
                }

                var host = req.Headers[HostHeaderName].FirstOrDefault();
                var authorizationString = req.Headers[AuthorizationHeaderName].FirstOrDefault();
                var signedHeaderTemplate = authorizationString.Split("SignedHeaders=").Last().Split('&').FirstOrDefault();
                var signatureHash = authorizationString.Split("Signature=").LastOrDefault();

                if (contentHash == null || requestDate == null || host == null || authorizationString == null || signedHeaderTemplate == null || signatureHash == null)
                    return false;
                log.LogInformation("Could read headers...");
                var stringToVerify = signedHeaderTemplate.Replace(DateTimeHeaderName, requestDate).Replace(HostHeaderName, host).Replace(ContentHashHeaderName, contentHash);
                log.LogInformation($"String to verify: {stringToVerify}");

                //Secret check
                var secretByte = Encoding.UTF8.GetBytes(HMACVerify.GetHMACSecret());
                var hmac256 = new HMACSHA256(secretByte);
                var hashString = Convert.ToBase64String(hmac256.ComputeHash(Encoding.UTF8.GetBytes(stringToVerify)));
                match = hashString.Equals(signatureHash);
                log.LogInformation($"SignatureHash - " + signatureHash);
                log.LogInformation($"hashString - " + hashString);
                log.LogInformation(match.ToString());
                if (!match)
                    log.LogWarning($"String hash {hashString} does not match Signature hash {signatureHash}");
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                throw ex;
            }
            return match;
        }

        private static bool IsReplayRequest(string  requestDate, TimeSpan maxTimeSpan, out TimeSpan? requestAge)
        {
            requestAge = null;
            if (!DateTime.TryParseExact(requestDate, "R", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var dt))
                return true;
            requestAge = DateTime.UtcNow - dt;
            return requestAge.Value.TotalSeconds >= maxTimeSpan.TotalSeconds;
        }

        private static async Task<bool> IsTamperedBody(HttpRequest req, string contentHash)
        {
            req.Body.Position = 0;
            string requestBody = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
            return Convert.ToBase64String(hashedBytes) != contentHash;
        }

        public static string GetHMACSecret()
        {
            string responce = "";
            string uri = "https://localhost:3001/WebHookProcessing/GetHmacApi";
            WebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Headers.Add("Authorization", $"Basic {encoded}");
            HttpWebResponse res = null;
            res = (HttpWebResponse)request.GetResponse();

            using(Stream stm=res.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stm);
                responce = sr.ReadToEnd();
                sr.Close();
            }
            return responce;
        }
    }
}
