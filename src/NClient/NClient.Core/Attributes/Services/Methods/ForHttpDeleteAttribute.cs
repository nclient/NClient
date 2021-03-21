namespace NClient.Core.Attributes.Services.Methods
{
    public class ForHttpDeleteAttribute : ForHttpMethodAttribute
    {
        public ForHttpDeleteAttribute(string? template = null) : base(template)
        {
        }
    }
}
