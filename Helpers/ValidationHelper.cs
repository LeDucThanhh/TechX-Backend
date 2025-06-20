using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TechX.API.Helpers
{
    // Custom Model Validation Filter
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, List<string>>();
                
                foreach (var modelError in context.ModelState)
                {
                    var fieldName = modelError.Key;
                    var fieldErrors = new List<string>();
                    
                    foreach (var error in modelError.Value.Errors)
                    {
                        fieldErrors.Add(error.ErrorMessage);
                    }
                    
                    if (fieldErrors.Any())
                    {
                        errors[fieldName] = fieldErrors;
                    }
                }

                var response = new
                {
                    success = false,
                    message = "Validation failed",
                    errors = errors
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }

    // Custom Validation Attributes
    public class VietnamesePhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true; // Optional fields

            var phone = value.ToString();
            return System.Text.RegularExpressions.Regex.IsMatch(phone!, @"^(\+84|84|0)[3-9][0-9]{8}$");
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid Vietnamese phone number";
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a future date";
        }
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (value is DateTime birthDate)
            {
                var age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                    age--;
                
                return age >= _minimumAge;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"User must be at least {_minimumAge} years old";
        }
    }

    public class MaximumAgeAttribute : ValidationAttribute
    {
        private readonly int _maximumAge;

        public MaximumAgeAttribute(int maximumAge)
        {
            _maximumAge = maximumAge;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (value is DateTime birthDate)
            {
                var age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                    age--;
                
                return age <= _maximumAge;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Age cannot exceed {_maximumAge} years";
        }
    }

    public class NotPastDateAttribute : ValidationAttribute
    {
        private readonly int _allowedPastDays;

        public NotPastDateAttribute(int allowedPastDays = 0)
        {
            _allowedPastDays = allowedPastDays;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (value is DateTime dateTime)
            {
                return dateTime >= DateTime.Now.AddDays(-_allowedPastDays);
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return _allowedPastDays == 0 
                ? $"{name} cannot be in the past"
                : $"{name} cannot be more than {_allowedPastDays} days in the past";
        }
    }

    public class CurrencyAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (decimal.TryParse(value.ToString(), out decimal amount))
            {
                // Check for maximum 2 decimal places
                var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];
                return decimalPlaces <= 2 && amount >= 0;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid currency amount with maximum 2 decimal places";
        }
    }

    public class PercentageAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            
            if (decimal.TryParse(value.ToString(), out decimal percentage))
            {
                return percentage >= 0 && percentage <= 100;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between 0 and 100";
        }
    }

    // Validation Helper Methods
    public static class ValidationHelper
    {
        public static Dictionary<string, List<string>> GetValidationErrors(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, List<string>>();
            
            foreach (var kvp in modelState)
            {
                if (kvp.Value?.Errors.Count > 0)
                {
                    errors[kvp.Key] = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList();
                }
            }
            
            return errors;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
} 