using TechX.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetDTO>> GetAllBudgetsAsync();
        Task<BudgetDTO> GetBudgetByIdAsync(int id);
        Task<IEnumerable<BudgetDTO>> GetUserBudgetsAsync(int userId);
        Task<BudgetDTO> CreateBudgetAsync(CreateBudgetDTO createDto);
        Task<BudgetDTO> UpdateBudgetAsync(int id, UpdateBudgetDTO updateDto);
        Task<bool> DeleteBudgetAsync(int id);
        Task<BudgetDTO> UpdateBudgetSpentAmountAsync(int id, decimal spentAmount);
    }
} 