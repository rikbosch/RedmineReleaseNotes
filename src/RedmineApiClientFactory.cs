using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using ReleaseNotesGenerator.Redmine;

namespace ReleaseNotesGenerator
{
    static internal class RedmineApiClientFactory
    {
        public static IRedmineApi CreateApiClient(Options options)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }
            var client = new HttpClient(new LoggingHandler(handler)) { BaseAddress = new Uri(options.RedmineUrl) };
            client.DefaultRequestHeaders.Add("X-Redmine-API-Key", options.ApiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return new RedmineApi(client);
        }
    }
}