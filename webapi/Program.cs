using Microsoft.Extensions.Azure;
using Serilog;
using stockmarrdk_api.Repository;
using System.Reflection;
using stockmarrdk_api.Common;
using stockmarrdk_api.Services;

StaticLogger.EnsureInitialized();
Log.Information("Azure Storage API Booting Up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Configuration.AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

    // Add blob service
    builder.Services.AddAzureClients(clientBuilder =>
    {
        string? connectionString = builder.Configuration.GetConnectionString("StockmarrStorage");
        if (connectionString == null)
        {
            throw new Exception("Could not connect to storage");
        }
        else
        {
            clientBuilder.AddBlobServiceClient(connectionString, preferMsi: true);
            clientBuilder.AddTableServiceClient(connectionString, preferMsi: true);
        }
    });

    // Add Services
    builder.Services.AddTransient<IImageRepository, ImageRepository>();
    builder.Services.AddTransient<IImageDataRepository, ImageDataRepository>();
    builder.Services.AddTransient<IImageService, ImageService>();
    builder.Services.AddTransient<ITripService, TripService>();
    builder.Services.AddTransient<ITripRepository, TripRepository>();
    Log.Information("Services has been successfully added...");

    var app = builder.Build();

    // Temporarily enable swagger for production
    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
    }
    else
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled Exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Azure Storage API Shutting Down...");
    Log.CloseAndFlush();
}