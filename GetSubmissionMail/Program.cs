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
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace GetSubmissionMail
{
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
                Resource = "test/submission/{uuid}/mail",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            }; // Test server

            request.AddUrlSegment("uuid", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // replaces matching uuid in request.Resource

            request.AddHeader("Accept", "application/json");

            // https://stackoverflow.com/questions/21779206/how-to-use-restsharp-with-async-await
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            try
            {
                var restResponse = await client.ExecuteTaskAsync(request, token);
                Console.WriteLine("HTTP Status Code is " + restResponse.StatusCode.ToString());
                if (!string.IsNullOrEmpty(restResponse.Content))
                    Console.WriteLine(restResponse.Content);
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

