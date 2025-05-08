using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    private static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new Order
            {
                UserName = "john",
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john@mai.com",
                AddressLine = "Dark city",
                Country = "Dark country",
                TotalPrice = 750,
                State = "DA",
                ZipCode = "666666",
                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "john",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "john",
                LastModifiedDate = DateTime.Now,
                Cvv = "123",
                Expiration = "12/25",
                PaymentMethod = 1
            }
        };
    }
    
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();
            logger?.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded");
        }
    }
}