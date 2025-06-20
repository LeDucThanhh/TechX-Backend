using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET /api/transaction - Get current user's transactions
        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> GetCurrentUserTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                // Get user ID from JWT token
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var transactions = await _transactionService.GetUserTransactionsAsync(userId, page, pageSize);
                return Ok(transactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transactions" });
            }
        }

        // GET /api/transaction/summary - Get current user's transaction summary
        [HttpGet("summary")]
        public async Task<ActionResult<TransactionSummaryDTO>> GetCurrentUserTransactionSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                // Get user ID from JWT token
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                // Default to current month if no dates provided
                var start = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var end = endDate ?? start.AddMonths(1).AddDays(-1);

                var summary = await _transactionService.GetTransactionSummaryAsync(userId, start, end);
                return Ok(summary);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating transaction summary" });
            }
        }

        // GET /api/transaction/{id} - Get specific transaction by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound(new { message = "Transaction not found" });
                }

                // Check if transaction belongs to current user
                var userId = GetCurrentUserId();
                if (userId != 0 && transaction.UserId != userId)
                {
                    return Forbid("You can only access your own transactions");
                }

                return Ok(transaction);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transaction" });
            }
        }

        // POST /api/transaction - Create new transaction
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction([FromBody] CreateTransactionDTO createDto)
        {
            try
            {
                // Get user ID from JWT token
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                // Override userId from token
                createDto.UserId = userId;

                var transaction = await _transactionService.CreateTransactionAsync(createDto);
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating transaction", details = ex.Message });
            }
        }

        // PUT /api/transaction/{id} - Update transaction
        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionDTO>> UpdateTransaction(int id, [FromBody] UpdateTransactionDTO updateDto)
        {
            try
            {
                // Check if transaction belongs to current user
                var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
                if (existingTransaction == null)
                {
                    return NotFound(new { message = "Transaction not found" });
                }

                var userId = GetCurrentUserId();
                if (userId != 0 && existingTransaction.UserId != userId)
                {
                    return Forbid("You can only update your own transactions");
                }

                var transaction = await _transactionService.UpdateTransactionAsync(id, updateDto);
                return Ok(transaction);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating transaction" });
            }
        }

        // DELETE /api/transaction/{id} - Delete transaction
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                // Check if transaction belongs to current user
                var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
                if (existingTransaction == null)
                {
                    return NotFound(new { message = "Transaction not found" });
                }

                var userId = GetCurrentUserId();
                if (userId != 0 && existingTransaction.UserId != userId)
                {
                    return Forbid("You can only delete your own transactions");
                }

                var result = await _transactionService.DeleteTransactionAsync(id);
                return Ok(new { message = "Transaction deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting transaction" });
            }
        }

        // GET /api/transaction/date-range - Get transactions by date range
        [HttpGet("date-range")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var transactions = await _transactionService.GetTransactionsByDateRangeAsync(userId, startDate, endDate);
                return Ok(transactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transactions by date range" });
            }
        }

        // GET /api/transaction/category/{categoryId} - Get transactions by category
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByCategory(int categoryId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var transactions = await _transactionService.GetTransactionsByCategoryAsync(userId, categoryId);
                return Ok(transactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transactions by category" });
            }
        }

        // GET /api/transaction/total-spent - Get total spent
        [HttpGet("total-spent")]
        public async Task<ActionResult<decimal>> GetUserTotalSpent()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var totalSpent = await _transactionService.GetUserTotalSpentAsync(userId);
                return Ok(new { totalSpent });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total spent" });
            }
        }

        // GET /api/transaction/total-income - Get total income
        [HttpGet("total-income")]
        public async Task<ActionResult<decimal>> GetUserTotalIncome()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var totalIncome = await _transactionService.GetUserTotalIncomeAsync(userId);
                return Ok(new { totalIncome });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total income" });
            }
        }

        // Helper method to get current user ID from JWT token
        private int GetCurrentUserId()
        {
            try
            {
                var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }
                
                // Fallback: try to get from HttpContext.Items (set by JwtMiddleware)
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj != null)
                {
                    if (int.TryParse(userIdObj.ToString(), out var contextUserId))
                    {
                        return contextUserId;
                    }
                }
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
} 
                return StatusCode(500, new { message = "An error occurred while deleting transaction" });
            }
        }

        // GET /api/transaction/date-range - Get transactions by date range
        [HttpGet("date-range")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var transactions = await _transactionService.GetTransactionsByDateRangeAsync(userId, startDate, endDate);
                return Ok(transactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transactions by date range" });
            }
        }

        // GET /api/transaction/category/{categoryId} - Get transactions by category
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByCategory(int categoryId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var transactions = await _transactionService.GetTransactionsByCategoryAsync(userId, categoryId);
                return Ok(transactions);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving transactions by category" });
            }
        }

        // GET /api/transaction/total-spent - Get total spent
        [HttpGet("total-spent")]
        public async Task<ActionResult<decimal>> GetUserTotalSpent()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var totalSpent = await _transactionService.GetUserTotalSpentAsync(userId);
                return Ok(new { totalSpent });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total spent" });
            }
        }

        // GET /api/transaction/total-income - Get total income
        [HttpGet("total-income")]
        public async Task<ActionResult<decimal>> GetUserTotalIncome()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var totalIncome = await _transactionService.GetUserTotalIncomeAsync(userId);
                return Ok(new { totalIncome });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total income" });
            }
        }

        // Helper method to get current user ID from JWT token
        private int GetCurrentUserId()
        {
            try
            {
                var userIdClaim = User.FindFirst("nameid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }
                
                // Fallback: try to get from HttpContext.Items (set by JwtMiddleware)
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj != null)
                {
                    if (int.TryParse(userIdObj.ToString(), out var contextUserId))
                    {
                        return contextUserId;
                    }
                }
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
} 