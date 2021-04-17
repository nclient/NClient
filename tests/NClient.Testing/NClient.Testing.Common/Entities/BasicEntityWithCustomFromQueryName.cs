using Microsoft.AspNetCore.Mvc;

namespace NClient.Testing.Common.Entities
{
    public class BasicEntityWithCustomFromQueryName
    {
        [FromQuery(Name = "MyId")]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}