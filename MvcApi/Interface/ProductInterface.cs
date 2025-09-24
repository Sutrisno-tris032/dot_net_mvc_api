using MvcApi.Models;

namespace MvcApi.Interface
{
    public interface ProductInterface
    {
        Task<List<ProductModel>> GetProductList();
        Task<ProductModel?> GetByIdAsync(int id);
        Task<ProductModel> AddProductAsync(ProductModel input);
        Task<ProductModel?> UpdateProductAsync(int id, ProductModel entity);
        Task<ProductModel?> DeleteProductAsync(int id);
    }
}
