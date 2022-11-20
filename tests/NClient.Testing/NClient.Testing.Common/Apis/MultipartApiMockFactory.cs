using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions.Extensions;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Types;
using WireMock.Util;

namespace NClient.Testing.Common.Apis
{
    public class MultipartApiMockFactory
    {
        private static readonly Regex CleanupRegex = 
            new(@"[^0-9a-zA-Z :\-\n]+", 
                RegexOptions.Multiline | RegexOptions.Compiled, 
                matchTimeout: 1.Seconds());
        
        private static readonly Regex MultipartRegex = 
            new(@"(--.*)$\n^((Content-Encoding:(.*))$\n^)?((Content-Type:(.*))$\n^)?((Content-Disposition:(.*))$\n^)?((Content-Length:(.*))$\n^)?$\n^(.*)$\n", 
                RegexOptions.Multiline | RegexOptions.Compiled, 
                matchTimeout: 1.Seconds()); 
        
        public static IWireMockServer MockPostMethod(
            IDictionary<string, object> keyValues,
            byte[] requestBytes)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/multipart")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", new RegexMatcher("multipart/mixed; boundary=\".*\""))
                    .WithBody((Func<IBodyData, bool>) (x => IsMultipartBodyMatch(x, shouldCount: 2)))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
        
        public static IWireMockServer MockPostMethod(
            byte[] requestBytes1,
            byte[] requestBytes2,
            (string Key, string Value) header)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/multipart")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", new RegexMatcher("multipart/mixed; boundary=\".*\""))
                    .WithBody((Func<IBodyData, bool>) (x => IsMultipartBodyMatch(x, shouldCount: 2)))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithHeader(header.Key, header.Value)
                    .WithStatusCode(200));

            return api;
        }

        // TODO: Write match properly
        private static bool IsMultipartBodyMatch(IBodyData bodyData, int shouldCount)
        {
            if (bodyData.DetectedBodyTypeFromContentType is not BodyType.MultiPart)
                return false;

            var bodyString = Encoding.UTF8.GetString(bodyData.BodyAsBytes);
            var cleanupBodyString = CleanupRegex.Replace(bodyString, replacement: "");
            var multiparts = MultipartRegex.Matches(cleanupBodyString);
            if (multiparts.Count != shouldCount)
                return false;

            return true;
        }
    }
}
