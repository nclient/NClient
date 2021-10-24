// ReSharper disable ConvertToConstant.Global
// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public enum RequestType : byte
    {
        Read,
        Update,
        Delete,
        Create,
        Head,
        Trace,
        Patch,
        Connect,
        Options,

        Custom,

        None = byte.MaxValue
    }
}
