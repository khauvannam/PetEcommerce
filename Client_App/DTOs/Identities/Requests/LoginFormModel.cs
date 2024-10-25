using System.ComponentModel.DataAnnotations;
using Client_App.Helpers.CustomValidationAttribute;

namespace Client_App.DTOs.Identities.Requests;

public record LoginFormModel(string? Email, string? Password)
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Your email isn't accepted")]
    public string? Email { get; set; } = Email;

    [PasswordValidate]
    public string? Password { get; set; } = Password;

    public static LoginFormModel Empty => new(default, default);
}
