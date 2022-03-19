using System;
using System.Linq;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using NClient.Benchmark.Client.JsonClient;
using NClient.Benchmark.Client.JsonHttpResponseClient;
using NClient.Benchmark.Client.JsonSourceGeneratorClient;
using NClient.Benchmark.Client.PrimitiveClient;
using NClient.Benchmark.Client.PrimitiveHttpResponseClient;

namespace NClient.Benchmark.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var summaries = new[]
            {
                new Lazy<Summary>(() => BenchmarkRunner.Run<PrimitiveClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<PrimitiveHttpResponseClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<JsonSourceGeneratorClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<JsonClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<JsonHttpResponseClientBenchmark>())
            };
            
            var logger = ConsoleLogger.Default;
            summaries.Select(x => x.Value).ToList().ForEach(summary =>
            {
                logger.WriteLine();
                logger.WriteHeader($"// *** Collected benchmark summaries: the summary of the {summary.Title} benchmark");
                MarkdownExporter.Console.ExportToLog(summary, logger);
                ConclusionHelper.Print(logger, summary.BenchmarksCases.First()
                    .Config.GetCompositeAnalyser().Analyse(summary).ToList());
            });
        }
    }
}
