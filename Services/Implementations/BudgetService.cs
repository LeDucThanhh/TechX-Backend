using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechX.API.Services.Implementations
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _context;

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BudgetDTO>> GetAllBudgetsAsync()
        {
            var budgets = await _context.Budgets.Include(b => b.Category).ToListAsync();
            return budgets.Select(b => new BudgetDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
                Amount = b.Amount,
                SpentAmount = b.SpentAmount,
                RemainingAmount = b.RemainingAmount,
                Period = b.Period,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            });
        }

        public async Task<BudgetDTO> GetBudgetByIdAsync(int id)
        {
            var b = await _context.Budgets.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) throw new KeyNotFoundException($"Budget {id} not found");
            return new BudgetDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
                Amount = b.Amount,
                SpentAmount = b.SpentAmount,
                RemainingAmount = b.RemainingAmount,
                Period = b.Period,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            };
        }

        public async Task<IEnumerable<BudgetDTO>> GetUserBudgetsAsync(int userId)
        {
            var budgets = await _context.Budgets.Include(b => b.Category).Where(b => b.UserId == userId).ToListAsync();
            return budgets.Select(b => new BudgetDTO
            {
                Id = b.Id,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
                Amount = b.Amount,
                SpentAmount = b.SpentAmount,
                RemainingAmount = b.RemainingAmount,
                Period = b.Period,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            });
        }

        public async Task<BudgetDTO> CreateBudgetAsync(CreateBudgetDTO createDto)
        {
            var b = new Budget
            {
                UserId = createDto.UserId,
                CategoryId = createDto.CategoryId,
                CategoryName = null,
                Amount = createDto.Amount,
                SpentAmount = 0,
                RemainingAmount = createDto.Amount,
                Period = createDto.Period,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Budgets.Add(b);
            await _context.SaveChangesAsync();
            return await GetBudgetByIdAsync(b.Id);
        }

        public async Task<BudgetDTO> UpdateBudgetAsync(int id, UpdateBudgetDTO updateDto)
        {
            var b = await _context.Budgets.FindAsync(id);
            if (b == null) throw new KeyNotFoundException($"Budget {id} not found");
            b.CategoryId = updateDto.CategoryId ?? b.CategoryId;
            b.Amount = updateDto.Amount ?? b.Amount;
            b.Period = updateDto.Period ?? b.Period;
            b.StartDate = updateDto.StartDate ?? b.StartDate;
            b.EndDate = updateDto.EndDate ?? b.EndDate;
            b.UpdatedAt = DateTime.UtcNow;
            b.RemainingAmount = b.Amount - b.SpentAmount;
            await _context.SaveChangesAsync();
            return await GetBudgetByIdAsync(b.Id);
        }

        public async Task<bool> DeleteBudgetAsync(int id)
        {
            var b = await _context.Budgets.FindAsync(id);
            if (b == null) return false;
            _context.Budgets.Remove(b);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BudgetDTO> UpdateBudgetSpentAmountAsync(int id, decimal spentAmount)
        {
            var b = await _context.Budgets.FindAsync(id);
            if (b == null) throw new KeyNotFoundException($"Budget {id} not found");
            b.SpentAmount = spentAmount;
            b.RemainingAmount = b.Amount - spentAmount;
            b.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetBudgetByIdAsync(b.Id);
        }
    }
} 