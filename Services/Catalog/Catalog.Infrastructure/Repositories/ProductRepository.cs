using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
{
    private ICatalogContext _context { get; }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        var sortDefinition = Builders<Product>.Sort.Ascending("Name");
        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            sortDefinition = catalogSpecParams.Sort switch
            {
                "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            };
        }
        
        return await _context.Products
            .Find(filter)
            .Sort(sortDefinition)
            .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();
    }
    
    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    
    public async Task<Pagination<Product>> GetAllProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            filter &= builder.Where(x => x.Name.ToLower().Contains(catalogSpecParams.Search));
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            filter &= builder.Eq(p => p.Brands.Id, catalogSpecParams.BrandId);
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            filter &= builder.Eq(p => p.Types.Id, catalogSpecParams.TypeId);
        }
        var totalItems = await _context.Products.CountDocumentsAsync(filter);
        var data = await DataFilter(catalogSpecParams, filter);
        
        return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, totalItems, data);
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