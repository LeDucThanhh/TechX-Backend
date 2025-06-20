using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TechX.API.Helpers;

namespace TechX.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHelper _passwordHelper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _passwordHelper = new PasswordHelper();
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Settings)
                .ToListAsync();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                ProfilePicture = u.Avatar,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            });
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.ProfilePicture = user.Avatar;
            return userDto;
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.ProfilePicture = user.Avatar;
            return userDto;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            user.Avatar = createUserDto.Avatar;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.ProfilePicture = user.Avatar;
            return userDto;
        }

        public async Task<UserDTO?> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(updateUserDto.FirstName))
                user.FirstName = updateUserDto.FirstName;
            if (!string.IsNullOrEmpty(updateUserDto.LastName))
                user.LastName = updateUserDto.LastName;
            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
                user.PhoneNumber = updateUserDto.PhoneNumber;
            if (!string.IsNullOrEmpty(updateUserDto.ProfilePicture))
                user.Avatar = updateUserDto.ProfilePicture;
            if (updateUserDto.DateOfBirth.HasValue)
                user.DateOfBirth = updateUserDto.DateOfBirth;
            if (!string.IsNullOrEmpty(updateUserDto.Gender))
                user.Gender = updateUserDto.Gender;
            if (!string.IsNullOrEmpty(updateUserDto.Address))
                user.Address = updateUserDto.Address;
            if (!string.IsNullOrEmpty(updateUserDto.Preferences))
                user.Preferences = updateUserDto.Preferences;

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.ProfilePicture = user.Avatar;
            return userDto;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.Password))
                return false;

            if (!_passwordHelper.VerifyPassword(currentPassword, user.Password))
                return false;

            user.Password = _passwordHelper.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyEmailAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsEmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyPhoneAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsPhoneVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 