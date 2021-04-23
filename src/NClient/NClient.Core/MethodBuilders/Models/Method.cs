using NClient.Annotations;
using NClient.Annotations.Methods;

namespace NClient.Core.MethodBuilders.Models
{
    public class Method
    {
        public string Name { get; }
        public string ClientName { get; }
        public MethodAttribute Attribute { get; }
        public PathAttribute? PathAttribute { get; set; }
        public MethodParam[] Params { get; }
        
        public Method(string name, string clientName, MethodAttribute attribute, MethodParam[] @params)
        {
            Name = name;
            ClientName = clientName;
            Attribute = attribute;
            Params = @params;
        }
    }
}