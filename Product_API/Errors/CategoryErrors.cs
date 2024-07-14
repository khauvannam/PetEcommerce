using BaseDomain.Results;

namespace Product_API.Errors;

public static class CategoryErrors
{
    public static ErrorType NotFound =>
        new("Category.NotFound", "Your CategoryId Not Found, Try contact with supporter");
}
