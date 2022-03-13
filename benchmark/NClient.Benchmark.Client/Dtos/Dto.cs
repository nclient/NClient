using System.Collections.Generic;

namespace NClient.Benchmark.Client.Dtos
{
    public class Dto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public InnerDto? InnerEntity { get; set; }
        public List<string>? Values { get; set; }
    }
}
