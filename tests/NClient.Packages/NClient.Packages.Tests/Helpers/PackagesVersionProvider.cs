using System;
using System.Reflection;

namespace NClient.Packages.Tests.Helpers
{
    public class PackagesVersionProvider
    {
        public static string GetNew()
        {
            return $"{Environment.GetEnvironmentVariable("VersionPrefix")}-{Environment.GetEnvironmentVariable("VersionSuffix")}";
        }

        public static string GetCurrent<T>()
        {
            return Assembly.GetAssembly(typeof(T))?.GetName().Version?.ToString()
                ?? throw new Exception("Current assembly version not found.");
        }

        public static string GetCurrent(string name)
        {
            return Assembly.Load(name).GetName().Version?.ToString() 
                ?? throw new Exception("Current assembly version not found.");
        }
    }
}
