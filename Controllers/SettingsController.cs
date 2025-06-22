using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechX.API.Data;
using TechX.API.Models;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/settings/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Setting>> GetUserSettings(int userId)
        {
            try
            {
                var settings = await _context.Settings.FindAsync(userId);

                if (settings == null)
                {
                    // Create default settings if not found
                    settings = new Setting
                    {
                        UserId = userId,
                        Language = "en",
                        Currency = "USD",
                        Theme = "light",
                        NotificationsEnabled = true,
                        PushNotificationsEnabled = true,
                        EmailNotificationsEnabled = true,
                        BiometricEnabled = false,
                        AutoSyncEnabled = true,
                        DateFormat = "MM/dd/yyyy",
                        TimeFormat = "12h"
                    };

                    _context.Settings.Add(settings);
                    await _context.SaveChangesAsync();
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/settings/user/{userId}
        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUserSettings(int userId, Setting settings)
        {
            try
            {
                if (userId != settings.UserId)
                {
                    return BadRequest();
                }

                var existingSettings = await _context.Settings.FindAsync(userId);

                if (existingSettings == null)
                {
                    settings.CreatedAt = DateTime.UtcNow;
                    _context.Settings.Add(settings);
                }
                else
                {
                    existingSettings.Language = settings.Language;
                    existingSettings.Currency = settings.Currency;
                    existingSettings.Theme = settings.Theme;
                    existingSettings.NotificationsEnabled = settings.NotificationsEnabled;
                    existingSettings.PushNotificationsEnabled = settings.PushNotificationsEnabled;
                    existingSettings.EmailNotificationsEnabled = settings.EmailNotificationsEnabled;
                    existingSettings.BiometricEnabled = settings.BiometricEnabled;
                    existingSettings.AutoSyncEnabled = settings.AutoSyncEnabled;
                    existingSettings.DateFormat = settings.DateFormat;
                    existingSettings.TimeFormat = settings.TimeFormat;
                    existingSettings.NotificationTypes = settings.NotificationTypes;
                    existingSettings.PrivacySettings = settings.PrivacySettings;
                    existingSettings.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/settings/user/{userId}/reset
        [HttpPost("user/{userId}/reset")]
        public async Task<IActionResult> ResetUserSettings(int userId)
        {
            try
            {
                var settings = await _context.Settings.FindAsync(userId);

                if (settings == null)
                {
                    return NotFound();
                }

                // Reset to default values
                settings.Language = "en";
                settings.Currency = "USD";
                settings.Theme = "light";
                settings.NotificationsEnabled = true;
                settings.PushNotificationsEnabled = true;
                settings.EmailNotificationsEnabled = true;
                settings.BiometricEnabled = false;
                settings.AutoSyncEnabled = true;
                settings.DateFormat = "MM/dd/yyyy";
                settings.TimeFormat = "12h";
                settings.NotificationTypes = null;
                settings.PrivacySettings = null;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Settings reset to default successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/settings/user/{userId}/theme
        [HttpPut("user/{userId}/theme")]
        public async Task<IActionResult> UpdateTheme(int userId, [FromBody] string theme)
        {
            try
            {
                var settings = await _context.Settings.FindAsync(userId);

                if (settings == null)
                {
                    return NotFound();
                }

                settings.Theme = theme;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Theme updated successfully", theme });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/settings/user/{userId}/language
        [HttpPut("user/{userId}/language")]
        public async Task<IActionResult> UpdateLanguage(int userId, [FromBody] string language)
        {
            try
            {
                var settings = await _context.Settings.FindAsync(userId);

                if (settings == null)
                {
                    return NotFound();
                }

                settings.Language = language;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Language updated successfully", language });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/settings/user/{userId}/notifications
        [HttpPut("user/{userId}/notifications")]
        public async Task<IActionResult> UpdateNotificationSettings(int userId, [FromBody] NotificationSettingsRequest request)
        {
            try
            {
                var settings = await _context.Settings.FindAsync(userId);

                if (settings == null)
                {
                    return NotFound();
                }

                settings.NotificationsEnabled = request.NotificationsEnabled;
                settings.PushNotificationsEnabled = request.PushNotificationsEnabled;
                settings.EmailNotificationsEnabled = request.EmailNotificationsEnabled;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Notification settings updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class NotificationSettingsRequest
    {
        public bool NotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
    }
} 