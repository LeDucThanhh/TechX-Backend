using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                message = "API is running"
            });
        }

        [HttpGet("database")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return StatusCode(500, new { 
                        success = false,
                        message = "Cannot connect to database",
                        timestamp = DateTime.UtcNow
                    });
                }

                var userCount = await _context.Users.CountAsync();
                var categoryCount = await _context.Categories.CountAsync();

                return Ok(new {
                    success = true,
                    message = "Database connection successful",
                    data = new {
                        connected = true,
                        userCount = userCount,
                        categoryCount = categoryCount,
                        timestamp = DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    message = "Database connection failed",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("connection-string")]
        public IActionResult TestConnectionString()
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                
                // Hide password for security
                var safeConnectionString = connectionString;
                if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Password="))
                {
                    var parts = connectionString.Split(';');
                    safeConnectionString = string.Join(";", parts.Where(p => !p.StartsWith("Password=")));
                }

                return Ok(new {
                    success = true,
                    connectionString = safeConnectionString,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    message = "Failed to get connection string",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
} 