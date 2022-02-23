// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP HEAD method.</summary>
    public interface IHeadMethodAttribute : ICheckOperationAttribute, IOrderProviderAttribute
    {
    }
}
