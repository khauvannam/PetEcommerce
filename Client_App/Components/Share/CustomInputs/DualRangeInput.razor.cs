using Client_App.DTOs.Products.Requests;
using Microsoft.AspNetCore.Components;

namespace Client_App.Components.Share.CustomInputs;

public partial class DualRangeInput : ComponentBase
{
    private decimal Min { get; set; }
    private decimal Max { get; set; }

    [Parameter]
    public decimal LeftValue { get; set; }

    [Parameter]
    public decimal RightValue { get; set; } = 10000;

    [Parameter]
    public EventCallback<decimal> LeftValueChanged { get; set; }

    [Parameter]
    public EventCallback<decimal> RightValueChanged { get; set; }

    [Parameter]
    public EventCallback<ProductsSearchFilterRequest> OnPriceChanged { get; set; }

    protected override void OnInitialized()
    {
        Min = LeftValue;
        Max = RightValue;
    }

    private async void SetLeftValue(decimal value)
    {
        LeftValue = Math.Min(value, RightValue - 100);

        await LeftValueChanged.InvokeAsync(LeftValue);
        if (OnPriceChanged.HasDelegate)
            await OnPriceChanged.InvokeAsync();
    }

    private async void SetRightValue(decimal value)
    {
        RightValue = Math.Max(value, LeftValue + 100);

        await RightValueChanged.InvokeAsync(RightValue);

        if (OnPriceChanged.HasDelegate)
            await OnPriceChanged.InvokeAsync();
    }

    private string GetLeftThumbPosition()
    {
        return $"left: {(LeftValue - Min) / (Max - Min) * 100}%;";
    }

    private string GetRightThumbPosition()
    {
        return $"right: {100 - (RightValue - Min) / (Max - Min) * 100}%;";
    }

    private string GetRangePosition()
    {
        return $"left: {(LeftValue - Min) / (Max - Min) * 100}%; right: {100 - (RightValue - Min) / (Max - Min) * 100}%;";
    }
}
