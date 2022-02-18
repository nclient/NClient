using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NClient.DotNetTool.Loaders
{
    public class NetworkLoader : ISpecificationLoader
    {
        private readonly Uri _uri;
        
        public NetworkLoader(Uri uri)
        {
            _uri = uri;
        }
        
        public async Task<string> LoadAsync()
        {
            using var client = new HttpClient();
            using var responseMessage = await client.GetAsync(_uri);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}