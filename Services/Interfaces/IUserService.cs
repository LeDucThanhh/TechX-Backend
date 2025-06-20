using TechX.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto);
        Task<UserDTO?> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> VerifyEmailAsync(int userId);
        Task<bool> VerifyPhoneAsync(int userId);
    }
} 