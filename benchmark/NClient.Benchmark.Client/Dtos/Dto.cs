using System;
using System.Collections.Generic;

namespace NClient.Benchmark.Client.Dtos
{
    public class Dto
    {
        public long Id { get; set; }
        public int Int { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public string? String { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Uri? Uri { get; set; }
        public Guid Guid { get; set; }
        public Dto? InnerDto { get; set; }
        public object? Object { get; set; }
        public List<Dto>? List { get; set; }
        public Dictionary<string, Dto>? Dictionary { get; set; }
    }
}
