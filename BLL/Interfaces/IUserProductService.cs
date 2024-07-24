using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserProductService
    {
        Task<AddRelationshipResponse> AddProductToUserAsync(ProductToUserDTO dto);
        Task<bool> RemoveProductFromUserAsync(Guid userId, int productId);
        Task<List<ProductToUserDTO>> GetProductsByUserAsync(Guid userId);


    }
}
