using Microsoft.Extensions.Configuration;
namespace InvoiceSystem.Infrastructure.Extensions;

public static class Extensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionName)
        where TOptions : new()
    {
        var options = new TOptions();
        configuration.GetSection(sectionName).Bind(options); // 'Bind' is an extension method from Microsoft.Extensions.Configuration.Binder
        return options;
    }
}

    