using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserProductRepository
    {
        Task<bool> CreateAsync(Guid userId, int productId);
        Task<bool> DeleteAsync(Guid userId, int productId);

        Task<List<ProductEntity>> GetProductsByUser(Guid userId);

    }
}
