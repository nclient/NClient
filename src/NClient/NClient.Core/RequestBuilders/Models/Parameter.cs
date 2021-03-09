using System;

namespace NClient.Core.RequestBuilders.Models
{
    internal record Parameter(string Name, object? Value, Attribute Attribute);
}
