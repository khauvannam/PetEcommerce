using MediatR;

namespace Product_API.Events.DiscountEvents;

public class CreateDiscountEvent : INotification
{
    private CreateDiscountEvent() { }

    public decimal Percent { get; private set; }
    public List<string> ProductIdList { get; private set; } = [];
    public string? CategoryId { get; private set; }

    public static CreateDiscountEvent Create(
        decimal percent,
        string? categoryId,
        List<string> productIdList
    )
    {
        if (categoryId is null && productIdList.Count < 0)
            throw new ArgumentException("At least one product id is required");
        return new CreateDiscountEvent
        {
            Percent = percent,
            CategoryId = categoryId,
            ProductIdList = productIdList,
        };
    }
}
