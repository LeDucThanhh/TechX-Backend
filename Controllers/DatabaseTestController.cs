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
                // Test connection by running a simple query
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Cannot connect to database",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Database connection successful",
                    timestamp = DateTime.UtcNow,
                    database = "Supabase PostgreSQL"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Database health check failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
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
    }
} 