using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("settings")]
    public class Setting
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(10)]
        [Column("language")]
        public string Language { get; set; } = "en";

        [MaxLength(10)]
        [Column("currency")]
        public string Currency { get; set; } = "USD";

        [MaxLength(20)]
        [Column("theme")]
        public string Theme { get; set; } = "light";

        [Column("notifications_enabled")]
        public bool NotificationsEnabled { get; set; } = true;

        [Column("push_notifications_enabled")]
        public bool PushNotificationsEnabled { get; set; } = true;

        [Column("email_notifications_enabled")]
        public bool EmailNotificationsEnabled { get; set; } = true;

        [Column("biometric_enabled")]
        public bool BiometricEnabled { get; set; } = false;

        [Column("auto_sync_enabled")]
        public bool AutoSyncEnabled { get; set; } = true;

        [MaxLength(20)]
        [Column("date_format")]
        public string DateFormat { get; set; } = "MM/dd/yyyy";

        [MaxLength(10)]
        [Column("time_format")]
        public string TimeFormat { get; set; } = "12h";

        [Column("notification_types")]
        public string? NotificationTypes { get; set; }

        [Column("privacy_settings")]
        public string? PrivacySettings { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 