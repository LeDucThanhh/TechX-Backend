using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechX.API.Services.Implementations
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly ApplicationDbContext _context;

        public LoyaltyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoyaltyPointsDTO>> GetUserLoyaltyPointsAsync(int userId)
        {
            var loyaltyPoints = await _context.LoyaltyPoints
                .Include(l => l.User)
                .Include(l => l.Store)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.ExpiryDate)
                .ToListAsync();

            return loyaltyPoints.Select(l => new LoyaltyPointsDTO
            {
                Id = l.Id,
                UserId = l.UserId,
                StoreId = l.StoreId,
                StoreName = l.Store?.Name,
                Points = l.Points,
                PointsValue = l.PointsValue,
                ExpiryDate = l.ExpiryDate,
                Status = l.Status,
                CreatedAt = l.CreatedAt,
                UsedAt = l.UsedAt
            });
        }

        public async Task<LoyaltyPointsDTO> GetLoyaltyPointsByIdAsync(int id)
        {
            var l = await _context.LoyaltyPoints
                .Include(lp => lp.User)
                .Include(lp => lp.Store)
                .FirstOrDefaultAsync(lp => lp.Id == id);

            if (l == null)
                throw new KeyNotFoundException($"Loyalty points with ID {id} not found");

            return new LoyaltyPointsDTO
            {
                Id = l.Id,
                UserId = l.UserId,
                StoreId = l.StoreId,
                StoreName = l.Store?.Name,
                Points = l.Points,
                PointsValue = l.PointsValue,
                ExpiryDate = l.ExpiryDate,
                Status = l.Status,
                CreatedAt = l.CreatedAt,
                UsedAt = l.UsedAt
            };
        }

        public async Task<LoyaltyPointsDTO> CreateLoyaltyPointsAsync(CreateLoyaltyPointsDTO createDto)
        {
            var loyaltyPoints = new LoyaltyPoints
            {
                UserId = createDto.UserId,
                StoreId = createDto.StoreId,
                Points = createDto.Points,
                PointsValue = createDto.PointsValue,
                ExpiryDate = createDto.ExpiryDate,
                Status = "active",
                CreatedAt = DateTime.UtcNow
            };

            _context.LoyaltyPoints.Add(loyaltyPoints);
            await _context.SaveChangesAsync();

            return await GetLoyaltyPointsByIdAsync(loyaltyPoints.Id);
        }

        public async Task<LoyaltyPointsDTO> UpdateLoyaltyPointsAsync(int id, UpdateLoyaltyPointsDTO updateDto)
        {
            var loyaltyPoints = await _context.LoyaltyPoints.FindAsync(id);
            if (loyaltyPoints == null)
                throw new KeyNotFoundException($"Loyalty points with ID {id} not found");

            loyaltyPoints.Points = updateDto.Points ?? loyaltyPoints.Points;
            loyaltyPoints.PointsValue = updateDto.PointsValue ?? loyaltyPoints.PointsValue;
            loyaltyPoints.ExpiryDate = updateDto.ExpiryDate ?? loyaltyPoints.ExpiryDate;
            loyaltyPoints.Status = updateDto.Status ?? loyaltyPoints.Status;

            await _context.SaveChangesAsync();

            return await GetLoyaltyPointsByIdAsync(loyaltyPoints.Id);
        }

        public async Task<bool> DeleteLoyaltyPointsAsync(int id)
        {
            var loyaltyPoints = await _context.LoyaltyPoints.FindAsync(id);
            if (loyaltyPoints == null)
                return false;

            _context.LoyaltyPoints.Remove(loyaltyPoints);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetUserTotalLoyaltyPointsAsync(int userId)
        {
            return await _context.LoyaltyPoints
                .Where(l => l.UserId == userId && l.Status == "active")
                .SumAsync(l => l.PointsValue);
        }
    }
} 