using System.Collections.Generic;

namespace NClient.Testing.Common.Entities
{
    public class EntityWithDict
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public Dictionary<int, string>? Dict { get; set; }
    }
}
