// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP PUT method.</summary>
    public interface IPutMethodAttribute : IUpdateOperationAttribute, IOrderProviderAttribute
    {
    }
}
