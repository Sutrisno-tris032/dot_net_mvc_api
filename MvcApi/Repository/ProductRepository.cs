using Microsoft.EntityFrameworkCore;
using MvcApi.Interface;
using MvcApi.Models;

namespace MvcApi.Repository
{
    public class ProductRepository : ProductInterface
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db) => _db = db;

        public async Task<ProductModel> AddProductAsync(ProductModel input)
        {
            _db.Products.Add(input);
            await _db.SaveChangesAsync();
            return input;
        }

        public async Task<ProductModel?> DeleteProductAsync(int id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity is null) return null;

            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<ProductModel?> GetByIdAsync(int id)
            => await _db.Products.FindAsync(id);

        public async Task<List<ProductModel>> GetProductList()
            => await _db.Products.AsNoTracking().ToListAsync();

        public async Task<ProductModel?> UpdateProductAsync(int id, ProductModel entity)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing is null) return null;

            _db.Entry(existing).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
