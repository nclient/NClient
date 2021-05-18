using System;

namespace NClient.Core.Helpers
{
    public interface IGuidProvider
    {
        Guid Create();
    }

    public class GuidProvider : IGuidProvider
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}