using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Interfaces.NSwag;
using NClient.DotNetTool.Loaders;

namespace NClient.DotNetTool
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private const string HelpLogo = @"
                   _--~~--_
                 /~/_|  |_\~\
                |____________|
                |[][][][][][]|:=  .               Welcome to NClient!
              __| __         |__ \  ' .          /
             |  ||. |   ==   |  |  \    ' .     /
            (|  ||__|   ==   |  |)   \      '<
             |  |[] []  ==   |  |      \    '\|
             |  |____________|  |        \    |
             /__\            /__\          \ / \
              ~~              ~~
        ";

        public static async Task<int> Main(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<GenerateInterfaceOptions, object>(args);

            return await parserResult.MapResult(
                (GenerateInterfaceOptions options) => RunGenerateInterfaceAsync(options),
                _ => OnError(parserResult)    
            );
        }

        private static Task<int> OnError<T>(ParserResult<T> parserResult)
        {
            var helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.AddPreOptionsText(HelpLogo);

                return HelpText.DefaultParsingErrorsHandler(parserResult, h);
            }, _ => _);
            Console.WriteLine(helpText);
            return Task.FromResult(-1);
        }

        private static async Task<int> RunGenerateInterfaceAsync(GenerateInterfaceOptions opts)
        {
            var serviceProvider = BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
            try
            {
                var specification = await serviceProvider.GetRequiredService<ILoaderFactory>().Create(opts).Load();
                var result = await serviceProvider.GetRequiredService<FacadeGenerator>().GenerateAsync(opts, specification);
                await Save(opts, result);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Generations is over! Please, see {opts.OutputPath} for result!");
                return 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Generation error {e.Message}");
                return -1;
            }
        }

        private static async Task Save(GenerateInterfaceOptions opts, string sourceCode)
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

