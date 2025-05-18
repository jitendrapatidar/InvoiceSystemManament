using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Infrastructure.Context;
using InvoiceSystem.Infrastructure.Extensions;
using InvoiceSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceSystem.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Ensure the GetOptions method is implemented as an extension method for IConfiguration
        var options = configuration.GetOptions<DataBaseOptions>("DataBaseConnectionString");
       
        services.AddDbContext<ReadInvoiceSystemDbContext>(ctx =>
            ctx.UseSqlServer(options.ConnectionString));

        services.AddDbContext<WriteInvoiceSystemDbContext>(ctx =>
            ctx.UseSqlServer(options.ConnectionString));

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
