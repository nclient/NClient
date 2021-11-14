// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class CustomOperationAttribute : OperationAttribute, ICustomOperationAttribute
    {
        public CustomOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
