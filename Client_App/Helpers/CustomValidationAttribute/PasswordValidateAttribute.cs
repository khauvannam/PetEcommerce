using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Client_App.Helpers.CustomValidationAttribute;

public partial class PasswordValidateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrEmpty(password))
            return new ValidationResult("Password is required");

        if (password.Length is < 8 or > 15)
            return new ValidationResult("Password must be between 8 and 15 characters");

        var regex = MyRegex();

        if (!regex.IsMatch(password))
            return new ValidationResult(
                "Password must contain at least one uppercase letter and one special character."
            );

        return ValidationResult.Success;
    }

    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*[\W_]).+$")]
    private static partial Regex MyRegex();
}
