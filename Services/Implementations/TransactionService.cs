using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TechX.API.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDTO?> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Store)
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
                return null;

            var transactionDto = _mapper.Map<TransactionDTO>(transaction);
            transactionDto.Type = transaction.Type;
            transactionDto.Date = transaction.Date;
            return transactionDto;
        }

        public async Task<List<TransactionDTO>> GetUserTransactionsAsync(int userId, int page = 1, int pageSize = 20)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Store)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var transactionDtos = _mapper.Map<List<TransactionDTO>>(transactions);
            
            for (int i = 0; i < transactions.Count; i++)
            {
                transactionDtos[i].Type = transactions[i].Type;
                transactionDtos[i].Date = transactions[i].Date;
            }

            return transactionDtos;
        }

        public async Task<List<TransactionDTO>> GetTransactionsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Store)
                .Where(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactionDtos = _mapper.Map<List<TransactionDTO>>(transactions);
            
            for (int i = 0; i < transactions.Count; i++)
            {
                transactionDtos[i].Type = transactions[i].Type;
                transactionDtos[i].Date = transactions[i].Date;
            }

            return transactionDtos;
        }

        public async Task<List<TransactionDTO>> GetTransactionsByCategoryAsync(int userId, int categoryId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Store)
                .Where(t => t.UserId == userId && t.CategoryId == categoryId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var transactionDtos = _mapper.Map<List<TransactionDTO>>(transactions);
            
            for (int i = 0; i < transactions.Count; i++)
            {
                transactionDtos[i].Type = transactions[i].Type;
                transactionDtos[i].Date = transactions[i].Date;
            }

            return transactionDtos;
        }

        public async Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO createTransactionDto)
        {
            var transaction = _mapper.Map<Transaction>(createTransactionDto);
            transaction.Type = createTransactionDto.Type;
            transaction.Date = createTransactionDto.Date;
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDto = _mapper.Map<TransactionDTO>(transaction);
            transactionDto.Type = transaction.Type;
            transactionDto.Date = transaction.Date;
            return transactionDto;
        }

        public async Task<TransactionDTO?> UpdateTransactionAsync(int transactionId, UpdateTransactionDTO updateTransactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
                return null;

            if (updateTransactionDto.StoreId.HasValue)
                transaction.StoreId = updateTransactionDto.StoreId;
            if (updateTransactionDto.CategoryId.HasValue)
                transaction.CategoryId = updateTransactionDto.CategoryId.Value;
            if (updateTransactionDto.Amount.HasValue)
                transaction.Amount = updateTransactionDto.Amount.Value;
            if (!string.IsNullOrEmpty(updateTransactionDto.Type))
                transaction.Type = updateTransactionDto.Type;
            if (!string.IsNullOrEmpty(updateTransactionDto.Description))
                transaction.Description = updateTransactionDto.Description;
            if (updateTransactionDto.Date.HasValue)
                transaction.Date = updateTransactionDto.Date.Value;
            if (!string.IsNullOrEmpty(updateTransactionDto.ReceiptUrl))
                transaction.ReceiptUrl = updateTransactionDto.ReceiptUrl;
            if (!string.IsNullOrEmpty(updateTransactionDto.Tags))
                transaction.Tags = updateTransactionDto.Tags;
            if (updateTransactionDto.IsRecurring.HasValue)
                transaction.IsRecurring = updateTransactionDto.IsRecurring.Value;
            if (!string.IsNullOrEmpty(updateTransactionDto.RecurringType))
                transaction.RecurringType = updateTransactionDto.RecurringType;
            if (!string.IsNullOrEmpty(updateTransactionDto.Notes))
                transaction.Notes = updateTransactionDto.Notes;

            transaction.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var transactionDto = _mapper.Map<TransactionDTO>(transaction);
            transactionDto.Type = transaction.Type;
            transactionDto.Date = transaction.Date;
            return transactionDto;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
                return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetUserTotalSpentAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "expense")
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetUserTotalIncomeAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "income")
                .SumAsync(t => t.Amount);
        }

        public async Task<TransactionSummaryDTO> GetTransactionSummaryAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();

            var totalIncome = transactions.Where(t => t.Type == "income").Sum(t => t.Amount);
            var totalExpense = transactions.Where(t => t.Type == "expense").Sum(t => t.Amount);
            var totalSaved = totalIncome - totalExpense;

            var spendingByCategory = transactions
                .Where(t => t.Type == "expense")
                .GroupBy(t => t.Category?.Name ?? "Unknown")
                .Select(g => new CategorySpending
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(c => c.Amount)
                .ToList();

            return new TransactionSummaryDTO
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                TotalSaved = totalSaved,
                SpendingByCategory = spendingByCategory
            };
        }
    }
} 