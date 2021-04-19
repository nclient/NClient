using NClient.Annotations.Parameters;

namespace NClient.Testing.Common.Entities
{
    public class BasicEntityWithCustomQueryName
    {
        [QueryParam(Name = "MyId")]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}