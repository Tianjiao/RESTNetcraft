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
using RestSharp;

namespace GetMailScreenshot
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to RESTNetcraft - a REST Client for Netcraft!");

            var client = new RestClient("https://report.netcraft.com/api/v1");

            var request = new RestRequest("test/submission/{uuid}/mail/screenshot")
            {
                RequestFormat = DataFormat.Json,
                Method = Method.GET
            }; // Test server

            request.AddUrlSegment("uuid", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // replaces matching token in request.Resource

            request.AddHeader("Accept", "application/json");

            // Add HTTP headers
            request.AddHeader("User-Agent", "RESTNetcraft v0.1.1 Beta");

            // https://stackoverflow.com/questions/29123291/how-to-use-restsharp-to-download-file
            try
            {
                var restResponse = client.DownloadData(request);
                if (restResponse.Length > 0)
                    File.WriteAllBytes(@"D:\MailScreenshot1.png", restResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
