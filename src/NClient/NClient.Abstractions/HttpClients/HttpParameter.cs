﻿using NClient.Common.Helpers;

namespace NClient.Abstractions.HttpClients
{
    public class HttpParameter
    {
        public string Name { get; }
        public object? Value { get; }

        public HttpParameter(string name, object? value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));

            Name = name;
            Value = value;
        }
    }
}
