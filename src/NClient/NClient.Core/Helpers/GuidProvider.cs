using System;
using System.Diagnostics.CodeAnalysis;

namespace NClient.Core.Helpers
{
    public interface IGuidProvider
    {
        Guid Create();
    }

    [SuppressMessage("ReSharper", "GuidNew")]
    public class GuidProvider : IGuidProvider
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
