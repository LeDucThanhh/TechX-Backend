using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using TechX.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace TechX.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHelper _passwordHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ApplicationDbContext context,
            JwtHelper jwtHelper,
            PasswordHelper passwordHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _passwordHelper = passwordHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || user.Password == null || !_passwordHelper.VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var tokenResult = _jwtHelper.GenerateTokens(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                User = new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    ProfilePicture = user.Avatar,
                    Address = user.Address,
                    IsEmailVerified = user.IsEmailVerified,
                    IsPhoneVerified = user.IsPhoneVerified,
                    Preferences = user.Preferences,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive,
                    GoogleId = user.GoogleId,
                    GooglePicture = user.GooglePicture,
                    AuthProvider = user.AuthProvider,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var hashedPassword = _passwordHelper.HashPassword(registerDto.Password);

            var user = new User
            {
                Email = registerDto.Email,
                Password = hashedPassword,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Gender = registerDto.Gender,
                DateOfBirth = registerDto.DateOfBirth.HasValue ? DateTime.SpecifyKind(registerDto.DateOfBirth.Value, DateTimeKind.Utc) : (DateTime?)null,
                AuthProvider = "Email",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var tokenResult = _jwtHelper.GenerateTokens(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                User = new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    ProfilePicture = user.Avatar,
                    Address = user.Address,
                    IsEmailVerified = user.IsEmailVerified,
                    IsPhoneVerified = user.IsPhoneVerified,
                    Preferences = user.Preferences,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive,
                    GoogleId = user.GoogleId,
                    GooglePicture = user.GooglePicture,
                    AuthProvider = user.AuthProvider,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var tokenResult = _jwtHelper.GenerateTokens(storedToken.User);
            
            // Revoke the old refresh token
            storedToken.IsRevoked = true;
            storedToken.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                User = new UserDTO
                {
                    Id = storedToken.User.Id,
                    Email = storedToken.User.Email,
                    FirstName = storedToken.User.FirstName,
                    LastName = storedToken.User.LastName,
                    PhoneNumber = storedToken.User.PhoneNumber,
                    DateOfBirth = storedToken.User.DateOfBirth,
                    Gender = storedToken.User.Gender,
                    ProfilePicture = storedToken.User.Avatar,
                    IsActive = storedToken.User.IsActive,
                    CreatedAt = storedToken.User.CreatedAt,
                    UpdatedAt = storedToken.User.UpdatedAt
                }
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null)
            {
                return false;
            }

            storedToken.IsRevoked = true;
            storedToken.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Password == null)
            {
                return false;
            }

            if (!_passwordHelper.VerifyPassword(currentPassword, user.Password))
            {
                return false;
            }

            user.Password = _passwordHelper.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            // Generate OTP and send email
            // This is a simplified implementation
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            // Verify token and reset password
            // This is a simplified implementation
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            // Verify email token
            // This is a simplified implementation
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> SendOTPAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            // Generate and send OTP
            // This is a simplified implementation
            return true;
        }

        public async Task<bool> VerifyOTPAsync(string email, string otpCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            // Verify OTP
            // This is a simplified implementation
            return true;
        }
    }
} 