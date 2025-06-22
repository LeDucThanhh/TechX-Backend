using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;
using TechX.API.Models;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReceiptController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/receipt/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetUserReceipts(int userId)
        {
            try
            {
                var receipts = await _context.Receipts
                    .Where(r => r.UserId == userId)
                    .Include(r => r.ReceiptItems)
                    .OrderByDescending(r => r.PurchaseDate)
                    .ToListAsync();

                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/receipt/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int id)
        {
            try
            {
                var receipt = await _context.Receipts
                    .Include(r => r.ReceiptItems)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (receipt == null)
                {
                    return NotFound();
                }

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/receipt
        [HttpPost]
        public async Task<ActionResult<Receipt>> UploadReceipt(Receipt receipt)
        {
            try
            {
                // Set default values
                receipt.Status = "pending";
                receipt.CreatedAt = DateTime.UtcNow;
                receipt.UpdatedAt = DateTime.UtcNow;

                _context.Receipts.Add(receipt);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReceipt), new { id = receipt.Id }, receipt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/receipt/{id}/process
        [HttpPut("{id}/process")]
        public async Task<IActionResult> ProcessReceipt(int id, [FromBody] ProcessReceiptRequest request)
        {
            try
            {
                var receipt = await _context.Receipts.FindAsync(id);

                if (receipt == null)
                {
                    return NotFound();
                }

                // Update receipt with OCR results
                receipt.OcrText = request.OcrText;
                receipt.Items = request.Items;
                receipt.Status = "processed";
                receipt.ProcessedAt = DateTime.UtcNow;
                receipt.UpdatedAt = DateTime.UtcNow;

                // Calculate cashback and points if applicable
                if (receipt.StoreId.HasValue)
                {
                    var store = await _context.Stores.FindAsync(receipt.StoreId.Value);
                    if (store != null)
                    {
                        receipt.CashbackAmount = receipt.TotalAmount * (store.CashbackRate / 100);
                        receipt.PointsEarned = Convert.ToInt32(receipt.TotalAmount * store.PointsRate);
                    }
                }

                // Add receipt items if provided
                if (request.ReceiptItems != null && request.ReceiptItems.Any())
                {
                    foreach (var item in request.ReceiptItems)
                    {
                        item.ReceiptId = receipt.Id;
                        _context.ReceiptItems.Add(item);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Receipt processed successfully", receipt });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/receipt/{id}/fail
        [HttpPut("{id}/fail")]
        public async Task<IActionResult> FailReceipt(int id, [FromBody] string errorMessage)
        {
            try
            {
                var receipt = await _context.Receipts.FindAsync(id);

                if (receipt == null)
                {
                    return NotFound();
                }

                receipt.Status = "failed";
                receipt.Notes = errorMessage;
                receipt.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Receipt marked as failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/receipt/user/{userId}/pending
        [HttpGet("user/{userId}/pending")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetPendingReceipts(int userId)
        {
            try
            {
                var receipts = await _context.Receipts
                    .Where(r => r.UserId == userId && r.Status == "pending")
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/receipt/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            try
            {
                var receipt = await _context.Receipts
                    .Include(r => r.ReceiptItems)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (receipt == null)
                {
                    return NotFound();
                }

                _context.Receipts.Remove(receipt);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Receipt deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ProcessReceiptRequest
    {
        public string? OcrText { get; set; }
        public string? Items { get; set; }
        public List<ReceiptItem>? ReceiptItems { get; set; }
    }
} 