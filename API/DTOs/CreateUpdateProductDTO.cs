using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateUpdateProductDTO
    { 
        public required string Brand { get; set; }

        public required string Title { get; set; }
    }
}
