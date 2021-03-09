namespace NClient.Testing.Common.Entities
{
    public class NestedEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public BasicEntity InnerEntity { get; set; }
    }
}
