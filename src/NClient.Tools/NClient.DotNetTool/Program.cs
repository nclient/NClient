using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Facades.NSwag;
using NClient.DotNetTool.Loaders;
using NClient.DotNetTool.Logging;
using NClient.DotNetTool.Options;
using NClient.DotNetTool.Savers;

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
       ~~              ~~";

        public static async Task<int> Main(string[] args)
        {
            var parser = new Parser(with =>
            {
                with.HelpWriter = null;
                with.IgnoreUnknownArguments = true;
            });
            
            var mainParserResult = parser.ParseArguments<GenerationOptions, int>(args);
            return await mainParserResult.MapResult(
                (GenerationOptions _) =>
                {
                    var generationParserResult = parser.ParseArguments<GenerationOptions.FacadeOptions, GenerationOptions.ClientOptions, int>(args.Skip(1));
                    return generationParserResult.MapResult(
                        (GenerationOptions.FacadeOptions facadeGenerationOptions) => HandleFacadeGenerationOptions(facadeGenerationOptions), 
                        (GenerationOptions.ClientOptions clientGenerationOptions) => HandleFacadeGenerationOptions(clientGenerationOptions), 
                        generationErrors => HandleErrors(generationParserResult, generationErrors, showGreeting: false));
                },
                errors =>
                {
                    var errorArrays = errors as Error[] ?? errors.ToArray();
                    bool IsNoVerbError(ErrorType errorType) => args.Length == 0 || errorType is not ErrorType.HelpRequestedError && errorType is not ErrorType.UnknownOptionError;
                    if (IsNoVerbError(errorArrays.First().Tag))
                        return HandleErrors(mainParserResult, errorArrays, showGreeting: true);

                    var generationParserResult = parser.ParseArguments<GenerationOptions.FacadeOptions, GenerationOptions.ClientOptions, int>(args.Skip(1));
                    return generationParserResult.MapResult(
                        _ => Task.FromResult(0),
                        generationErrors => HandleErrors(generationParserResult, generationErrors, showGreeting: false));
                });
        }

        private static Task<int> HandleFacadeGenerationOptions(InterfaceGenerationOptions generationOptions)
        {
            using var serviceProvider = BuildServiceProvider(generationOptions.LogLevel);
            return RunFacadeGenerationAsync(generationOptions, serviceProvider);
        }
        
        private static async Task<int> RunFacadeGenerationAsync(InterfaceGenerationOptions generationOptions, ServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            
            try
            {
                var loaderFactory = serviceProvider.GetRequiredService<ILoaderFactory>();
                var facadeGenerator = serviceProvider.GetRequiredService<IFacadeGenerator>();
                var saver = serviceProvider.GetRequiredService<ISaver>();
                
                var specification = await loaderFactory.Create(generationOptions).LoadAsync();
                var result = await facadeGenerator.GenerateAsync(generationOptions, specification);
                await saver.SaveAsync(result, generationOptions.OutputPath);
                logger.LogDone("Generations is over. Please, see '{OutputPath}' for result", Path.GetFullPath(generationOptions.OutputPath));
                return 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Generation error: {Message}", e.Message);
                return -1;
            }
        }

        private static Task<int> HandleErrors<T>(ParserResult<T> parserResult, IEnumerable<Error> errors, bool showGreeting = false)
        {
            using var serviceProvider = BuildServiceProvider(LogLevel.Trace);
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            bool IsHelpRequested(ErrorType errorType) => errorType is ErrorType.HelpVerbRequestedError or ErrorType.HelpRequestedError or ErrorType.NoVerbSelectedError;
            if (errors.Any(x => IsHelpRequested(x.Tag)))
                return OnHelpRequested(parserResult, showGreeting, logger);
            return OnError(parserResult, logger);
        }

        private static Task<int> OnHelpRequested<T>(ParserResult<T> parserResult, bool showGreeting, ILogger logger)
        {
            var helpText = HelpText.AutoBuild(parserResult, helpText =>
            {
                helpText.Heading = showGreeting ? $"{HelpLogo}\n{helpText.Heading}" : helpText.Heading;
                helpText.AdditionalNewLineAfterOption = false;
                return helpText;
            }, _ => _, verbsIndex: true);
            
            logger.LogInformation("{HelpText}", helpText);
            return Task.FromResult(0);
        }
        
        private static Task<int> OnError<T>(ParserResult<T> parserResult, ILogger logger)
        {
            var helpText = HelpText.AutoBuild(parserResult, helpText =>
            {
                helpText.AdditionalNewLineAfterOption = false;
                return HelpText.DefaultParsingErrorsHandler(parserResult, helpText);
            }, _ => _, verbsIndex: true);
            
            logger.LogInformation("{HelpText}", helpText);
            return Task.FromResult(-1);
        }

        private static ServiceProvider BuildServiceProvider(LogLevel logLevel)
        {
            return new ServiceCollection()
                .AddLogging(x => x
                    .AddConsole(opts => opts.FormatterName = nameof(ToolConsoleFormatter))
                    .AddConsoleFormatter<ToolConsoleFormatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(logLevel))
                .AddSingleton<ILoaderFactory, LoaderFactory>()
                .AddSingleton<ISaver, FileSaver>()
                .AddSingleton(x => new NSwagFacadeGeneratorProvider().Create(x.GetRequiredService<ILogger<INClientFacadeGenerator>>()))
                .AddSingleton<IFacadeGenerator, FacadeGenerator>()
                .BuildServiceProvider();
        }
    }
}

