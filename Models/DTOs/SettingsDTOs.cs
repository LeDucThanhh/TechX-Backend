using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class SettingsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string Language { get; set; } = "en";
        public string Theme { get; set; } = "light";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateSettingsDTO
    {
        public bool? NotificationsEnabled { get; set; }
        public string? Language { get; set; }
        public string? Theme { get; set; }
    }
} 