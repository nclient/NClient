namespace NClient.Annotations.Parameters
{
    /// <summary>
    /// Specifies that a parameter should be bound using the request headers.
    /// </summary>
    public class HeaderParamAttribute : ParamAttribute, INameProviderAttribute
    {
        /// <summary>
        /// Model name.
        /// </summary>
        public string? Name { get; set; }
    }
}
