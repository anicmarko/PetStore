using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class PopularProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int AssignmentCount { get; set; }
        public string CreatedByUserName { get; set; }
    }
}
