using System.Linq;
using NClient.Benchmark.Client.Dtos;

namespace NClient.Benchmark.Client.Helpers
{
    public class DtoProvider
    {
        public static Dto Get() => new()
        {
            Id = IdProvider.Get(),
            Name = "DtoName",
            InnerEntity = new InnerDto
            {
                Id = IdProvider.Get(),
                Name = "InnerDtoName"
            },
            Values = ListProvider.Get().ToList()
        };
    }
}
