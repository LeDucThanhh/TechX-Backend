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
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Column("password")]
        [StringLength(255)]
        public string? Password { get; set; } // Nullable for Google users
        
        [Required]
        [Column("first_name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [Column("last_name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Column("phone_number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Column("avatar")]
        [StringLength(500)]
        public string? Avatar { get; set; }
        
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }
        
        [Column("gender")]
        [StringLength(10)]
        public string? Gender { get; set; } // 'male', 'female', 'other'
        
        [Column("address")]
        [StringLength(500)]
        public string? Address { get; set; }
        
        [Column("is_email_verified")]
        public bool IsEmailVerified { get; set; } = false;
        
        [Column("is_phone_verified")]
        public bool IsPhoneVerified { get; set; } = false;
        
        [Column("preferences", TypeName = "jsonb")]
        public string? Preferences { get; set; } // JSON string for Map<String, dynamic>
        
        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Google Authentication fields
        [Column("google_id")]
        [StringLength(255)]
        public string? GoogleId { get; set; }
        
        [Column("google_picture")]
        [StringLength(500)]
        public string? GooglePicture { get; set; }
        
        [Column("auth_provider")]
        [StringLength(10)]
        public string? AuthProvider { get; set; } = "Email"; // "Email" or "Google"
        
        // OTP fields
        [Column("otp_code")]
        [StringLength(6)]
        public string? OtpCode { get; set; }

        [Column("otp_expiry_time")]
        public DateTime? OtpExpiryTime { get; set; }

        [Column("otp_attempts")]
        public int OtpAttempts { get; set; } = 0;

        [Column("last_otp_request_time")]
        public DateTime? LastOtpRequestTime { get; set; }
        
        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<CashbackTransaction> CashbackTransactions { get; set; } = new List<CashbackTransaction>();
        public virtual ICollection<LoyaltyPoints> LoyaltyPointsHistory { get; set; } = new List<LoyaltyPoints>();
        public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual Settings? Settings { get; set; }
    }
} 