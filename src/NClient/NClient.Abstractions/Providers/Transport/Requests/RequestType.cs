// ReSharper disable ConvertToConstant.Global
// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public enum RequestType : byte
    {
        Get,
        Put,
        Delete,
        Post,
        Head,
        Trace,
        Patch,
        Connect,
        Options,

        Custom,

        None = byte.MaxValue
    }
}
