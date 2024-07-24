using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ProductToUserDTO
    {
        public Guid UserId { get; set; }

        public int ProductId { get; set; }
    
    }
}
