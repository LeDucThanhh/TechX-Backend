using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string? Preferences { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string? GoogleId { get; set; }
        public string? GooglePicture { get; set; }
        public string? AuthProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
        public string LastName { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }
        
        [Url(ErrorMessage = "Profile picture must be a valid URL")]
        [StringLength(500, ErrorMessage = "Profile picture URL cannot exceed 500 characters")]
        public string? Avatar { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string? Gender { get; set; }
        
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }
        
        [StringLength(2000, ErrorMessage = "Preferences cannot exceed 2000 characters")]
        public string? Preferences { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue)
            {
                if (DateOfBirth.Value > DateTime.Now.AddYears(-13))
                {
                    yield return new ValidationResult("User must be at least 13 years old", new[] { nameof(DateOfBirth) });
                }
                
                if (DateOfBirth.Value < DateTime.Now.AddYears(-120))
                {
                    yield return new ValidationResult("Invalid birth date", new[] { nameof(DateOfBirth) });
                }
            }
        }
    }

    public class UpdateUserDTO : IValidatableObject
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
        public string? FirstName { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Profile picture must be a valid URL")]
        [StringLength(500, ErrorMessage = "Profile picture URL cannot exceed 500 characters")]
        public string? ProfilePicture { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string? Gender { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [StringLength(2000, ErrorMessage = "Preferences cannot exceed 2000 characters")]
        public string? Preferences { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue)
            {
                if (DateOfBirth.Value > DateTime.Now.AddYears(-13))
                {
                    yield return new ValidationResult("User must be at least 13 years old", new[] { nameof(DateOfBirth) });
                }
                
                if (DateOfBirth.Value < DateTime.Now.AddYears(-120))
                {
                    yield return new ValidationResult("Invalid birth date", new[] { nameof(DateOfBirth) });
                }
            }
        }
    }

    public class ChangePasswordRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Current password is required")]
        [StringLength(100, ErrorMessage = "Current password cannot exceed 100 characters")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "Password confirmation does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CurrentPassword == NewPassword)
            {
                yield return new ValidationResult("New password must be different from current password", new[] { nameof(NewPassword) });
            }
        }
    }
} 
    }

    public class UpdateUserDTO : IValidatableObject
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
        public string? FirstName { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Profile picture must be a valid URL")]
        [StringLength(500, ErrorMessage = "Profile picture URL cannot exceed 500 characters")]
        public string? ProfilePicture { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        [RegularExpression(@"^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string? Gender { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [StringLength(2000, ErrorMessage = "Preferences cannot exceed 2000 characters")]
        public string? Preferences { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue)
            {
                if (DateOfBirth.Value > DateTime.Now.AddYears(-13))
                {
                    yield return new ValidationResult("User must be at least 13 years old", new[] { nameof(DateOfBirth) });
                }
                
                if (DateOfBirth.Value < DateTime.Now.AddYears(-120))
                {
                    yield return new ValidationResult("Invalid birth date", new[] { nameof(DateOfBirth) });
                }
            }
        }
    }

    public class ChangePasswordRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Current password is required")]
        [StringLength(100, ErrorMessage = "Current password cannot exceed 100 characters")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "Password confirmation does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CurrentPassword == NewPassword)
            {
                yield return new ValidationResult("New password must be different from current password", new[] { nameof(NewPassword) });
            }
        }
    }
} 