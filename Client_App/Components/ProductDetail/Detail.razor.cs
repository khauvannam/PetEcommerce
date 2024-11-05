using Client_App.DTOs.Products.Responses;
using Microsoft.AspNetCore.Components;

namespace Client_App.Components.ProductDetail;

public partial class Detail : ComponentBase
{
    private readonly Dictionary<string, bool> _isVisible =
        new()
        {
            { nameof(ProductById.Description), true },
            { nameof(ProductById.ProductUseGuide), false },
        };

    [Parameter]
    public ProductById ProductById { get; set; } = null!;

    private string VariantName { get; set; } = null!;
    private decimal Price { get; set; }
    private int Quantity { get; set; } = 1;

    private bool SelectInputVisible { get; set; } = false;

    protected override Task OnParametersSetAsync()
    {
        Price = ProductById.ProductVariants[0].OriginalPrice.Value;
        VariantName = ProductById.ProductVariants[0].VariantName;
        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        Service.OnChangeQuantity += HandleQuantityChange;
        Service.Quantity = Quantity;
        Service.VariantName = VariantName;
    }

    private void HandleQuantityChange(int newQuantity)
    {
        Quantity = newQuantity;
        StateHasChanged();
    }

    private void OnDropdownClicked(string variantName)
    {
        VariantName = variantName;
        var productVariant = ProductById.ProductVariants.FirstOrDefault(p =>
            p.VariantName == variantName
        );

        SelectInputVisible = false;

        Price = productVariant!.OriginalPrice.Value;
    }

    private void ToggleSelectInput()
    {
        SelectInputVisible = !SelectInputVisible;
    }

    private string CalculateDiscountPrice(ProductById productById, decimal price)
    {
        return (price * productById.DiscountPercent / 100).ToString("F2");
    }

    private void AdjustQuantity(char behavior)
    {
        Service.Quantity = behavior switch
        {
            '+' => Math.Min(Service.Quantity + 1, ProductById.TotalQuantity),
            '-' => Math.Max(Service.Quantity - 1, 1),
            _ => Service.Quantity,
        };
    }

    private void ToggleSection(string section)
    {
        if (_isVisible.ContainsKey(section))
            _isVisible[section] = !_isVisible[section];
    }
}
