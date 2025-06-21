using Microsoft.EntityFrameworkCore;
using TechX.API.Data;
using TechX.API.Services.Interfaces;
using TechX.API.Services.Implementations;
using TechX.API.Middleware;
using TechX.API.Helpers;
using TechX.API.Mappings;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configure DateTime handling for PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Add global model validation filter
    options.Filters.Add<TechX.API.Helpers.ModelValidationAttribute>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechX API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register JWT Helper
builder.Services.AddScoped<JwtHelper>();

// Register Password Helper
builder.Services.AddScoped<PasswordHelper>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Add Health Checks (simplified)
builder.Services.AddHealthChecks();

// Add DbContext - Force use appsettings (bypass environment variables)
var connectionString = builder.Environment.IsProduction() 
    ? "postgresql://postgres:QvTwobWAnPApraKSyHULicWOsfbokigo@postgres.railway.internal:5432/railway"
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, 
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

// Add CORS policy for mobile apps
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy",
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }
            else
            {
                // Allow mobile apps and web clients
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                      
                // Note: For production with credentials, specify exact origins
                // policy.WithOrigins("https://yourapp.com", "https://localhost")
                //       .AllowAnyMethod()
                //       .AllowAnyHeader()
                //       .AllowCredentials();
            }
        });
});

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ICashbackService, CashbackService>();
builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

// Add HttpClient for Google OAuth
builder.Services.AddHttpClient();

// Configure logging
builder.Logging.ClearProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Disable Swagger in production for security and focus on API functionality
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechX API v1");
        c.RoutePrefix = "swagger";
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    
    // Add security headers for production
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        await next();
    });
}

app.UseHttpsRedirection();

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Use Rate Limiting
app.UseRateLimiter();

// Use CORS with more restrictive policy
app.UseCors("DefaultPolicy");

// Use custom exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// Use JWT authentication middleware
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

// Map health checks
app.MapHealthChecks("/health");

app.MapControllers();

// Apply database migrations (DISABLED because database is managed manually)
/*
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while applying database migrations.");
}
*/

// Configure port for Railway deployment (only in production and when PORT env var is set)
if (app.Environment.IsProduction() && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT")))
{
    var port = Environment.GetEnvironmentVariable("PORT");
    app.Urls.Clear();
    app.Urls.Add($"http://*:{port}");
}

app.Run(); 