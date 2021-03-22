namespace NClient.Core.Attributes.Clients.Methods
{
    public class AsHttpPostAttribute : AsHttpMethodAttribute
    {
        public AsHttpPostAttribute(string? template = null) : base(template)
        {
        }
    }
}
