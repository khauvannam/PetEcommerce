namespace Client_App.Domains.Share;

public static class Errors
{
    public static readonly ErrorType None = new(string.Empty, string.Empty);
}

public record ErrorType(string Code, string Description);
