namespace Identity.API.Common;

public static class ValidatorMessage
{
    public static string NotEmpty(string type) => $"You have to fill your {type}";

    public static string MustBeUnique(string type) => $"Your {type} must be unique";
}
