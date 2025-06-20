using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace TechX.API.Services.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoreDTO>> GetAllStoresAsync()
        {
            var stores = await _context.Stores
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            var storeDtos = _mapper.Map<List<StoreDTO>>(stores);
            
            // Ensure proper mapping of Logo and Banner properties
            for (int i = 0; i < stores.Count; i++)
            {
                storeDtos[i].Logo = stores[i].Logo;
                storeDtos[i].Banner = stores[i].Banner;
            }

            return storeDtos;
        }

        public async Task<StoreDTO?> GetStoreByIdAsync(int storeId)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == storeId);

            if (store == null)
                return null;

            var storeDto = _mapper.Map<StoreDTO>(store);
            storeDto.Logo = store.Logo;
            storeDto.Banner = store.Banner;
            return storeDto;
        }

        public async Task<StoreDTO> CreateStoreAsync(CreateStoreDTO createStoreDto)
        {
            var store = _mapper.Map<Store>(createStoreDto);
            store.Logo = createStoreDto.Logo;
            store.Banner = createStoreDto.Banner;
            store.CreatedAt = DateTime.UtcNow;
            store.UpdatedAt = DateTime.UtcNow;

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            var storeDto = _mapper.Map<StoreDTO>(store);
            storeDto.Logo = store.Logo;
            storeDto.Banner = store.Banner;
            return storeDto;
        }

        public async Task<StoreDTO?> UpdateStoreAsync(int storeId, UpdateStoreDTO updateStoreDto)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateStoreDto.Name))
                store.Name = updateStoreDto.Name;
            if (!string.IsNullOrEmpty(updateStoreDto.Description))
                store.Description = updateStoreDto.Description;
            if (!string.IsNullOrEmpty(updateStoreDto.Logo))
                store.Logo = updateStoreDto.Logo;
            if (!string.IsNullOrEmpty(updateStoreDto.Banner))
                store.Banner = updateStoreDto.Banner;
            if (!string.IsNullOrEmpty(updateStoreDto.Address))
                store.Address = updateStoreDto.Address;
            if (updateStoreDto.Latitude.HasValue)
                store.Latitude = updateStoreDto.Latitude;
            if (updateStoreDto.Longitude.HasValue)
                store.Longitude = updateStoreDto.Longitude;
            if (!string.IsNullOrEmpty(updateStoreDto.PhoneNumber))
                store.PhoneNumber = updateStoreDto.PhoneNumber;
            if (!string.IsNullOrEmpty(updateStoreDto.Email))
                store.Email = updateStoreDto.Email;
            if (!string.IsNullOrEmpty(updateStoreDto.Website))
                store.Website = updateStoreDto.Website;
            if (!string.IsNullOrEmpty(updateStoreDto.OperatingHours))
                store.OperatingHours = updateStoreDto.OperatingHours;
            if (updateStoreDto.CashbackRate.HasValue)
                store.CashbackRate = updateStoreDto.CashbackRate.Value;
            if (updateStoreDto.PointsRate.HasValue)
                store.PointsRate = updateStoreDto.PointsRate.Value;
            if (updateStoreDto.IsPartner.HasValue)
                store.IsPartner = updateStoreDto.IsPartner.Value;
            if (updateStoreDto.IsActive.HasValue)
                store.IsActive = updateStoreDto.IsActive.Value;

            store.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var storeDto = _mapper.Map<StoreDTO>(store);
            storeDto.Logo = store.Logo;
            storeDto.Banner = store.Banner;
            return storeDto;
        }

        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null)
                return false;

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StoreDTO>> GetNearbyStoresAsync(decimal latitude, decimal longitude, decimal radiusKm = 10)
        {
            var stores = await _context.Stores
                .Where(s => s.IsActive && s.Latitude.HasValue && s.Longitude.HasValue)
                .ToListAsync();

            // Calculate distance and filter by radius
            var nearbyStores = stores
                .Where(s => CalculateDistance(latitude, longitude, s.Latitude!.Value, s.Longitude!.Value) <= radiusKm)
                .OrderBy(s => CalculateDistance(latitude, longitude, s.Latitude!.Value, s.Longitude!.Value))
                .ToList();

            var storeDtos = _mapper.Map<List<StoreDTO>>(nearbyStores);
            
            // Ensure proper mapping of Logo and Banner properties
            for (int i = 0; i < nearbyStores.Count; i++)
            {
                storeDtos[i].Logo = nearbyStores[i].Logo;
                storeDtos[i].Banner = nearbyStores[i].Banner;
                storeDtos[i].Distance = CalculateDistance(latitude, longitude, nearbyStores[i].Latitude!.Value, nearbyStores[i].Longitude!.Value);
            }

            return storeDtos;
        }

        public async Task<List<StoreDTO>> SearchStoresAsync(string searchTerm)
        {
            var stores = await _context.Stores
                .Where(s => s.IsActive && 
                           (s.Name.Contains(searchTerm) || 
                            s.Description!.Contains(searchTerm) || 
                            s.Address.Contains(searchTerm)))
                .OrderBy(s => s.Name)
                .ToListAsync();

            var storeDtos = _mapper.Map<List<StoreDTO>>(stores);
            
            // Ensure proper mapping of Logo and Banner properties
            for (int i = 0; i < stores.Count; i++)
            {
                storeDtos[i].Logo = stores[i].Logo;
                storeDtos[i].Banner = stores[i].Banner;
            }

            return storeDtos;
        }

        private decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const decimal R = 6371; // Earth's radius in kilometers
            var dLat = (lat2 - lat1) * (decimal)Math.PI / 180;
            var dLon = (lon2 - lon1) * (decimal)Math.PI / 180;
            var a = (decimal)Math.Sin((double)(dLat / 2)) * (decimal)Math.Sin((double)(dLat / 2)) +
                    (decimal)Math.Cos((double)(lat1 * (decimal)Math.PI / 180)) * (decimal)Math.Cos((double)(lat2 * (decimal)Math.PI / 180)) *
                    (decimal)Math.Sin((double)(dLon / 2)) * (decimal)Math.Sin((double)(dLon / 2));
            var c = 2 * (decimal)Math.Atan2((double)(decimal)Math.Sqrt((double)a), (double)(decimal)Math.Sqrt((double)(1 - a)));
            return R * c;
        }
    }
} 