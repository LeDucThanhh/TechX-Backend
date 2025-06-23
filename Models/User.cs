using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        [Column("gender")]
        public string? Gender { get; set; }

        [MaxLength(500)]
        [Column("address")]
        public string? Address { get; set; }

        [Column("is_email_verified")]
        public bool IsEmailVerified { get; set; } = false;

        [Column("is_phone_verified")]
        public bool IsPhoneVerified { get; set; } = false;

        [Column("preferences")]
        public string? Preferences { get; set; } // JSONB stored as string

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [MaxLength(255)]
        [Column("google_id")]
        public string? GoogleId { get; set; }

        [MaxLength(500)]
        [Column("google_picture")]
        public string? GooglePicture { get; set; }

        [MaxLength(10)]
        [Column("auth_provider")]
        public string AuthProvider { get; set; } = "Email";

        [MaxLength(6)]
        [Column("otp_code")]
        public string? OtpCode { get; set; }

        [Column("otp_expiry_time")]
        public DateTime? OtpExpiryTime { get; set; }

        [Column("otp_attempts")]
        public int OtpAttempts { get; set; } = 0;

        [Column("last_otp_request_time")]
        public DateTime? LastOtpRequestTime { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - no initialization to avoid EF conflicts
        public virtual ICollection<Transaction> Transactions { get; set; } = null!;
        public virtual ICollection<Budget> Budgets { get; set; } = null!;
        public virtual ICollection<Receipt> Receipts { get; set; } = null!;
        public virtual ICollection<LoyaltyPoint> LoyaltyPoints { get; set; } = null!;
        public virtual ICollection<CashbackTransaction> CashbackTransactions { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; } = null!;
        public virtual ICollection<UserVoucher> UserVouchers { get; set; } = null!;
        public virtual Setting? Settings { get; set; }
    }
} 