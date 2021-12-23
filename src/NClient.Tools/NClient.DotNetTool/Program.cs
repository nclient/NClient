using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            
            var mainParserResult = parser.ParseArguments<FacadeOptions, ClientOptions, int>(args);
            return await mainParserResult.MapResult(
                (FacadeOptions _) =>
                {
                    var facadeParserResult = parser.ParseArguments<FacadeOptions.GenerationOptions, int>(args.Skip(1));
                    return facadeParserResult.MapResult(
                        (FacadeOptions.GenerationOptions generateOptions) => HandleFacadeGenerationOptions(generateOptions), 
                        facadeErrors => HandleErrors(facadeParserResult, facadeErrors, showGreeting: false));
                },
                (ClientOptions _) =>
                {
                    var clientParserResult = parser.ParseArguments<ClientOptions.GenerationOptions, int>(args.Skip(1));
                    return clientParserResult.MapResult(
                        (ClientOptions.GenerationOptions generateOptions) => HandleFacadeGenerationOptions(generateOptions), 
                        facadeErrors => HandleErrors(clientParserResult, facadeErrors, showGreeting: false));
                },
                errors =>
                {
                    var errorArrays = errors as Error[] ?? errors.ToArray();
                    bool IsFirstVerbError(ErrorType errorType) => args.Length == 0 || errorType is not ErrorType.HelpRequestedError && errorType is not ErrorType.UnknownOptionError;
                    if (IsFirstVerbError(errorArrays.First().Tag))
                        return HandleErrors(mainParserResult, errorArrays, showGreeting: true);

                    var facadeVerb = typeof(FacadeOptions).GetCustomAttribute<VerbAttribute>()?.Name
                        ?? throw new InvalidOperationException("The attribute with the verb was not found for the facade.");
                    if (args.First() == facadeVerb)
                    {
                        var facadeParserResult = parser.ParseArguments<FacadeOptions.GenerationOptions, int>(args.Skip(1));
                        return facadeParserResult.MapResult(
                            _ => Task.FromResult(0),
                            facadeErrors => HandleErrors(facadeParserResult, facadeErrors, showGreeting: false));
                    }

                    var clientVerb = typeof(ClientOptions).GetCustomAttribute<VerbAttribute>()?.Name
                        ?? throw new InvalidOperationException("The attribute with the verb was not found for the facade.");
                    if (args.First() == clientVerb)
                    {
                        var clientParserResult = parser.ParseArguments<ClientOptions.GenerationOptions, int>(args.Skip(1));
                        return clientParserResult.MapResult(
                            _ => Task.FromResult(0),
                            clientErrors => HandleErrors(clientParserResult, clientErrors, showGreeting: false));
                    }

                    throw new NotSupportedException("Invalid arguments.");
                });
        }

        private static Task<int> HandleFacadeGenerationOptions(CommonGenerationOptions generationOptions)
        {
            _serviceProvider = BuildServiceProvider(generationOptions.LogLevel); 
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>(); 
            return RunFacadeGenerationAsync(generationOptions);
        }
        
        private static async Task<int> RunFacadeGenerationAsync(CommonGenerationOptions generationOptions)
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

