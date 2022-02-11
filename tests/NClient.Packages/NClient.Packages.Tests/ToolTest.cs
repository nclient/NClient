#if NET5_0
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Packages.Tests.Helpers;
using NUnit.Framework;

namespace NClient.Packages.Tests
{
    public class ToolTest
    {
        [Test]
        public async Task Install()
        {
            var installResult = await ExecuteBashCommandAsync(
                command: $"dotnet tool install --global dotnet-nclient --version {PackagesVersionProvider.GetNew()}", 
                timeout: 10.Seconds());
            
            installResult.Error.Should().BeEmpty();
            
            var gettingVersionResult = await ExecuteBashCommandAsync(
                command: "dotnet nclient --version",
                timeout: 5.Seconds());
            
            gettingVersionResult.Error.Should().BeEmpty();
            gettingVersionResult.Output.Should().MatchRegex("NClient.DotNetTool *.");
        }
        
        private async Task<(string? Output, string? Error)> ExecuteBashCommandAsync(string command, TimeSpan timeout)
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            var isWindowsOs = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = isWindowsOs ? "cmd.exe" : "/bin/bash",
                    Arguments = isWindowsOs ? "/C \"" + command + "\"" : "-c \"" + command + "\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            
            process.Start();
            await process.WaitForExitAsync(cancellationTokenSource.Token);

            var output = await process.StandardOutput.ReadToEndAsync();
            TestContext.WriteLine(output);
            var error = await process.StandardError.ReadToEndAsync();
            TestContext.WriteLine(error);
            return (output, error);
        }
    }
}
#endif
