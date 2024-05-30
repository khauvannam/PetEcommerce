using Shared.Domain.Results;

namespace Identity.API.Errors;

internal static class UserErrors
{
    public static ErrorType WentWrong => new("Went Wrong", "Something went wrong, try again");
    public static ErrorType NotFound => new("Not Found", "User not found");
}
