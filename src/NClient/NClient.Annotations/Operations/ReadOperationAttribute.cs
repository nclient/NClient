// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class ReadOperationAttribute : OperationAttribute, IReadOperationAttribute
    {
        public ReadOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
