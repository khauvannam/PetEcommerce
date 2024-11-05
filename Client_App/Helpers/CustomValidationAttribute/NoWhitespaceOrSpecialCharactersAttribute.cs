using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Client_App.Helpers.CustomValidationAttribute;

public partial class NoWhitespaceOrSpecialCharactersAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string str)
            return ValidationResult.Success!;

        if (string.IsNullOrWhiteSpace(str))
            return new ValidationResult("This field is required and cannot contain whitespace.");

        // Check for special characters
        var regex = MyRegex(); // Allow only alphanumeric characters
        return !regex.IsMatch(str)
            ? new ValidationResult("This field cannot contain special characters.")
            : ValidationResult.Success!;
    }

    [GeneratedRegex("^[a-zA-Z0-9]*$")]
    private static partial Regex MyRegex();
}
