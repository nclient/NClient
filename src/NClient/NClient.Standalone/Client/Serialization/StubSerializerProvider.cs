﻿using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Client.Serialization
{
    internal class StubSerializerProvider : ISerializerProvider
    {
        public ISerializer Create()
        {
            return new StubSerializer();
        }
    }
}
