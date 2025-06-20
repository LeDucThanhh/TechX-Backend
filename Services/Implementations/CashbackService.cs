using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechX.API.Services.Implementations
{
    public class CashbackService : ICashbackService
    {
        private readonly ApplicationDbContext _context;

        public CashbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CashbackTransactionDTO>> GetUserCashbackTransactionsAsync(int userId)
        {
            var cashbackTransactions = await _context.CashbackTransactions
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.TransactionDate)
                .ToListAsync();

            return cashbackTransactions.Select(c => new CashbackTransactionDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                TransactionId = c.TransactionId,
                StoreId = c.StoreId,
                StoreName = c.StoreName,
                TransactionAmount = c.TransactionAmount,
                CashbackAmount = c.CashbackAmount,
                CashbackRate = c.CashbackRate,
                Status = c.Status,
                TransactionDate = c.TransactionDate,
                ApprovedDate = c.ApprovedDate,
                PaidDate = c.PaidDate,
                Notes = c.Notes,
                RejectionReason = c.RejectionReason,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<CashbackTransactionDTO> GetCashbackTransactionByIdAsync(int id)
        {
            var c = await _context.CashbackTransactions.FindAsync(id);
            if (c == null) throw new KeyNotFoundException($"Cashback transaction {id} not found");
            return new CashbackTransactionDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                TransactionId = c.TransactionId,
                StoreId = c.StoreId,
                StoreName = c.StoreName,
                TransactionAmount = c.TransactionAmount,
                CashbackAmount = c.CashbackAmount,
                CashbackRate = c.CashbackRate,
                Status = c.Status,
                TransactionDate = c.TransactionDate,
                ApprovedDate = c.ApprovedDate,
                PaidDate = c.PaidDate,
                Notes = c.Notes,
                RejectionReason = c.RejectionReason,
                CreatedAt = c.CreatedAt
            };
        }

        public async Task<CashbackTransactionDTO> CreateCashbackTransactionAsync(CreateCashbackTransactionDTO createDto)
        {
            var c = new CashbackTransaction
            {
                UserId = createDto.UserId,
                TransactionId = createDto.TransactionId,
                StoreId = createDto.StoreId,
                StoreName = createDto.StoreName,
                TransactionAmount = createDto.TransactionAmount,
                CashbackAmount = createDto.CashbackAmount,
                CashbackRate = createDto.CashbackRate,
                Status = "pending",
                TransactionDate = createDto.TransactionDate,
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            _context.CashbackTransactions.Add(c);
            await _context.SaveChangesAsync();
            return await GetCashbackTransactionByIdAsync(c.Id);
        }

        public async Task<CashbackTransactionDTO> UpdateCashbackTransactionAsync(int id, UpdateCashbackTransactionDTO updateDto)
        {
            var c = await _context.CashbackTransactions.FindAsync(id);
            if (c == null) throw new KeyNotFoundException($"Cashback transaction {id} not found");
            c.Status = updateDto.Status ?? c.Status;
            c.ApprovedDate = updateDto.ApprovedDate ?? c.ApprovedDate;
            c.PaidDate = updateDto.PaidDate ?? c.PaidDate;
            c.Notes = updateDto.Notes ?? c.Notes;
            c.RejectionReason = updateDto.RejectionReason ?? c.RejectionReason;
            await _context.SaveChangesAsync();
            return await GetCashbackTransactionByIdAsync(c.Id);
        }

        public async Task<bool> DeleteCashbackTransactionAsync(int id)
        {
            var c = await _context.CashbackTransactions.FindAsync(id);
            if (c == null) return false;
            _context.CashbackTransactions.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetUserTotalCashbackAsync(int userId)
        {
            return await _context.CashbackTransactions
                .Where(c => c.UserId == userId && c.Status == "paid")
                .SumAsync(c => c.CashbackAmount);
        }
    }
} 