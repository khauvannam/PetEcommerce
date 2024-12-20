using MediatR;

namespace Product_API.Events.DiscountEvents;

public class CreateDiscountEvent : INotification
{
    private CreateDiscountEvent() { }

    public decimal Percent { get; private set; }
    public List<int> ProductIdList { get; private set; } = [];
    public int? CategoryId { get; private set; }
    public int Priority { get; private set; }

    public static CreateDiscountEvent Create(
        decimal percent,
        int? categoryId,
        List<int> productIdList,
        int priority
    )
    {
        if (categoryId is null && productIdList.Count < 0)
            throw new ArgumentException("At least one product id is required");
        return new CreateDiscountEvent
        {
            Percent = percent,
            CategoryId = categoryId,
            ProductIdList = productIdList,
            Priority = priority,
        };
    }
}
