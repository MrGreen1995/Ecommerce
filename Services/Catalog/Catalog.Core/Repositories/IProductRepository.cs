using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    
    Task<Product> GetProduct(string id);

    Task<IEnumerable<Product>> GetAllProductsByName(string name);
    
    Task<IEnumerable<Product>> GetAllProductsByBrand(string brandName);
    
    Task<Product> CreateProduct(Product product);
    
    Task<bool> UpdateProduct(Product product);
    
    Task<bool> DeleteProduct(string id);
}