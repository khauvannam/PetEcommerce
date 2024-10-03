using BasedDomain.Results;

namespace Product_API.Errors;

public static class ProductErrors
{
    public static ErrorType NotFound =>
        new("Products.NotFound", "Your ProductId Not Found, Try contact with supporter");

    public static ErrorType DuplicateName =>
        new("Products.DuplicateName", "Your ProductName already exists");
}
