//
// Author:
//   Tianjiao(Wang Genghuang) (https://github.com/Tianjiao)
//
// Copyright (c) 2019 Tianjiao(Wang Genghuang)
//
// Licensed under the MIT/X11 license.
//

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RestSharp;
using MimeKit;

namespace ReportMail
{
    
    internal class MailObject
    {
        // Require that the Email is not null.
        // Use standard validation error.
        [Required()]
        public string email { get; set; }
        [Required()]
        public string message { get; set; }
    }

    internal class UrlReportFeedbackObject
    {
        public string Message { get; set; }
        public string Uuid { get; set; }
    }

    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Welcome to RESTNetcraft - a REST Client for Netcraft!");

            // https://stackoverflow.com/questions/28286086/default-securityprotocol-in-net-4-5
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient()
            {
                BaseUrl = new Uri("https://report.netcraft.com/api/v1"),
                UserAgent = "RESTNetcraft v0.1.5 Beta"
            };

            var request = new RestRequest()
            {
                Resource = "test/report/mail",
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            }; // Test server

            var messageSent = new MailObject
            {
                email = "example@netcraft.com"
            };
            var messageContent = MimeMessage.Load(@"D:\sample.eml");
            messageSent.message = messageContent.ToString();

            request.AddHeader("Accept", "application/json");

            request.AddJsonBody(messageSent);

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
                    Console.WriteLine("restResponse message is " + restResponse.Data.Message);
                    Console.WriteLine("restResponse uuid is " + restResponse.Data.Uuid);
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
