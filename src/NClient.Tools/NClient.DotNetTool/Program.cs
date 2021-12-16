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
using NClient.CodeGeneration.Interfaces.NSwag;
using NClient.DotNetTool.Loaders;
using NClient.DotNetTool.Logging;
using NClient.DotNetTool.Options;

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
                with.IgnoreUnknownArguments = true;
            });
            
            var mainParserResult = parser.ParseArguments<FacadeOptions, int>(args);
            return await mainParserResult.MapResult(
                (FacadeOptions _) =>
                {
                    var facadeParserResult = parser.ParseArguments<FacadeOptions.GenerationOptions, int>(args.Skip(1));
                    return facadeParserResult.MapResult(
                        (FacadeOptions.GenerationOptions generateOptions) => HandleFacadeGenerationOptions(generateOptions), 
                        facadeErrors => HandleErrors(facadeParserResult, facadeErrors, showGreeting: false));
                },
                errors =>
                {
                    var errorArrays = errors as Error[] ?? errors.ToArray();
                    if (errorArrays.First().Tag != ErrorType.HelpRequestedError)
                        return HandleErrors(mainParserResult, errorArrays, showGreeting: true);
                    
                    var facadeParserResult = parser.ParseArguments<FacadeOptions.GenerationOptions, int>(args.Skip(1));
                    return facadeParserResult.MapResult(
                        (FacadeOptions.GenerationOptions generateOptions) => HandleFacadeGenerationOptions(generateOptions), 
                        facadeErrors => HandleErrors(facadeParserResult, facadeErrors, showGreeting: false));
                });
        }

        private static Task<int> HandleFacadeGenerationOptions(FacadeOptions.GenerationOptions generationOptions)
        {
            _serviceProvider = BuildServiceProvider(generationOptions.LogLevel); 
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>(); 
            return RunFacadeGenerationAsync(generationOptions);
        }

        private static Task<int> HandleErrors(ParserResult<object> parserResult, IEnumerable<Error> errors, bool showGreeting = false)
        {
            _serviceProvider = BuildServiceProvider(LogLevel.Trace);
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
                    
            if (errors.Any(x => x.Tag is ErrorType.HelpVerbRequestedError or ErrorType.NoVerbSelectedError))
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

        private static async Task<int> RunFacadeGenerationAsync(FacadeOptions.GenerationOptions generationOptions)
        {
            try
            {
                var specification = await _serviceProvider.GetRequiredService<ILoaderFactory>().Create(generationOptions).Load();
                var result = await _serviceProvider.GetRequiredService<IFacadeGenerator>().GenerateAsync(generationOptions, specification);
                await Save(generationOptions, result);
                _logger.LogDone("Generations is over! Please, see {OutputPath} for result!", generationOptions.OutputPath);
                return 0;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                _logger.LogError(e, "Generation error {Message}", e.Message);
                return -1;
            }
        }

        private static async Task Save(FacadeOptions.GenerationOptions generationOptions, string sourceCode)
        {
            if (File.Exists(generationOptions.OutputPath))
                File.Delete(generationOptions.OutputPath);

            await File.WriteAllTextAsync(generationOptions.OutputPath, sourceCode);
        }

        private static IServiceProvider BuildServiceProvider(LogLevel logLevel)
        {
            return new ServiceCollection()
                .AddLogging(x => x
                    .AddConsole(opts => opts.FormatterName = nameof(SimpleConsoleFormatter))
                    .AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>()
                    .SetMinimumLevel(logLevel))
                .AddSingleton<ILoaderFactory, LoaderFactory>()
                .AddSingleton(x => new NSwagFacadeGeneratorProvider().Create(x.GetRequiredService<ILogger<INClientFacadeGenerator>>()))
                .AddSingleton<IFacadeGenerator, FacadeGenerator>()
                .BuildServiceProvider();
        }
    }
}

