using Base.Results;

namespace Order.API.Errors;

public static class ShippingMethodErrors
{
    public static ErrorType NotFound => new("ShippingMethod.NotFound", "Shipping Method Not Found");
}
