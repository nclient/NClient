// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class DeleteOperationAttribute : OperationAttribute, IDeleteOperationAttribute
    {
        public DeleteOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
