using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        var checkProducts = productCollection.Find(b => true).Any();
        //var path = Path.Combine("Data", "SeedData", "products.json");
        if (!checkProducts)
        {
            //var productsData = File.ReadAllText(path);
            var productsData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/products.json");
            var products  = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (var product in products)
                {
                    productCollection.InsertOne(product);
                }
            }
        }
    }
}