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
            var googleUserInfo = await VerifyGoogleTokenAsync(googleToken);
            
            if (googleUserInfo == null)
            {
                throw new UnauthorizedAccessException("Invalid Google token");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == googleUserInfo.Email);

            if (user == null)
            {
                // Create new user from Google data
                user = new User
                {
                    Email = googleUserInfo.Email,
                    FirstName = googleUserInfo.FirstName,
                    LastName = googleUserInfo.LastName,
                    Avatar = googleUserInfo.ProfilePicture,
                    IsActive = true,
                    AuthProvider = "Google",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
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
                    IsActive = user.IsActive,
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
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string? ProfilePicture { get; set; }
        }
    }
}