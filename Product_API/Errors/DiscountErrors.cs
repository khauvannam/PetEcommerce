using BaseDomain.Results;

namespace Product_API.Errors;

public static class DiscountErrors
{
    public static ErrorType NotFound() =>
        new(".NotFound", "Your ProductId Not Found, Try contact with supporter");

    public static ErrorType CreateForNothing() =>
        new("Discount.CreateForNothing", "You cannot create Discount for nothing");
}
