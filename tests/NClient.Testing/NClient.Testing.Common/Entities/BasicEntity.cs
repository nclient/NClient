using System.ComponentModel.DataAnnotations;

namespace NClient.Testing.Common.Entities
{
    public class BasicEntity
    {
        [Required]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
