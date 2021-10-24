// ReSharper disable ConvertToConstant.Global

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
