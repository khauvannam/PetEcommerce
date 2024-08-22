using BaseDomain.Results;

namespace Identity.API.Errors;

public static class UserErrors
{
    public static ErrorType WentWrong => new("Went Wrong", "Something went wrong, try again");
    public static ErrorType NotFound => new("Not Found", "User not found");
}
