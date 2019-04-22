//
// Author:
//   Tianjiao(Wang Genghuang) (https://github.com/Tianjiao)
//
// Copyright (c) 2019 Tianjiao(Wang Genghuang)
//
// Licensed under the MIT/X11 license.
//

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace GetMailScreenshot
{
    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Welcome to RESTNetcraft - a REST Client for Netcraft!");

            var client = new RestClient()
            {
                BaseUrl = new Uri("https://report.netcraft.com/api/v1"),
                UserAgent = "RESTNetcraft v0.1.3 Beta"
            };

            var request = new RestRequest()
            {
                Resource = "test/submission/{uuid}/mail/screenshot",
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            }; // Test server

            request.AddUrlSegment("uuid", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // replaces matching uuid in request.Resource

            request.AddHeader("Accept", "application/json");

            // https://stackoverflow.com/questions/21779206/how-to-use-restsharp-with-async-await
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            // https://stackoverflow.com/questions/29123291/how-to-use-restsharp-to-download-file
            try
            {
                var restResponse = client.DownloadData(request);
                await File.WriteAllBytesAsync(@"D:\MailScreenshot1.png", restResponse, token);
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
