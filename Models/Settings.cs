using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("settings")]
    public class Settings
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("language")]
        [StringLength(10)]
        public string Language { get; set; } = "vi"; // 'vi', 'en'

        [Required]
        [Column("currency")]
        [StringLength(3)]
        public string Currency { get; set; } = "VND"; // 'VND', 'USD', 'EUR'

        [Required]
        [Column("theme")]
        [StringLength(20)]
        public string Theme { get; set; } = "light"; // 'light', 'dark', 'auto'

        [Column("enable_push_notifications")]
        public bool EnablePushNotifications { get; set; } = true;

        [Column("enable_email_notifications")]
        public bool EnableEmailNotifications { get; set; } = true;

        [Column("budget_alerts")]
        public bool BudgetAlerts { get; set; } = true;

        [Column("cashback_alerts")]
        public bool CashbackAlerts { get; set; } = true;

        [Column("loyalty_alerts")]
        public bool LoyaltyAlerts { get; set; } = true;

        [Column("privacy_settings", TypeName = "jsonb")]
        public string? PrivacySettings { get; set; } // JSON object for privacy preferences

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 