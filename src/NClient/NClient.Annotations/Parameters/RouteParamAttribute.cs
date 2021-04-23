using NClient.Annotations.Abstractions;

namespace NClient.Annotations.Parameters
{
    public class RouteParamAttribute : ParamAttribute, INameProviderAttribute
    {
        public string? Name { get; set; }
    }
}
