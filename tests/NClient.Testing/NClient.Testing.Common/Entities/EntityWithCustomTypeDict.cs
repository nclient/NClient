using System.Collections.Generic;

namespace NClient.Testing.Common.Entities
{
    public class EntityWithCustomTypeDict
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public Dictionary<int, BasicEntity>? Dict { get; set; }
    }
}
