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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

// Add JWT Authentication (CRITICAL FIX for 405 errors)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false; // For development/production flexibility
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false, // We'll handle this in JWT middleware  
            ValidateAudience = false, // We'll handle this in JWT middleware
            ValidateLifetime = false, // We'll handle this in JWT middleware
            ValidateIssuerSigningKey = false, // We'll handle this in JWT middleware
            ClockSkew = TimeSpan.Zero
        };
        
        // Don't validate tokens here, let our custom JWT middleware handle it
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Prevent default 401 challenge
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                return Task.CompletedTask;
            }
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

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

// Database Configuration
var connectionString = "";

// Check for environment variables
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING");

Console.WriteLine("🔍 Checking environment variables...");
Console.WriteLine($"DATABASE_URL: {(!string.IsNullOrEmpty(databaseUrl) ? "Found" : "Not found")}");
Console.WriteLine($"SUPABASE_CONNECTION_STRING: {(!string.IsNullOrEmpty(supabaseUrl) ? "Found" : "Not found")}");

// Priority: Environment variables -> Configuration file
if (!string.IsNullOrEmpty(supabaseUrl))
{
    connectionString = supabaseUrl;
    Console.WriteLine("🟢 Using SUPABASE_CONNECTION_STRING from environment");
}
else if (!string.IsNullOrEmpty(databaseUrl))
{
    connectionString = databaseUrl;
    Console.WriteLine("🟡 Using DATABASE_URL from environment");
}
else
{
    // Use from configuration file
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
    Console.WriteLine("🟠 Using DefaultConnection from configuration file");
}

// Convert PostgreSQL URL format to .NET connection string format if needed
if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
{
    Console.WriteLine("🔄 Converting PostgreSQL URL to .NET format...");
    
    try
    {
        var uri = new Uri(connectionString);
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');
        
        var username = "";
        var password = "";
        
        if (!string.IsNullOrEmpty(uri.UserInfo))
        {
            var userInfo = uri.UserInfo.Split(':');
            username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : "";
            password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        }
        
        connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;Timeout=30;Command Timeout=30";
        Console.WriteLine($"✅ URL conversion completed - Username: {username}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ URL parsing failed: {ex.Message}");
        throw new InvalidOperationException($"Failed to parse database URL: {ex.Message}");
    }
}

// Validate connection string
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("❌ No database connection string found!");
}

Console.WriteLine($"✅ Database connection configured successfully");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    try
    {
        Console.WriteLine("🔧 Configuring Entity Framework DbContext...");
        
        // Configure Npgsql with connection string
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            npgsqlOptions.CommandTimeout(60); // Increase timeout to 60 seconds
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 1, // Reduce retry count
                maxRetryDelay: TimeSpan.FromSeconds(3),
                errorCodesToAdd: null);
        });
        
        // Enable sensitive data logging in development
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
        
        // Configure logging
        options.LogTo(Console.WriteLine, LogLevel.Information);
        
        Console.WriteLine("✅ DbContext configured successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database configuration error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
});

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

// Enable Swagger for debugging in production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechX API v1");
    c.RoutePrefix = "swagger";
});

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

// Use CORS first (before authentication)
app.UseCors("DefaultPolicy");

// Use Rate Limiting AFTER CORS but BEFORE Authentication
app.UseRateLimiter();

// Add missing UseAuthentication() - CRITICAL FIX for 405 errors
app.UseAuthentication();

// Use authorization AFTER authentication
app.UseAuthorization();

// Use JWT middleware AFTER standard authentication to handle custom JWT logic
app.UseMiddleware<JwtMiddleware>();

// Use custom exception handling middleware AFTER auth
app.UseMiddleware<ExceptionMiddleware>();

// Map health checks BEFORE controllers
app.MapHealthChecks("/health");

// Map controllers LAST
app.MapControllers();

// Database connection test and diagnostics
try
{
    Console.WriteLine("🧪 Testing database connection...");
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Test basic connection
    var canConnect = await context.Database.CanConnectAsync();
    if (canConnect)
    {
        Console.WriteLine("✅ Database connection test successful!");
        
        // Test a simple query to make sure tables exist
        try
        {
            var userCount = await context.Users.CountAsync();
            Console.WriteLine($"📊 Database contains {userCount} users");
            
            var categoryCount = await context.Categories.CountAsync();
            Console.WriteLine($"📊 Database contains {categoryCount} categories");
            
            Log.Information("Database connection and tables verified successfully");
        }
        catch (Exception queryEx)
        {
            Console.WriteLine($"⚠️ Database connected but table query failed: {queryEx.Message}");
            Log.Warning(queryEx, "Database connected but queries failed - tables may not exist");
        }
    }
    else
    {
        Console.WriteLine("❌ Database connection test failed - CanConnect returned false");
        Log.Error("Database connection test failed");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Database connection error: {ex.GetType().Name}: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"   Inner exception: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
    }
    Console.WriteLine($"   Stack trace: {ex.StackTrace}");
    
    Log.Error(ex, "Database connection error during startup: {ErrorMessage}", ex.Message);
    
    // Don't crash the application - continue without database for now
    Console.WriteLine("⚠️ Application will continue without database connectivity");
}

// Configure port for Railway deployment (only in production and when PORT env var is set)
if (app.Environment.IsProduction() && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT")))
{
    var port = Environment.GetEnvironmentVariable("PORT");
    app.Urls.Clear();
    app.Urls.Add($"http://*:{port}");
}

app.Run(); 