namespace Client_App.DTOs.Identities.Requests;

public record LoginFormModel(string Email, string Password)
{
    public string Email { get; set; } = Email;
    public string Password { get; set; } = Password;
}
