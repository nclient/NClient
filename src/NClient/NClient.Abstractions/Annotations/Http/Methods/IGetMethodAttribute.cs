// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP GET method.</summary>
    public interface IGetMethodAttribute : IReadOperationAttribute, IOrderProviderAttribute
    {
    }
}
