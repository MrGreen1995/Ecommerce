using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    
    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ($"SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
        
        return coupon ?? new Coupon { ProductName = "No Coupon Found", Amount = 0, Description = "No discount available"};
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        
        var affected = await connection.ExecuteAsync
            ($"INSERT INTO Coupon (ProductName, Description, Amount) values (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount});
        
        return affected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        
        var affected = await connection.ExecuteAsync
        ($"UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id});
        
        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        
        var affected = await connection.ExecuteAsync
        ($"DELETE FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });
        
        return affected != 0;
    }
}