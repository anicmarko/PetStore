using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid userId,int id);
        Task<List<ProductEntity>> GetAll(Guid userId);
        Task<ProductEntity> GetByIdAsync(Guid userId,int id);
        Task<bool> UpdateAsync(ProductEntity product);

        Task<int> GetTotalProducts();
        Task<decimal> GetAveragePrice();
        Task<decimal> GetMinPrice();
        Task<decimal> GetMaxPrice();
        Task<int> GetTotalAssignments();
        
        Task<List<ProductEntity>> GetPopularProducts(int topN);
        Task SaveChangesAsync();
    }
}
