using NClient.Annotations.Abstractions;

namespace NClient.Annotations.Parameters
{
    public class HeaderParamAttribute : ParamAttribute, INameProviderAttribute
    {
        public string? Name { get; set; }
    }
}
