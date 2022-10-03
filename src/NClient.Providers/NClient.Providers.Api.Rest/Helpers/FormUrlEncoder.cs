using System;
using System.Collections.Generic;
using System.Text;

namespace NClient.Providers.Api.Rest.Helpers
{
    public interface IFormUrlEncoder
    {
        byte[] GetContentByteArray(IEnumerable<KeyValuePair<string?, string?>> nameValueCollection);
    }
    public class FormUrlEncoder : IFormUrlEncoder
    {
        public byte[] GetContentByteArray(IEnumerable<KeyValuePair<string?, string?>> nameValueCollection)
        {
            var builder = new StringBuilder();
            foreach (var pair in nameValueCollection)
            {
                if (builder.Length > 0)
                    builder.Append('&');

                builder.Append(Encode(pair.Key));
                builder.Append('=');
                builder.Append(Encode(pair.Value));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
        
        private static string Encode(string? data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;
            
            // Escape spaces as '+'.
            return Uri.EscapeDataString(data).Replace("%20", "+");
        }
    }
}
