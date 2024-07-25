using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public required string Brand { get; set; }

        public required string Title { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<UserEntity> Users { get; } = [];

        public Guid OwnerId { get; set; }

        public UserEntity Owner { get; set; }
    }
}
