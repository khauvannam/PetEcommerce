using System.ComponentModel.DataAnnotations;
using Client_App.Helpers.CustomValidationAttribute;

namespace Client_App.DTOs.Identities.Requests;

public class RegisterFormModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Your email isn't accepted")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [NoWhitespaceOrSpecialCharacters(
        ErrorMessage = "Username cannot contain whitespace or special characters."
    )]
    public string? Username { get; set; }

    [PasswordValidate]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(
        @"^0[3-9]\d{8}$",
        ErrorMessage = "Invalid phone number format. The number must start with '8' followed by 10 digits, starting with 3, 4, 5, 6, 7, 8, or 9."
    )]
    public string? PhoneNumber { get; set; }

    public static RegisterFormModel Empty =>
        new()
        {
            Email = default,
            Username = default,
            Password = default,
            PhoneNumber = default,
        };
}
