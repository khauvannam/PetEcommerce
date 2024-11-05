using Client_App.DTOs.Comments.Responses;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client_App.Pages.Product;

public partial class ProductDetail : ComponentBase
{
    private IJSObjectReference? _jsObject;

    [Parameter]
    public int ProductId { get; set; }

    private ProductById? ProductById { get; set; }
    private Pagination<Comment>? Pagination { get; set; }
    private static int Limit => 8;
    private int Offset { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (_jsObject != null)
            await _jsObject.DisposeAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        _jsObject = await Js.InvokeAsync<IJSObjectReference>(
            "import",
            "./js/intersectionObserver.js"
        );
        await _jsObject.InvokeVoidAsync(
            "observeElement",
            "observeTarget",
            DotNetObjectReference.Create(this)
        );
    }

    protected override async Task OnParametersSetAsync()
    {
        Offset = 0;
        var productResult = await ProductService.GetByIdAsync<ProductById>(ProductId);
        if (productResult.IsFailure)
        {
            ErrorModalService.SetErrorMessage(productResult.ErrorTypes).NavigateTo();
            await TitleService.SetTitleErrorAsync();
            return;
        }

        ProductById = productResult.Value;

        await TitleService.SetTitleAsync(ProductById.Name);

        var commentResult = await CommentService.GetAllByProductIdAsync<Comment>(
            Limit,
            Offset * Limit,
            ProductId
        );

        if (commentResult.IsFailure)
        {
            ErrorModalService.SetErrorMessage(commentResult.ErrorTypes).NavigateTo();
            await TitleService.SetTitleErrorAsync();
            return;
        }

        Pagination = commentResult.Value;
    }

    [JSInvokable]
    public void OnIntersectionChanged()
    {
        Service.IsHidden = !Service.IsHidden;
    }
}
