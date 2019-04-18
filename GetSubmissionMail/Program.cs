//
// Author:
//   Tianjiao(Wang Genghuang) (https://github.com/Tianjiao)
//
// Copyright (c) 2019 Tianjiao(Wang Genghuang)
//
// Licensed under the MIT/X11 license.
//

using System;
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

            var client = new RestClient("https://report.netcraft.com/api/v1");

            var request = new RestRequest("test/submission/{uuid}/mail")
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            }; // Test server

            request.AddUrlSegment("uuid", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // replaces matching token in request.Resource

            request.AddHeader("Accept", "application/json");

            // Add HTTP headers
            request.AddHeader("User-Agent", "RESTNetcraft v0.1.1 Beta");

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

