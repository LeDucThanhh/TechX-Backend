using TechX.API.Models.DTOs;

namespace TechX.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginDTO request);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> VerifyEmailAsync(string token);
        Task<bool> SendOTPAsync(string email);
        Task<bool> VerifyOTPAsync(string email, string otpCode);
    }
} 