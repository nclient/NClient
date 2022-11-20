using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NClient.Common.Helpers
{
    // ReSharper disable once UnusedType.Global
    internal static class ContentExtensions
    {
        internal static async Task<string> ReadToEndAsync(this Stream stream, Encoding? encoding, CancellationToken cancellationToken = default)
        {
            var encodingOrDefault = encoding ?? Encoding.UTF8;
            
            if (stream is MemoryStream memoryStream)
                return GetString(memoryStream, encodingOrDefault);

            memoryStream = new MemoryStream();
            #if NETSTANDARD2_0 || NETFRAMEWORK
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            #else
            await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
            #endif
            return GetString(memoryStream, encodingOrDefault);
        }

        private static string GetString(MemoryStream memoryStream, Encoding encoding)
        {
            var result = encoding.GetString(memoryStream.ToArray());
            memoryStream.Position = 0;
            return result;
        }
    }
}
