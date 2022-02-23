// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP POST method.</summary>
    public interface IPostMethodAttribute : ICreateOperationAttribute, IOrderProviderAttribute
    {
    }
}
