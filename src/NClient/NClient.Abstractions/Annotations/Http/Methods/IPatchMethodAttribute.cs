// ReSharper disable once CheckNamespace
// ReSharper disable once EmptyNamespace

namespace NClient.Annotations.Http
{
    #if !NETSTANDARD2_0
    public interface IPatchMethodAttribute : IPartialUpdateOperationAttribute, IOrderProviderAttribute
    {
    }
    #endif
}
