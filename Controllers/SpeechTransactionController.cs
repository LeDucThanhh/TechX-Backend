using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;
using TechX.API.Models;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeechTransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeechTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/speechtransaction
        [HttpPost]
        public async Task<ActionResult<SpeechTransaction>> CreateSpeechTransaction(SpeechTransaction speechTransaction)
        {
            try
            {
                speechTransaction.Status = "pending";
                speechTransaction.CreatedAt = DateTime.UtcNow;
                speechTransaction.UpdatedAt = DateTime.UtcNow;

                _context.SpeechTransactions.Add(speechTransaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSpeechTransaction), new { id = speechTransaction.Id }, speechTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/speechtransaction/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SpeechTransaction>> GetSpeechTransaction(int id)
        {
            try
            {
                var speechTransaction = await _context.SpeechTransactions.FindAsync(id);

                if (speechTransaction == null)
                {
                    return NotFound();
                }

                return Ok(speechTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/speechtransaction/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<SpeechTransaction>>> GetUserSpeechTransactions(int userId)
        {
            try
            {
                var speechTransactions = await _context.SpeechTransactions
                    .Where(st => st.UserId == userId)
                    .OrderByDescending(st => st.CreatedAt)
                    .ToListAsync();

                return Ok(speechTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/speechtransaction/{id}/process
        [HttpPut("{id}/process")]
        public async Task<IActionResult> ProcessSpeechTransaction(int id, [FromBody] ProcessSpeechRequest request)
        {
            try
            {
                var speechTransaction = await _context.SpeechTransactions.FindAsync(id);

                if (speechTransaction == null)
                {
                    return NotFound();
                }

                speechTransaction.ProcessedText = request.ProcessedText;
                speechTransaction.ExtractedData = request.ExtractedData;
                speechTransaction.Amount = request.Amount;
                speechTransaction.Category = request.Category;
                speechTransaction.Description = request.Description;
                speechTransaction.Type = request.Type;
                speechTransaction.TransactionDate = request.TransactionDate;
                speechTransaction.Status = "processed";
                speechTransaction.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Speech transaction processed successfully", speechTransaction });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/speechtransaction/{id}/fail
        [HttpPut("{id}/fail")]
        public async Task<IActionResult> FailSpeechTransaction(int id, [FromBody] string errorMessage)
        {
            try
            {
                var speechTransaction = await _context.SpeechTransactions.FindAsync(id);

                if (speechTransaction == null)
                {
                    return NotFound();
                }

                speechTransaction.Status = "failed";
                speechTransaction.ErrorMessage = errorMessage;
                speechTransaction.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Speech transaction marked as failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/speechtransaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeechTransaction(int id)
        {
            try
            {
                var speechTransaction = await _context.SpeechTransactions.FindAsync(id);

                if (speechTransaction == null)
                {
                    return NotFound();
                }

                _context.SpeechTransactions.Remove(speechTransaction);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Speech transaction deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ProcessSpeechRequest
    {
        public string? ProcessedText { get; set; }
        public string? ExtractedData { get; set; }
        public decimal? Amount { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
} 