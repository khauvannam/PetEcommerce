using MediatR;

namespace Product_API.Events.DiscountEvents;

public class CreateDiscountEvent : INotification
{
    private CreateDiscountEvent() { }

    public decimal Percent { get; private set; }
    public List<Guid> ProductIdList { get; private set; } = [];
    public Guid? CategoryId { get; private set; }
    public int Priority { get; private set; }

    public static CreateDiscountEvent Create(
        decimal percent,
        Guid? categoryId,
        List<Guid> productIdList,
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
