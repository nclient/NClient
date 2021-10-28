// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    #if !NETSTANDARD2_0
    public interface IPatchMethodAttribute : IPartialUpdateOperationAttribute, IOrderProviderAttribute
    {
    }
    #endif
}
