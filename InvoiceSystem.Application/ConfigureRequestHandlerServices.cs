using AutoMapper;
using InvoiceSystem.Application.Common;
using InvoiceSystem.Application.DTO;
using InvoiceSystem.Application.Features.Invoices.Commands;
using InvoiceSystem.Application.Features.Invoices.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace InvoiceSystem.Application;

[ExcludeFromCodeCoverage]
public static class ConfigureRequestHandlerServices
{
    /// <summary>
    /// Adds command handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(IApplicationMarker));
        services
             .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
             .AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(assembly))));
             //.AddValidatorsFromAssembly(assembly);
        // Explicit handler registration
        services.AddTransient<IRequestHandler<CreateInvoiceCommand, int>, CreateInvoiceCommandHandler>();
        services.AddTransient<IRequestHandler<UpdateInvoiceCommand, Unit>, UpdateInvoiceCommandHandler>();
        services.AddTransient<IRequestHandler<DeleteInvoiceCommand, Unit>, DeleteInvoiceCommandHandler>();
        services.AddTransient<IRequestHandler<MarkInvoiceAsPaidCommand, Unit>, MarkInvoiceAsPaidCommandHandler>();
        services.AddTransient<IRequestHandler<GetInvoiceQuery, InvoiceDto>, GetInvoiceQueryHandler>();
        services.AddTransient<IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>, GetAllInvoicesQueryHandler>();
         

        return services;

       


    }
}

 