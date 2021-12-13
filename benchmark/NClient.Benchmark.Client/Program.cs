using System;
using System.Linq;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using NClient.Benchmark.Client.JsonApi;
using NClient.Benchmark.Client.JsonHttpResponseApi;
using NClient.Benchmark.Client.PrimitiveApi;
using NClient.Benchmark.Client.PrimitiveHttpResponseApi;

namespace NClient.Benchmark.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var summaries = new[]
            {
                new Lazy<Summary>(() => BenchmarkRunner.Run<PrimitiveApiClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<PrimitiveHttpResponseApiClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<JsonApiClientBenchmark>()),
                new Lazy<Summary>(() => BenchmarkRunner.Run<JsonHttpResponseApiClientBenchmark>())
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
