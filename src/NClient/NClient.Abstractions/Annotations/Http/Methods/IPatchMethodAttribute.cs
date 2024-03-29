﻿// ReSharper disable once CheckNamespace
// ReSharper disable once EmptyNamespace

namespace NClient.Annotations.Http
{
    #if !NETSTANDARD2_0
    /// <summary>Identifies an action that supports the HTTP PATCH method.</summary>
    public interface IPatchMethodAttribute : IPartialUpdateOperationAttribute, IOrderProviderAttribute
    {
    }
    #endif
}
