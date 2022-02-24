using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Serialization.MessagePack
{
    public enum MimeType   
    {
        [Description("application/msgpack")]
        ProperType = 0,
        [Description("application/x-msgpack")]
        XType = 1,
        [Description("application/vnd.msgpack")]
        VendorType = 2
    }
}
