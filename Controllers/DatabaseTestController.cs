using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DatabaseTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("health")]
        public async Task<ActionResult> DatabaseHealth()
        {
            try
            {
                Console.WriteLine("🔍 Database health check requested...");
                
                // Get connection string information (without password)
                var connectionString = _context.Database.GetConnectionString();
                var connectionInfo = "Not available";
                
                if (!string.IsNullOrEmpty(connectionString))
                {
                    try
                    {
                        var parts = connectionString.Split(';');
                        var safeParts = parts.Where(p => !p.StartsWith("Password=", StringComparison.OrdinalIgnoreCase))
                                             .Where(p => !p.StartsWith("Pwd=", StringComparison.OrdinalIgnoreCase));
                        connectionInfo = string.Join(";", safeParts);
                    }
                    catch
                    {
                        connectionInfo = "Connection string parsing failed";
                    }
                }
                
                Console.WriteLine($"Connection info: {connectionInfo}");
                
                // Test connection
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    Console.WriteLine("❌ Cannot connect to database");
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Cannot connect to database",
                        connectionInfo = connectionInfo,
                        timestamp = DateTime.UtcNow,
                        environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
                    });
                }
                
                Console.WriteLine("✅ Database connection successful");
                
                // Test table access
                var diagnostics = new Dictionary<string, object>();
                
                try
                {
                    var userCount = await _context.Users.CountAsync();
                    diagnostics["userCount"] = userCount;
                    Console.WriteLine($"Users table: {userCount} records");
                }
                catch (Exception ex)
                {
                    diagnostics["userCount"] = $"Error: {ex.Message}";
                    Console.WriteLine($"Users table error: {ex.Message}");
                }
                
                try
                {
                    var categoryCount = await _context.Categories.CountAsync();
                    diagnostics["categoryCount"] = categoryCount;
                    Console.WriteLine($"Categories table: {categoryCount} records");
                }
                catch (Exception ex)
                {
                    diagnostics["categoryCount"] = $"Error: {ex.Message}";
                    Console.WriteLine($"Categories table error: {ex.Message}");
                }
                
                try
                {
                    var storeCount = await _context.Stores.CountAsync();
                    diagnostics["storeCount"] = storeCount;
                    Console.WriteLine($"Stores table: {storeCount} records");
                }
                catch (Exception ex)
                {
                    diagnostics["storeCount"] = $"Error: {ex.Message}";
                    Console.WriteLine($"Stores table error: {ex.Message}");
                }

                return Ok(new
                {
                    success = true,
                    message = "Database connection and table access successful",
                    connectionInfo = connectionInfo,
                    diagnostics = diagnostics,
                    timestamp = DateTime.UtcNow,
                    database = "Supabase PostgreSQL",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database health check failed: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                return StatusCode(500, new
                {
                    success = false,
                    message = "Database health check failed",
                    error = ex.Message,
                    errorType = ex.GetType().Name,
                    innerError = ex.InnerException?.Message,
                    timestamp = DateTime.UtcNow,
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
                });
            }
        }

        [HttpGet("tables")]
        public async Task<ActionResult> GetTablesInfo()
        {
            try
            {
                var tableInfo = new
                {
                    users = await _context.Users.CountAsync(),
                    categories = await _context.Categories.CountAsync(),
                    stores = await _context.Stores.CountAsync(),
                    transactions = await _context.Transactions.CountAsync(),
                    budgets = await _context.Budgets.CountAsync(),
                    items = await _context.Items.CountAsync(),
                    receipts = await _context.Receipts.CountAsync(),
                    receipt_items = await _context.ReceiptItems.CountAsync(),
                    loyalty_points = await _context.LoyaltyPoints.CountAsync(),
                    cashback_transactions = await _context.CashbackTransactions.CountAsync(),
                    reviews = await _context.Reviews.CountAsync(),
                    notifications = await _context.Notifications.CountAsync(),
                    settings = await _context.Settings.CountAsync(),
                    refresh_tokens = await _context.RefreshTokens.CountAsync(),
                    vouchers = await _context.Vouchers.CountAsync(),
                    user_vouchers = await _context.UserVouchers.CountAsync(),
                    voucher_usages = await _context.VoucherUsages.CountAsync()
                };

                return Ok(new
                {
                    success = true,
                    message = "Table information retrieved successfully",
                    data = tableInfo,
                    total_tables = 17,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving table information",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("sample-data")]
        public async Task<ActionResult> GetSampleData()
        {
            try
            {
                var sampleData = new
                {
                    sample_users = await _context.Users
                        .Select(u => new { u.Id, u.Email, u.FirstName, u.LastName, u.AuthProvider })
                        .Take(5)
                        .ToListAsync(),
                    
                    sample_categories = await _context.Categories
                        .Select(c => new { c.Id, c.Name, c.Type, c.Icon })
                        .Take(5)
                        .ToListAsync(),
                    
                    sample_stores = await _context.Stores
                        .Select(s => new { s.Id, s.Name, s.CashbackRate, s.IsPartner })
                        .Take(5)
                        .ToListAsync(),
                    
                    sample_vouchers = await _context.Vouchers
                        .Select(v => new { v.Id, v.Code, v.Title, v.Type, v.Value, v.Status })
                        .Take(5)
                        .ToListAsync(),
                    
                    sample_transactions = await _context.Transactions
                        .Select(t => new { t.Id, t.Amount, t.Type, t.Description, t.Date })
                        .Take(5)
                        .ToListAsync()
                };

                return Ok(new
                {
                    success = true,
                    message = "Sample data retrieved successfully",
                    data = sampleData,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving sample data",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        // Simple connectivity test without database dependency
        [HttpGet("ping")]
        public ActionResult Ping()
        {
            return Ok(new
            {
                success = true,
                message = "API is running and responding correctly",
                timestamp = DateTime.UtcNow,
                server = "TechX Backend API",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                methods_supported = new[] { "GET", "POST", "PUT", "DELETE" },
                version = "1.0.0"
            });
        }
    }
} 