using System;

namespace NClient.Common.Helpers
{
    internal static class Converters
    {
        public static string GetString(byte[] source)
        {
            var chars = new char[source.Length];
            Buffer.BlockCopy(source, 0, chars, 0, source.Length);
            return new string(chars);
        }

        public static byte[] GetBytes(string source)
        {
            byte[] result = new byte[source.Length];
            Buffer.BlockCopy(source.ToCharArray(), 0, result, 0, result.Length);
            return result;
        }
    }
}
