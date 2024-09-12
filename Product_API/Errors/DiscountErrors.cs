using BaseDomain.Results;

namespace Product_API.Errors;

public static class DiscountErrors
{
    public static ErrorType NotFound()
    {
        return new ErrorType(".NotFound", "Your ProductId Not Found, Try contact with supporter");
    }

    public static ErrorType CreateForNothing()
    {
        return new ErrorType("Discount.CreateForNothing", "You cannot create Discount for nothing");
    }

    public static ErrorType CurrentlyActive()
    {
        return new ErrorType("Discount.CurrentlyActive", "You cannot create the current discount ");
    }
}
