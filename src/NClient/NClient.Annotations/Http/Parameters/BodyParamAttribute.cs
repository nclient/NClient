﻿// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Specifies that a parameter should be bound using the request body.</summary>
    public class BodyParamAttribute : ContentParamAttribute, IBodyParamAttribute
    {
    }
}
