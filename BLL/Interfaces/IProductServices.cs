using BLL.DTOs;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IProductServices
    {
        Task<ProductEntity> CreateProduct(CreateUpdateProductDTO dto);
        Task<List<ProductEntity>> GetProducts();
        Task<ProductEntity> GetProductById(Guid id);
        Task<bool> UpdateProduct(Guid id, CreateUpdateProductDTO dto);

        Task<bool> DeleteProduct(Guid id);
    }
}
