using TechX.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDTO?> GetTransactionByIdAsync(int transactionId);
        Task<List<TransactionDTO>> GetUserTransactionsAsync(int userId, int page = 1, int pageSize = 20);
        Task<List<TransactionDTO>> GetTransactionsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<List<TransactionDTO>> GetTransactionsByCategoryAsync(int userId, int categoryId);
        Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO createTransactionDto);
        Task<TransactionDTO?> UpdateTransactionAsync(int transactionId, UpdateTransactionDTO updateTransactionDto);
        Task<bool> DeleteTransactionAsync(int transactionId);
        Task<decimal> GetUserTotalSpentAsync(int userId);
        Task<decimal> GetUserTotalIncomeAsync(int userId);
        Task<TransactionSummaryDTO> GetTransactionSummaryAsync(int userId, DateTime startDate, DateTime endDate);
    }
} 