// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    // TODO: doc
    public class CheckOperationAttribute : OperationAttribute, ICheckOperationAttribute
    {
        public CheckOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}
