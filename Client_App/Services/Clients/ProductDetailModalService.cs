using Client_App.DTOs.Products.Responses;
using Microsoft.AspNetCore.Components;

namespace Client_App.Services.Clients;

public class ProductDetailModalService
{
    private bool _isHidden = true;

    private int _quantity;

    private string _variantName = string.Empty;

    public List<ProductVariant> Variants { get; set; } = null!;

    public EventCallback PerformApiAsync { get; set; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnChangeQuantity.Invoke(_quantity);
        }
    }

    public string VariantName
    {
        get => _variantName;
        set
        {
            _variantName = value;
            OnChangeVariant.Invoke(_variantName);
        }
    }

    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            _isHidden = value;
            OnObserved.Invoke();
        }
    }

    public event Action OnObserved = null!;

    public event Action<int> OnChangeQuantity = null!;

    public event Action<string> OnChangeVariant = null!;
}
