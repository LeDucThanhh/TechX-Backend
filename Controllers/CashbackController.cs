using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashbackController : ControllerBase
    {
        private readonly ICashbackService _cashbackService;

        public CashbackController(ICashbackService cashbackService)
        {
            _cashbackService = cashbackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashbackTransactionDTO>>> GetAllCashbackTransactions()
        {
            try
            {
                var cashbackTransactions = await _cashbackService.GetUserCashbackTransactionsAsync(0); // This should be filtered by user
                return Ok(cashbackTransactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving cashback transactions" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CashbackTransactionDTO>> GetCashbackTransactionById(int id)
        {
            try
            {
                var cashbackTransaction = await _cashbackService.GetCashbackTransactionByIdAsync(id);
                return Ok(cashbackTransaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving cashback transaction" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CashbackTransactionDTO>>> GetUserCashbackTransactions(int userId)
        {
            try
            {
                var cashbackTransactions = await _cashbackService.GetUserCashbackTransactionsAsync(userId);
                return Ok(cashbackTransactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user cashback transactions" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CashbackTransactionDTO>> CreateCashbackTransaction([FromBody] CreateCashbackTransactionDTO createDto)
        {
            try
            {
                var cashbackTransaction = await _cashbackService.CreateCashbackTransactionAsync(createDto);
                return CreatedAtAction(nameof(GetCashbackTransactionById), new { id = cashbackTransaction.Id }, cashbackTransaction);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating cashback transaction" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CashbackTransactionDTO>> UpdateCashbackTransaction(int id, [FromBody] UpdateCashbackTransactionDTO updateDto)
        {
            try
            {
                var cashbackTransaction = await _cashbackService.UpdateCashbackTransactionAsync(id, updateDto);
                return Ok(cashbackTransaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating cashback transaction" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCashbackTransaction(int id)
        {
            try
            {
                var result = await _cashbackService.DeleteCashbackTransactionAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Cashback transaction not found" });
                }
                return Ok(new { message = "Cashback transaction deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting cashback transaction" });
            }
        }

        [HttpGet("user/{userId}/total")]
        public async Task<ActionResult<decimal>> GetUserTotalCashback(int userId)
        {
            try
            {
                var totalCashback = await _cashbackService.GetUserTotalCashbackAsync(userId);
                return Ok(new { totalCashback });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total cashback" });
            }
        }
    }
} 