using System.ComponentModel;

namespace NClient.Providers.Serialization.MessagePack
{
    public enum MIMEType   
    {
        [Description("application/msgpack")]
        ProperType = 0,
        [Description("application/x-msgpack")]
        XType = 1,
        [Description("application/vnd.msgpack")]
        VendorType = 2
    }
}
