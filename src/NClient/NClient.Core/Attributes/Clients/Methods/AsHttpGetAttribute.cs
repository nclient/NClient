namespace NClient.Core.Attributes.Clients.Methods
{
    public class AsHttpGetAttribute : AsHttpMethodAttribute
    {
        public AsHttpGetAttribute(string? template = null) : base(template)
        {
        }
    }
}
