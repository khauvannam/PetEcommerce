using BasedDomain.Results;

namespace Order.API.Errors;

public static class OrderErrors
{
    public static ErrorType NotFound => new("Order.NotFound", "Order not found.");
}
