using System;
using System.Collections.Generic;
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
        private static IServiceProvider _serviceProvider = null!;
        private static ILogger<Program> _logger = null!;

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
            _serviceProvider = BuildServiceProvider(generationOptions.LogLevel); 
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>(); 
            return RunFacadeGenerationAsync(generationOptions);
        }
        
        private static async Task<int> RunFacadeGenerationAsync(InterfaceGenerationOptions generationOptions)
        {
            try
            {
                var specification = await _serviceProvider.GetRequiredService<ILoaderFactory>().Create(generationOptions).Load();
                var result = await _serviceProvider.GetRequiredService<IFacadeGenerator>().GenerateAsync(generationOptions, specification);
                await _serviceProvider.GetRequiredService<ISaver>().SaveAsync(result, generationOptions.OutputPath);
                _logger.LogDone("Generations is over! Please, see {OutputPath} for result!", generationOptions.OutputPath);
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Generation error {Message}", e.Message);
                return -1;
            }
        }

        private static Task<int> HandleErrors<T>(ParserResult<T> parserResult, IEnumerable<Error> errors, bool showGreeting = false)
        {
            _serviceProvider = BuildServiceProvider(LogLevel.Trace);
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();

            bool IsHelpRequested(ErrorType errorType) => errorType is ErrorType.HelpVerbRequestedError or ErrorType.HelpRequestedError or ErrorType.NoVerbSelectedError;
            if (errors.Any(x => IsHelpRequested(x.Tag)))
                return OnHelpRequested(parserResult, showGreeting);
            return OnError(parserResult);
        }

        private static Task<int> OnHelpRequested<T>(ParserResult<T> parserResult, bool showGreeting = false)
        {
            var helpText = HelpText.AutoBuild(parserResult, helpText =>
            {
                helpText.Heading = showGreeting ? $"{HelpLogo}\n{helpText.Heading}" : helpText.Heading;
                helpText.AdditionalNewLineAfterOption = false;
                return helpText;
            }, _ => _, verbsIndex: true);
            
            _logger.LogInformation(helpText);
            return Task.FromResult(0);
        }
        
        private static Task<int> OnError<T>(ParserResult<T> parserResult)
        {
            var helpText = HelpText.AutoBuild(parserResult, helpText =>
            {
                helpText.AdditionalNewLineAfterOption = false;
                return HelpText.DefaultParsingErrorsHandler(parserResult, helpText);
            }, _ => _, verbsIndex: true);
            
            _logger.LogInformation(helpText);
            return Task.FromResult(-1);
        }

        private static IServiceProvider BuildServiceProvider(LogLevel logLevel)
        {
            return new ServiceCollection()
                .AddLogging(x => x
                    .AddConsole(opts => opts.FormatterName = nameof(SimpleConsoleFormatter))
                    .AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(logLevel))
                .AddSingleton<ILoaderFactory, LoaderFactory>()
                .AddSingleton<ISaver, FileSaver>()
                .AddSingleton(x => new NSwagFacadeGeneratorProvider().Create(x.GetRequiredService<ILogger<INClientFacadeGenerator>>()))
                .AddSingleton<IFacadeGenerator, FacadeGenerator>()
                .BuildServiceProvider();
        }
    }
}

