using TechX.API.Models.DTOs;
using System.Threading.Tasks;

namespace TechX.API.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<AuthResponseDTO> AuthenticateWithGoogleAsync(string googleToken);
    }
} 