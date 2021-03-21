namespace NClient.Core.Attributes.Services.Methods
{
    public class ForHttpGetAttribute : ForHttpMethodAttribute
    {
        public ForHttpGetAttribute(string? template = null) : base(template)
        {
        }
    }
}
