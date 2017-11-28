using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
            [Queue("messages")] IAsyncCollector<Request> output, 
            TraceWriter log
        )
        {
            log.Info("Received conversion request.");

            string requestJsonString = await req.Content.ReadAsStringAsync();

            string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("requests");
            await container.CreateIfNotExistsAsync();

            string filename = $"{DateTimeOffset.UtcNow.ToFileTime()}-{Guid.NewGuid().ToString().Replace("-", String.Empty).ToLower()}.json";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadTextAsync(requestJsonString);

            Request request = new Request
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTimeOffset.UtcNow,
                FileLocation = filename
            };

            await output.AddAsync(request);
            log.Info($"[{request.Id}] Enqueued conversion request.");

            return req.CreateResponse(HttpStatusCode.OK, "Request queued successfully");
        }
    }
}