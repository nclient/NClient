using System;
using System.Collections.Generic;
using NClient.Benchmark.Client.Dtos;

namespace NClient.Benchmark.Client.Helpers
{
    public class DtoProvider
    {
        private static long _id;
        private static Dto Dto { get; } = Get(id: 0, innerDto: Get(id: 1, innerDto: null));

        public static Dto Get()
        {
            Dto.Id = _id++;
            return Dto;
        }        
        
        private static Dto Get(long id, Dto? innerDto)
        {
            return new Dto
            {
                Id = id,
                Int = int.MinValue,
                Double = double.MaxValue,
                Decimal = 0,
                String = "DtoName",
                DateTime = DateTime.Today,
                TimeSpan = TimeSpan.MaxValue,
                Uri = new Uri("http://localhost:5000"),
                Guid = Guid.Empty,
                InnerDto = innerDto,
                List = innerDto is null ? null : new List<Dto> { innerDto },
                Dictionary = innerDto is null ? null : new Dictionary<string, Dto> { ["key1"] = innerDto }
            };
        }
    }
}
