//
// Author:
//   Tianjiao(Wang Genghuang) (https://github.com/Tianjiao)
//
// Copyright (c) 2019 Tianjiao(Wang Genghuang)
//
// Licensed under the MIT/X11 license.
//

using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using MimeKit;

namespace ReportMail
{
    internal class MailObject
    {
        public string email { get; set; }
        public string message { get; set; }
    }

    internal class UrlReportFeedbackObject
    {
        public string message { get; set; }
        public string uuid { get; set; }
    }

    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Welcome to RESTNetcraft - a REST Client for Netcraft!");
            
            var client = new RestClient("https://report.netcraft.com/api/v1");
            
            var request = new RestRequest("test/report/mail"); // Test server
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.POST;
            
            var messageSent = new MailObject();
            messageSent.email = "demo222@gmail.com";
            var messageContent = MimeMessage.Load(@"D:\sample.eml");
            messageSent.message = messageContent.ToString();

            request.AddHeader("Accept", "application/json");

            request.AddJsonBody(messageSent);

            // Add HTTP headers
            request.AddHeader("User-Agent", "RESTNetcraft v0.1.1 Beta");

            // https://stackoverflow.com/questions/21779206/how-to-use-restsharp-with-async-await
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            try
            {
                var restResponse = await client.ExecuteTaskAsync<UrlReportFeedbackObject>(request, token);
                Console.WriteLine("HTTP Status Code is " + restResponse.StatusCode.ToString());
                if (!string.IsNullOrEmpty(restResponse.Content))
                {
                    Console.WriteLine(restResponse.Content);
                    Console.WriteLine("restResponse message is " + restResponse.Data.message);
                    Console.WriteLine("restResponse uuid is " + restResponse.Data.uuid);
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

        }
    }
}
