using System;
using System.Diagnostics;
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
            var assembly = Assembly.GetAssembly(typeof(T))?.Location
                ?? throw new Exception("Current assembly version not found.");
            return FileVersionInfo.GetVersionInfo(assembly).ProductVersion
                ?? throw new Exception("Product version not found in assembly.");
        }

        public static string GetCurrent(string name)
        {
            var assembly = Assembly.Load(name).Location;
            return FileVersionInfo.GetVersionInfo(assembly).ProductVersion
                ?? throw new Exception("Product version not found in assembly.");
        }
    }
}
