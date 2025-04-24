using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation($"Discount DB Migration started");
                
            ApplyMigrations(config);
                
            logger.LogInformation($"Discount DB Migration completed");
        }
        catch (Exception e)
        {
            logger.LogError($"Discount DB Migration failed: {e.Message}");
            Console.WriteLine(e.Message);
            throw;
        }

        return host;
    }

    private static void ApplyMigrations(IConfiguration config)
    {
        var retry = 5;
        while (retry > 0)
        {
            try
            {
                using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
        
                connection.Open();

                using var cmd = new NpgsqlCommand();
                cmd.Connection = connection;
        
                cmd.CommandText = @"DROP TABLE IF EXISTS Coupon";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(500) NOT NULL, Description TEXT, Amount INT)";
                cmd.ExecuteNonQuery();
        
                cmd.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500)";
                cmd.ExecuteNonQuery();
        
                cmd.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Shoe Discount', 500)";
                cmd.ExecuteNonQuery();
        
                connection.Close();
                
                break;
            }
            catch (Exception e)
            {
                retry--;
                if (retry == 0)
                {
                    throw;
                }
                
                Thread.Sleep(2000);
            }
        }
    }
}