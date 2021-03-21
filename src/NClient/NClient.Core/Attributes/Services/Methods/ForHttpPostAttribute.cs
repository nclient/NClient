namespace NClient.Core.Attributes.Services.Methods
{
    public class ForHttpPostAttribute : ForHttpMethodAttribute
    {
        public ForHttpPostAttribute(string? template = null) : base(template)
        {
        }
    }
}
