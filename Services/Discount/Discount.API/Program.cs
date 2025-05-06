using System.Reflection;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Aumapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register MediatR
var assemblies = new Assembly[]
{
    Assembly.GetEntryAssembly(),
    typeof(CreateDiscountCommandHandler).Assembly,
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

// Migrate Database
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

//app.UseAuthorization();

//app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
    });
});

app.Run();