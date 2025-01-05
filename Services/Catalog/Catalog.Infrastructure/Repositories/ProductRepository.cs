using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
{
    private ICatalogContext _context { get; }
    
    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context
            .Products
            .Find(a => true)
            .ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context
            .Products
            .Find(a => a.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsByName(string name)
    {
        return await _context
            .Products
            .Find(a => a.Name.ToLower() == name.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsByBrand(string brandName)
    {
        return await _context
            .Products
            .Find(a => a.Brands.Name.ToLower() == brandName.ToLower())
            .ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updatedProduct = await _context
            .Products
            .ReplaceOneAsync(a => a.Id == product.Id, product);
        
        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var deletedProduct = await _context
            .Products
            .DeleteOneAsync(a => a.Id == id);
        
        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context
            .Brands
            .Find(a => true)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context
            .Types
            .Find(a => true)
            .ToListAsync();
    }
}