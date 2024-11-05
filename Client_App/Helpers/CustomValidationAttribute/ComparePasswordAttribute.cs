using System.ComponentModel.DataAnnotations;

namespace Client_App.Helpers.CustomValidationAttribute;

public class ComparePasswordAttribute(string password) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var currentValue = value as string;
        var property = validationContext.ObjectType.GetProperty(password);

        if (property is null)
            return new ValidationResult($"Property {password} not found.");

        var compareValue = property.GetValue(validationContext.ObjectInstance, null) as string;

        if (currentValue != compareValue)
            return new ValidationResult($"Property {password} does not match.");

        return ValidationResult.Success!;
    }
}
