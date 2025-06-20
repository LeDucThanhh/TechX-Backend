using TechX.API.Models.DTOs;

namespace TechX.API.Services.Interfaces
{
    public interface ILoyaltyService
    {
        Task<IEnumerable<LoyaltyPointsDTO>> GetUserLoyaltyPointsAsync(int userId);
        Task<LoyaltyPointsDTO> GetLoyaltyPointsByIdAsync(int id);
        Task<LoyaltyPointsDTO> CreateLoyaltyPointsAsync(CreateLoyaltyPointsDTO createDto);
        Task<LoyaltyPointsDTO> UpdateLoyaltyPointsAsync(int id, UpdateLoyaltyPointsDTO updateDto);
        Task<bool> DeleteLoyaltyPointsAsync(int id);
        Task<decimal> GetUserTotalLoyaltyPointsAsync(int userId);
    }
} 