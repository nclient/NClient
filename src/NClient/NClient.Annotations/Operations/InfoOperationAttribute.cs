// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class InfoOperationAttribute : OperationAttribute, IInfoOperationAttribute
    {
        public InfoOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
