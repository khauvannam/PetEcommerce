namespace Client_App.DTOs.Share;

public static class Errors
{
    public static readonly ErrorType None = new(string.Empty, string.Empty);
}

public record ErrorType(string Code, string Description);
