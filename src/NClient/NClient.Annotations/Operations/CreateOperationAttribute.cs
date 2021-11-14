// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class CreateOperationAttribute : OperationAttribute, ICreateOperationAttribute
    {
        public CreateOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
