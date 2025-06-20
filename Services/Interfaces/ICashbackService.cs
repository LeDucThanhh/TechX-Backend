using TechX.API.Models.DTOs;

namespace TechX.API.Services.Interfaces
{
    public interface ICashbackService
    {
        Task<IEnumerable<CashbackTransactionDTO>> GetUserCashbackTransactionsAsync(int userId);
        Task<CashbackTransactionDTO> GetCashbackTransactionByIdAsync(int id);
        Task<CashbackTransactionDTO> CreateCashbackTransactionAsync(CreateCashbackTransactionDTO createDto);
        Task<CashbackTransactionDTO> UpdateCashbackTransactionAsync(int id, UpdateCashbackTransactionDTO updateDto);
        Task<bool> DeleteCashbackTransactionAsync(int id);
        Task<decimal> GetUserTotalCashbackAsync(int userId);
    }
} 