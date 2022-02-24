﻿// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP GET method.</summary>
    public class GetMethodAttribute : ReadOperationAttribute, IGetMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>Initializes a new <see cref="GetMethodAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public GetMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}
