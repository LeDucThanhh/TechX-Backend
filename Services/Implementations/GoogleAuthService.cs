using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using TechX.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace TechX.API.Services.Implementations
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GoogleAuthService(
            ApplicationDbContext context,
            JwtHelper jwtHelper,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> AuthenticateWithGoogleAsync(string googleToken)
        {
            GoogleUserInfo? googleUserInfo = null;
            
            try
            {
                Console.WriteLine($"Attempting Google auth with token: {googleToken.Substring(0, Math.Min(50, googleToken.Length))}...");
                
                googleUserInfo = await VerifyGoogleTokenAsync(googleToken);
                
                if (googleUserInfo == null)
                {
                    Console.WriteLine("Google token verification failed");
                    throw new UnauthorizedAccessException("Invalid Google token. Please try signing in again.");
                }
                
                Console.WriteLine($"Google auth successful for: {googleUserInfo.Email}");
            }
            catch (Exception ex) when (!(ex is UnauthorizedAccessException))
            {
                Console.WriteLine($"Google auth error: {ex.Message}");
                throw new UnauthorizedAccessException($"Google authentication failed: {ex.Message}");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == googleUserInfo.Email || u.GoogleId == googleUserInfo.GoogleId);

            if (user == null)
            {
                // Create new user from Google data
                user = new User
                {
                    Email = googleUserInfo.Email,
                    FirstName = googleUserInfo.FirstName,
                    LastName = googleUserInfo.LastName,
                    Avatar = googleUserInfo.ProfilePicture,
                    GoogleId = googleUserInfo.GoogleId,
                    GooglePicture = googleUserInfo.ProfilePicture,
                    IsActive = true,
                    IsEmailVerified = true, // Google emails are verified
                    AuthProvider = "Google",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else if (user.AuthProvider == "Email" && string.IsNullOrEmpty(user.GoogleId))
            {
                // Link existing email account with Google
                user.GoogleId = googleUserInfo.GoogleId;
                user.GooglePicture = googleUserInfo.ProfilePicture;
                user.IsEmailVerified = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
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

        private async Task<GoogleUserInfo?> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                // Get Google OAuth configuration
                var clientId = _configuration["GoogleAuth:ClientId"];
                var tokenUri = _configuration["GoogleAuth:TokenUri"];
                
                // Verify token with Google's tokeninfo endpoint
                var tokenInfoUrl = $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}";
                
                var response = await _httpClient.GetAsync(tokenInfoUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                
                // Verify the token audience (client_id)
                if (tokenInfo.TryGetProperty("aud", out var audience))
                {
                    if (audience.GetString() != clientId)
                    {
                        return null; // Token not intended for this application
                    }
                }
                else
                {
                    return null;
                }
                
                // Verify token expiration
                if (tokenInfo.TryGetProperty("exp", out var expElement))
                {
                    var exp = expElement.GetInt64();
                    var expDateTime = DateTimeOffset.FromUnixTimeSeconds(exp);
                    if (expDateTime < DateTimeOffset.UtcNow)
                    {
                        return null; // Token expired
                    }
                }
                
                // Extract user information
                return new GoogleUserInfo
                {
                    GoogleId = tokenInfo.GetProperty("sub").GetString() ?? "", // 'sub' is the Google user ID
                    Email = tokenInfo.GetProperty("email").GetString() ?? "",
                    FirstName = tokenInfo.TryGetProperty("given_name", out var firstName) ? firstName.GetString() ?? "" : "",
                    LastName = tokenInfo.TryGetProperty("family_name", out var lastName) ? lastName.GetString() ?? "" : "",
                    ProfilePicture = tokenInfo.TryGetProperty("picture", out var picture) ? picture.GetString() : null
                };
            }
            catch (Exception ex)
            {
                // Log the exception in a real implementation
                Console.WriteLine($"Error verifying Google token: {ex.Message}");
                return null;
            }
        }

        private class GoogleUserInfo
        {
            public string GoogleId { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string? ProfilePicture { get; set; }
        }
    }
}