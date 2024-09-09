namespace Identity.API.Helpers;

public static class ValidatorMessage
{
    public static string NotEmpty(string type)
    {
        return $"You have to fill your {type}";
    }

    public static string MustBeUnique(string type)
    {
        return $"Your {type} must be unique";
    }
}
