using System;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Versioning;

namespace NClient.Core.Interceptors.MethodBuilders.Models
{
    internal class Method
    {
        public string Name { get; }
        public string ClientName { get; }
        public MethodAttribute Attribute { get; }
        public UseVersionAttribute? UseVersionAttribute { get; set; }
        public PathAttribute? PathAttribute { get; set; }
        public HeaderAttribute[] HeaderAttributes { get; set; }
        public MethodParam[] Params { get; }

        public Method(string name, string clientName, MethodAttribute attribute, MethodParam[] @params)
        {
            Name = name;
            ClientName = clientName;
            Attribute = attribute;
            Params = @params;
            HeaderAttributes = Array.Empty<HeaderAttribute>();
        }
    }
}