using BaseDomain.Results;

namespace Identity.API.Errors;

public static class TokenErrors
{
    public static ErrorType WrongToken(string tokenType) =>
        new("Wrong Token", $"Your {tokenType} token is wrong.");

    public static ErrorType ExpiredToken() => new("Expired Token", "Your Token has expired");
}
