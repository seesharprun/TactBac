using CsvHelper;
using CsvHelper.Configuration;
using Markdig;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TactBac.Processing.Function.Models;

namespace TactBac.Processing.Function
{
    public static class RequestProcessor
    {
        [FunctionName("RequestProcessor")]
        public static async void Run(
            [QueueTrigger("messages")]Request request,
            TraceWriter log
        )
        {
            log.Info($"[{request.Id}] Dequeued conversion request.");
            
            Payload payload = await GetPayload(request);

            string emailApiKey = Environment.GetEnvironmentVariable("SendGridApiKey", EnvironmentVariableTarget.Process);

            var client = new SendGridClient(emailApiKey);

            Dictionary<string, string> attachments = new Dictionary<string, string>();

            foreach(Format format in payload.Formats)
            {
                attachments.Add(
                    format.ToString(),
                    await GetAttachmentLink(
                        CreateContentString(format, payload.Contacts),
                        GenerateFilename(request.Id, format.ToString().ToLower())
                    )
                );
            }
            
            var message = MailHelper.CreateSingleEmailToMultipleRecipients(
                new EmailAddress("auto@tactbac.com", "TactBac Automated Email"),
                payload.Email.Select(e => new EmailAddress { Email = e }).ToList(),
                "Your Contacts Export",
                BuildPlainTextResponse(attachments),
                BuildHtmlResponse(attachments)
            );

            await client.SendEmailAsync(message);

            log.Info($"[{request.Id}] E-mail message sent.");
        }

        private static CloudBlobClient GetClient()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            return storageAccount.CreateCloudBlobClient();
        }

        private static async Task<Payload> GetPayload(Request request)
        {
            CloudBlobClient blobClient = GetClient();

            CloudBlobContainer container = blobClient.GetContainerReference("requests");
            await container.CreateIfNotExistsAsync();

            string filename = request.FileLocation;
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            string payloadJsonString = await blockBlob.DownloadTextAsync();

            Payload payload = JsonConvert.DeserializeObject<Payload>(payloadJsonString);

            await blockBlob.DeleteIfExistsAsync();

            return payload;
        }

        private static async Task<string> GetAttachmentLink(string content, string filename)
        {
            CloudBlobClient blobClient = GetClient();

            CloudBlobContainer container = blobClient.GetContainerReference("attachments");
            
            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadTextAsync(content);

            return blockBlob.Uri.AbsoluteUri;
        }

        private static string CreateContentString(Format format, IEnumerable<Contact> contacts)
        {
            if (format == Format.CSV)
            {
                string csv = String.Empty;
                using (StringWriter stringWriter = new StringWriter())
                {
                    var csvWriter = new CsvWriter(stringWriter);
                    csvWriter.Configuration.RegisterClassMap<ContactMap>();
                    csvWriter.WriteRecords(contacts);
                    csv = stringWriter.ToString();
                }
                return csv;
            }
            else if (format == Format.XML)
            {
                string xml = String.Empty;
                using (StringWriter stringWriter = new StringWriter())
                {
                    var xmlSerializer = new XmlSerializer(typeof(List<Contact>));
                    xmlSerializer.Serialize(stringWriter, contacts);
                    xml = stringWriter.ToString();
                }
                return xml;
            }
            else if (format == Format.JSON)
            {
                return JsonConvert.SerializeObject(contacts);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

        private sealed class ContactMap : CsvClassMap<Contact>
        {
            public ContactMap()
            {
                Map(m => m.EmailAddress).Name("Email Address");
                Map(m => m.DisplayName).Name("Name");
            }
        }

        private static string GenerateFilename(Guid requestId, string extension)
        {
            return $"{requestId.ToString().Replace("-", String.Empty).ToLower()}-{DateTimeOffset.UtcNow.ToFileTime()}.{extension}";
        }

        private static string BuildHtmlResponse(Dictionary<string, string> attachments)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("# TactBac Export Complete");
            builder.AppendLine("**Thank you for using [TactBac](http://tactbac.com/).** Links to your exported contact files are included below. Please tell your friends about our app!");
            builder.AppendLine();
            builder.AppendLine(String.Join(Environment.NewLine, attachments.Select(a => $"- [{a.Key} Export]({a.Value})")));
            builder.AppendLine();
            builder.AppendLine("---");
            builder.AppendLine();
            builder.AppendLine("*The team at TactBac*");
            builder.AppendLine();
            builder.AppendLine("- [TactBac on iTunes](https://itunes.apple.com/us/app/tactbac/id1282805236)");
            builder.AppendLine("- [TactBac on Google Play](https://play.google.com/store/apps/details?id=com.tactbac)");

            return Markdown.ToHtml(builder.ToString());
        }

        private static string BuildPlainTextResponse(Dictionary<string, string> attachments)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("TactBac Export Complete");
            builder.AppendLine();
            builder.AppendLine("Thank you for using TactBac(http://tactbac.com/). Links to your exported contact files are included below. Please tell your friends about our app!");
            builder.AppendLine();
            builder.AppendLine(String.Join(Environment.NewLine, attachments.Select(a => $"- {a.Key} Export: {a.Value}")));
            builder.AppendLine();
            builder.AppendLine("Thanks,");
            builder.AppendLine();
            builder.AppendLine("The team at TactBac");
            builder.AppendLine();
            builder.AppendLine("TactBac on iTunes: https://itunes.apple.com/us/app/tactbac/id1282805236");
            builder.AppendLine("TactBac on Google Play: https://play.google.com/store/apps/details?id=com.tactbac");

            return builder.ToString();
        }
    }
}