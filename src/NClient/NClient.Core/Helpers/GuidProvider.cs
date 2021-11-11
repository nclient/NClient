using System;
using System.Diagnostics.CodeAnalysis;

namespace NClient.Core.Helpers
{
    internal interface IGuidProvider
    {
        Guid Create();
    }

    [SuppressMessage("ReSharper", "GuidNew")]
    internal class GuidProvider : IGuidProvider
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
