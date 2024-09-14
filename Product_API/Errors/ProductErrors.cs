using BaseDomain.Results;

namespace Product_API.Errors;

public static class ProductErrors
{
    public static ErrorType NotFound =>
        new("Product.NotFound", "Your ProductId Not Found, Try contact with supporter");

    public static ErrorType DuplicateName =>
        new("Product.DuplicateName", "Your ProductName already exists");
}
