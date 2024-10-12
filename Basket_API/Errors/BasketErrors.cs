using Base.Results;

namespace Basket_API.Errors;

public static class BasketErrors
{
    public static ErrorType NotFound =>
        new("Basket.NotFound", "Your BasketId was not found. Please contact support.");
}

public static class BasketItemErrors
{
    public static ErrorType NotFound =>
        new("BasketItem.NotFound", "Your BasketId was not found. Please contact support.");
}
