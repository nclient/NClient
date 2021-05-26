namespace NClient.Annotations.Methods
{
#if !NETSTANDARD2_0
    public class PatchMethodAttribute : MethodAttribute
    {
        public PatchMethodAttribute(string? template = null) : base(template)
        {
        }
    }
#endif
}
