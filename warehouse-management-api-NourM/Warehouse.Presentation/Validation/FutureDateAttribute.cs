using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Validation;

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context) {
            if (value is DateTime date && date <= DateTime.Today)
            {
                return new ValidationResult("Date must be in the future");
            }

            return ValidationResult.Success;
        }
        
    }