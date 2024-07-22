using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid id);
        Task<List<ProductEntity>> GetAll();
        Task<ProductEntity> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(ProductEntity product);

        Task SaveChangesAsync();
    }
}
