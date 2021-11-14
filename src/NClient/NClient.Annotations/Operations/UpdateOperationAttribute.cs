// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class UpdateOperationAttribute : OperationAttribute, IUpdateOperationAttribute
    {
        public UpdateOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
