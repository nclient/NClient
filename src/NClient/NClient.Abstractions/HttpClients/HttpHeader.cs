﻿using NClient.Common.Helpers;

namespace NClient.Abstractions.HttpClients
{
    // TODO: rename
    /// <summary>
    /// The container for HTTP header data.
    /// </summary>
    public record HttpHeader2 : IHttpHeader2
    {
        public string Name { get; }
        public string Value { get; }

        public HttpHeader2(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}
