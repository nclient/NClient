using System.IO;
using System.Text;

namespace NClient.Models
{
    public interface IStreamContent
    {
        Stream Value { get; }
        public Encoding Encoding { get; }
        string ContentType { get; }
        string? Name { get; }
    }
}
