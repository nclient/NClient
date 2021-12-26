using System.IO;

namespace NClient.Common.Helpers
{
    public static class StreamExtensions
    {
        public static void ResetStreamReader(this StreamReader? sr, long position = 0)
        {
            if (sr is null) return;
            sr.BaseStream.Position = position;
        }
    }
}
