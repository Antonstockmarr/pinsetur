using Microsoft.Extensions.Azure;
using Serilog;
using stockmarrdk_api.Repository;
using System.Reflection;
using stockmarrdk_api.Common;
using stockmarrdk_api.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using stockmarrdk_api;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.ApplicationInsights.Extensibility;

StaticLogger.EnsureInitialized();
Log.Information("Azure Storage API Booting Up...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    // Configure Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        string? jwtKey = builder.Configuration.GetValue<string>("JwtKey") ?? throw new Exception("jwtKey could not be read");
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

    // Configure Authorization
    builder.Services.AddAuthorization(options =>
    {
        var adminPolicy = new AuthorizationPolicyBuilder()
            .RequireClaim(ClaimTypes.Role, UserRole.Admin.ToString())
            .Build();

        options.FallbackPolicy = adminPolicy;
        options.AddPolicy("Admins", adminPolicy);

        options.AddPolicy("Users", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });

    builder.Services.AddControllers()
        // Map enums to names in Swagger
        .AddJsonOptions(options => {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // Setup Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    // Get secrets
    builder.Configuration.AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

    // Add Serilog
    builder.Host.UseSerilog((_, config) =>
    {
        if (builder.Configuration.GetConnectionString("AppInsights") is not null)
        {
            config.WriteTo.ApplicationInsights(new TelemetryConfiguration { ConnectionString = builder.Configuration.GetConnectionString("AppInsights") }, TelemetryConverter.Events);
        }
        else
        {
            config.WriteTo.Console();
        }
        config.ReadFrom.Configuration(builder.Configuration);
    });

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
    builder.Services.AddTransient<ITokenService, TokenService>();
    builder.Services.AddTransient<ITokenRepository, TokenRepository>();
    builder.Services.AddTransient<ILoginService, LoginService>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    Log.Information("Services has been successfully added...");

    var app = builder.Build();

    // Enable swagger for production
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

    app.UseAuthentication();
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