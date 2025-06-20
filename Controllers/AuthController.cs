using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthController(IAuthService authService, IGoogleAuthService googleAuthService)
        {
            _authService = authService;
            _googleAuthService = googleAuthService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while refreshing token" });
            }
        }

        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(new { message = "Token revoked successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while revoking token" });
            }
        }

        [HttpPost("google")]
        public async Task<ActionResult<AuthResponseDTO>> GoogleAuth([FromBody] GoogleAuthDTO googleAuthDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _googleAuthService.AuthenticateWithGoogleAsync(googleAuthDto.GoogleToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred during Google authentication" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                // Get user ID from JWT token
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(new { message = "Invalid token." });
                }

                var result = await _authService.ChangePasswordAsync(userId, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                if (result)
                {
                    return Ok(new { message = "Password changed successfully." });
                }
                return BadRequest(new { message = "Current password is incorrect." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while changing password." });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.ForgotPasswordAsync(request.Email);
                if (result)
                {
                    return Ok(new { message = "Password reset email sent successfully." });
                }
                return BadRequest(new { message = "Email not found." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while processing forgot password request." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
                if (result)
                {
                    return Ok(new { message = "Password reset successfully." });
                }
                return BadRequest(new { message = "Invalid or expired token." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while resetting password." });
            }
        }

        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOTP([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.SendOTPAsync(request.Email);
                if (result)
                {
                    return Ok(new { message = "OTP sent successfully." });
                }
                return BadRequest(new { message = "Email not found." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while sending OTP." });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifyOTP([FromBody] VerifyOtpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(TechX.API.Helpers.ValidationHelper.GetValidationErrors(ModelState));
            }

            try
            {
                var result = await _authService.VerifyOTPAsync(request.Email, request.OtpCode);
                if (result)
                {
                    return Ok(new { message = "OTP verified successfully." });
                }
                return BadRequest(new { message = "Invalid OTP." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while verifying OTP." });
            }
        }
    }
}