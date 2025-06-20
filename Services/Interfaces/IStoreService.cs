using TechX.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Services.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDTO>> GetAllStoresAsync();
        Task<StoreDTO?> GetStoreByIdAsync(int storeId);
        Task<StoreDTO> CreateStoreAsync(CreateStoreDTO createStoreDto);
        Task<StoreDTO?> UpdateStoreAsync(int storeId, UpdateStoreDTO updateStoreDto);
        Task<bool> DeleteStoreAsync(int storeId);
        Task<List<StoreDTO>> GetNearbyStoresAsync(decimal latitude, decimal longitude, decimal radiusKm = 10);
        Task<List<StoreDTO>> SearchStoresAsync(string searchTerm);
    }
} 