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
        
        public async Task<string> Load()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync(_uri);
        }
    }
}