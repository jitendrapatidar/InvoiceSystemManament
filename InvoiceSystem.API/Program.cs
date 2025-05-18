using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using InvoiceSystem.Infrastructure;
using AutoMapper;
using InvoiceSystem.Application;

// Remove duplicate service registrations
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoice System API", Version = "v1" });
});

builder.WebHost.UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false);

builder.Services
    .AddInfrastructure(configuration)
    .AddCommandHandlers(); 

//builder.Services.AddAutoMapper(typeof(Program)); // Replace `Startup` with the appropriate assembly containing AutoMapper profiles.

var app = builder.Build();
app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Route: /scalar/v1
app.MapScalarApiReference(scalarOptions =>
{
    scalarOptions.DarkMode = true;
    scalarOptions.DotNetFlag = false;
    scalarOptions.HideDownloadButton = true;
    scalarOptions.HideModels = true;
    scalarOptions.Title = "Invoice System API";
    scalarOptions.ShowSidebar = false;

});

app.UseHttpsRedirection();

// Add Swagger middleware here
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice System API v1");
});


//// Open Scalar URL dynamically on app start
app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();

    if (addressFeature != null)
    {
        var baseUrl = addressFeature.Addresses.FirstOrDefault() ?? "https://localhost:5257";
        var scalarUrl = $"{baseUrl.TrimEnd('/')}/scalar/v1";

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = scalarUrl,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not open scalar: {ex.Message}");
        }
    }
});


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();