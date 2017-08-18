using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TactBac.Processing.Function.Models;

namespace TactBac.Processing.Function
{
    public static class ReceiveRequest
    {
        [FunctionName("ReceiveRequest")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, 
            [Queue("messages")] IAsyncCollector<Payload> output, 
            TraceWriter log
        )
        {
            log.Info("C# HTTP trigger function processed a request.");

            string jsonString = await req.Content.ReadAsStringAsync();
            Payload payload = JsonConvert.DeserializeObject<Payload>(jsonString);
            payload.Id = Guid.NewGuid();

            await output.AddAsync(payload);

            log.Info($"C# HTTP trigger function enqueued a request: {payload.Id}.");
            return req.CreateResponse(HttpStatusCode.OK, "Request queued successfully");
        }
    }
}