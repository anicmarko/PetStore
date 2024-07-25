using BLL.DTOs;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IProductServices
    {
        Task<ProductEntity> CreateProduct(CreateUpdateProductDTO dto);
        Task<List<ProductEntity>> GetProducts();
        Task<ProductEntity> GetProductById(int id);
        Task<bool> UpdateProduct(int id, CreateUpdateProductDTO dto);

        Task<bool> DeleteProduct(int id);
        Task<ProductStatisticsDTO> GetProductStatistics();

        Task<List<PopularProductDTO>> GetPopularProducts(int topN);

    }
}
