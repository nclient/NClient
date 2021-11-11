// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class PartialUpdateOperationAttribute : OperationAttribute, IPartialUpdateOperationAttribute
    {
        public PartialUpdateOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
