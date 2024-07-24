using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid userId,int id);
        Task<List<ProductEntity>> GetAll(Guid userId);
        Task<ProductEntity> GetByIdAsync(Guid userId,int id);
        Task<bool> UpdateAsync(ProductEntity product);

        Task SaveChangesAsync();
    }
}
