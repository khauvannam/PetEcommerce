using BasedDomain.Results;

namespace Identity.API.Errors;

public static class TokenErrors
{
    public static ErrorType WrongToken(string tokenType)
    {
        return new ErrorType("Wrong Token", $"Your {tokenType} token is wrong.");
    }

    public static ErrorType ExpiredToken()
    {
        return new ErrorType("Expired Token", "Your Token has expired");
    }
}
