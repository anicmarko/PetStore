using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class CreateUpdateProductDTO
    { 
        public required string Brand { get; set; }

        public required string Title { get; set; }

        public decimal Price { get; set; }
    }
}
