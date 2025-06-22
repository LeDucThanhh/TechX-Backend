using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;
using TechX.API.Models;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoucherController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
        {
            try
            {
                var vouchers = await _context.Vouchers
                    .Where(v => v.Status == "active")
                    .OrderByDescending(v => v.IsFeatured)
                    .ThenBy(v => v.ValidUntil)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = "Vouchers retrieved successfully",
                    data = vouchers,
                    count = vouchers.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving vouchers",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(int id)
        {
            try
            {
                var voucher = await _context.Vouchers
                    .Include(v => v.Store)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (voucher == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Voucher not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Voucher retrieved successfully",
                    data = voucher
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving voucher",
                    error = ex.Message
                });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserVoucher>>> GetUserVouchers(int userId)
        {
            try
            {
                var userVouchers = await _context.UserVouchers
                    .Include(uv => uv.Voucher)
                    .Include(uv => uv.User)
                    .Where(uv => uv.UserId == userId && uv.Status == "available")
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = "User vouchers retrieved successfully",
                    data = userVouchers,
                    count = userVouchers.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving user vouchers",
                    error = ex.Message
                });
            }
        }

        [HttpPost("collect/{voucherId}/user/{userId}")]
        public async Task<ActionResult> CollectVoucher(int voucherId, int userId)
        {
            try
            {
                // Check if voucher exists
                var voucher = await _context.Vouchers.FindAsync(voucherId);
                if (voucher == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Voucher not found"
                    });
                }

                // Check if user already has this voucher
                var existingUserVoucher = await _context.UserVouchers
                    .FirstOrDefaultAsync(uv => uv.UserId == userId && uv.VoucherId == voucherId);

                if (existingUserVoucher != null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "User already has this voucher"
                    });
                }

                // Add voucher to user's collection
                var userVoucher = new UserVoucher
                {
                    UserId = userId,
                    VoucherId = voucherId,
                    Status = "available",
                    ObtainedAt = DateTime.UtcNow
                };

                _context.UserVouchers.Add(userVoucher);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Voucher collected successfully",
                    data = userVoucher
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error collecting voucher",
                    error = ex.Message
                });
            }
        }
    }
} 