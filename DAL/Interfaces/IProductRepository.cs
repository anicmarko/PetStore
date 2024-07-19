using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid id);
        IQueryable<ProductEntity> GetAll();
        Task<ProductEntity> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(ProductEntity product);

        Task SaveChangesAsync();
    }
}
