using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class ProductEntity
    {
        //TODO Fluent validation
        [Key]
        public Guid Id { get; set; }

        public  string Brand { get; set; }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
