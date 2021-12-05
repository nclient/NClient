using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Providers.NSwag;
using NClient.DotNetTool.Loaders;

namespace NClient.DotNetTool
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async opts =>
                    {
                        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
                        try
                        {
                            var specification = await serviceProvider.GetRequiredService<ILoaderFactory>().Create(opts).Load();
                            var result = await serviceProvider.GetRequiredService<FacadeGenerator>().GenerateAsync(opts, specification);
                            await Save(opts, result);
                            return 0;
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"Generation error {e.Message}");
                            return -1;
                        }
                    },
                    _ => Task.FromResult(-1));
        }

        private static async Task Save(CommandLineOptions opts, string sourceCode)
        {
            if (File.Exists(opts.OutputPath))
                File.Delete(opts.OutputPath);

            await File.WriteAllTextAsync(opts.OutputPath, sourceCode);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                .AddSingleton<ILoaderFactory>(_ => new LoaderFactory())
                .AddSingleton(serviceProvider =>
                {
                    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<INClientInterfaceGenerator>();
                    return new NSwagInterfaceGeneratorProvider().Create(logger);
                })
                .AddSingleton<FacadeGenerator>()
                .BuildServiceProvider();
        }
    }    
}

