using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// ReSharper disable once CheckNamespace
namespace NClient.Models
{
    #pragma warning disable CS8644
    public class HeaderDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        public long? ContentLength { get; set; }

        public HeaderDictionary()
        {
        }
        
        public HeaderDictionary(IEnumerable<KeyValuePair<string, StringValues>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
    #pragma warning restore CS8644
}
