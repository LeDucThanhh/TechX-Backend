using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class StoreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? OperatingHours { get; set; }
        public decimal CashbackRate { get; set; }
        public decimal PointsRate { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public decimal? Distance { get; set; }
        public bool IsPartner { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateStoreDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Store name must be between 2 and 255 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_'.]+$", ErrorMessage = "Store name contains invalid characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Logo must be a valid URL")]
        [StringLength(500, ErrorMessage = "Logo URL cannot exceed 500 characters")]
        public string? Logo { get; set; }

        [Url(ErrorMessage = "Banner must be a valid URL")]
        [StringLength(500, ErrorMessage = "Banner URL cannot exceed 500 characters")]
        public string? Banner { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees")]
        public decimal? Longitude { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8,9}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }

        [Url(ErrorMessage = "Website must be a valid URL")]
        [StringLength(500, ErrorMessage = "Website URL cannot exceed 500 characters")]
        public string? Website { get; set; }

        [StringLength(2000, ErrorMessage = "Operating hours cannot exceed 2000 characters")]
        public string? OperatingHours { get; set; }

        [Required(ErrorMessage = "Cashback rate is required")]
        [Range(0, 50, ErrorMessage = "Cashback rate must be between 0% and 50%")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback rate can have maximum 2 decimal places")]
        public decimal CashbackRate { get; set; } = 0;

        [Required(ErrorMessage = "Points rate is required")]
        [Range(0, 100, ErrorMessage = "Points rate must be between 0 and 100")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points rate can have maximum 2 decimal places")]
        public decimal PointsRate { get; set; } = 0;

        public bool IsPartner { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Coordinate validation - both or neither
            if ((Latitude.HasValue && !Longitude.HasValue) || (!Latitude.HasValue && Longitude.HasValue))
            {
                yield return new ValidationResult("Both latitude and longitude must be provided together", 
                    new[] { nameof(Latitude), nameof(Longitude) });
            }

            // Vietnam geographic bounds validation
            if (Latitude.HasValue && Longitude.HasValue)
            {
                // Vietnam approximate bounds
                if (Latitude.Value < 8.0m || Latitude.Value > 24.0m)
                {
                    yield return new ValidationResult("Latitude is outside Vietnam geographic bounds", new[] { nameof(Latitude) });
                }
                
                if (Longitude.Value < 102.0m || Longitude.Value > 110.0m)
                {
                    yield return new ValidationResult("Longitude is outside Vietnam geographic bounds", new[] { nameof(Longitude) });
                }
            }

            // Website validation
            if (!string.IsNullOrEmpty(Website) && !Website.StartsWith("http://") && !Website.StartsWith("https://"))
            {
                yield return new ValidationResult("Website URL must start with http:// or https://", new[] { nameof(Website) });
            }
        }
    }

    public class UpdateStoreDTO : IValidatableObject
    {
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Store name must be between 2 and 255 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_'.]+$", ErrorMessage = "Store name contains invalid characters")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Logo must be a valid URL")]
        [StringLength(500, ErrorMessage = "Logo URL cannot exceed 500 characters")]
        public string? Logo { get; set; }

        [Url(ErrorMessage = "Banner must be a valid URL")]
        [StringLength(500, ErrorMessage = "Banner URL cannot exceed 500 characters")]
        public string? Banner { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 500 characters")]
        public string? Address { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees")]
        public decimal? Longitude { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8,9}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }

        [Url(ErrorMessage = "Website must be a valid URL")]
        [StringLength(500, ErrorMessage = "Website URL cannot exceed 500 characters")]
        public string? Website { get; set; }

        [StringLength(2000, ErrorMessage = "Operating hours cannot exceed 2000 characters")]
        public string? OperatingHours { get; set; }

        [Range(0, 50, ErrorMessage = "Cashback rate must be between 0% and 50%")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback rate can have maximum 2 decimal places")]
        public decimal? CashbackRate { get; set; }

        [Range(0, 100, ErrorMessage = "Points rate must be between 0 and 100")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points rate can have maximum 2 decimal places")]
        public decimal? PointsRate { get; set; }

        public bool? IsPartner { get; set; }

        public bool? IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Vietnam geographic bounds validation
            if (Latitude.HasValue)
            {
                if (Latitude.Value < 8.0m || Latitude.Value > 24.0m)
                {
                    yield return new ValidationResult("Latitude is outside Vietnam geographic bounds", new[] { nameof(Latitude) });
                }
            }
            
            if (Longitude.HasValue)
            {
                if (Longitude.Value < 102.0m || Longitude.Value > 110.0m)
                {
                    yield return new ValidationResult("Longitude is outside Vietnam geographic bounds", new[] { nameof(Longitude) });
                }
            }

            // Website validation
            if (!string.IsNullOrEmpty(Website) && !Website.StartsWith("http://") && !Website.StartsWith("https://"))
            {
                yield return new ValidationResult("Website URL must start with http:// or https://", new[] { nameof(Website) });
            }
        }
    }
} 
    {
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Store name must be between 2 and 255 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_'.]+$", ErrorMessage = "Store name contains invalid characters")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Logo must be a valid URL")]
        [StringLength(500, ErrorMessage = "Logo URL cannot exceed 500 characters")]
        public string? Logo { get; set; }

        [Url(ErrorMessage = "Banner must be a valid URL")]
        [StringLength(500, ErrorMessage = "Banner URL cannot exceed 500 characters")]
        public string? Banner { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 500 characters")]
        public string? Address { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees")]
        public decimal? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees")]
        public decimal? Longitude { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+84|84|0)[3-9][0-9]{8,9}$", ErrorMessage = "Invalid Vietnamese phone number format")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }

        [Url(ErrorMessage = "Website must be a valid URL")]
        [StringLength(500, ErrorMessage = "Website URL cannot exceed 500 characters")]
        public string? Website { get; set; }

        [StringLength(2000, ErrorMessage = "Operating hours cannot exceed 2000 characters")]
        public string? OperatingHours { get; set; }

        [Range(0, 50, ErrorMessage = "Cashback rate must be between 0% and 50%")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback rate can have maximum 2 decimal places")]
        public decimal? CashbackRate { get; set; }

        [Range(0, 100, ErrorMessage = "Points rate must be between 0 and 100")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points rate can have maximum 2 decimal places")]
        public decimal? PointsRate { get; set; }

        public bool? IsPartner { get; set; }

        public bool? IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Vietnam geographic bounds validation
            if (Latitude.HasValue)
            {
                if (Latitude.Value < 8.0m || Latitude.Value > 24.0m)
                {
                    yield return new ValidationResult("Latitude is outside Vietnam geographic bounds", new[] { nameof(Latitude) });
                }
            }
            
            if (Longitude.HasValue)
            {
                if (Longitude.Value < 102.0m || Longitude.Value > 110.0m)
                {
                    yield return new ValidationResult("Longitude is outside Vietnam geographic bounds", new[] { nameof(Longitude) });
                }
            }

            // Website validation
            if (!string.IsNullOrEmpty(Website) && !Website.StartsWith("http://") && !Website.StartsWith("https://"))
            {
                yield return new ValidationResult("Website URL must start with http:// or https://", new[] { nameof(Website) });
            }
        }
    }
} 