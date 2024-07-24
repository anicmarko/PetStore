using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public required string Brand { get; set; }

        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<UserEntity> Users { get; } = [];

        public Guid OwnerId { get; set; }

        public UserEntity Owner { get; set; }
    }
}
