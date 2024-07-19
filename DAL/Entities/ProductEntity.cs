using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class ProductEntity
    {
        //TODO Fluent validation
        [Key]
        public Guid Id { get; set; }

        public required string Brand { get; set; }

        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
