using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TactBac.Processing.Function.Models;

namespace TactBac.Processing.Function
{
    public static class RequestProcessor
    {
        [FunctionName("RequestProcessor")]
        public static async void Run(
            [QueueTrigger("messages")]Payload payload,
            TraceWriter log
        )
        {
            log.Info($"C# Queue trigger function processed a queue message: {payload.Id}.");

            string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.Process);

            var client = new SendGridClient(apiKey);

            var message = MailHelper.CreateSingleEmailToMultipleRecipients(
                new EmailAddress("auto@tactbac.com", "TactBac Automated Email"),
                payload.Email.Select(e => new EmailAddress { Email = e }).ToList(),
                "Your Contacts Export",
                "Test e-mail message",
                "<h1>Test e-mail message</h1>"
            );

            if (payload.Formats.Contains("CSV"))
            {
                string csvContent = CreateCSVString(payload.Contacts);
                message.AddAttachment("export.csv", Base64Encode(csvContent));
            }
            if (payload.Formats.Contains("JSON"))
            {
                string jsonContent = CreateJSONString(payload.Contacts);
                message.AddAttachment("export.json", Base64Encode(jsonContent));
            }
            if (payload.Formats.Contains("XML"))
            {
                string xmlContent = CreateXMLString(payload.Contacts);
                message.AddAttachment("export.xml", Base64Encode(xmlContent));
            }

            await client.SendEmailAsync(message);
        }

        private static string CreateCSVString(IEnumerable<Contact> contacts)
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

        private static string CreateJSONString(IEnumerable<Contact> contacts)
        {
            return JsonConvert.SerializeObject(contacts);
        }

        private static string CreateXMLString(IEnumerable<Contact> contacts)
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

        private sealed class ContactMap : CsvClassMap<Contact>
        {
            public ContactMap()
            {
                Map(m => m.EmailAddress).Name("Email Address");
                Map(m => m.DisplayName).Name("Name");
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}