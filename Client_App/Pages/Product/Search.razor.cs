using Client_App.DTOs.Products.Requests;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client_App.Pages.Product;

public partial class Search : ComponentBase
{
    [Parameter]
    public string SearchText { get; set; } = null!;

    private Pagination<ProductsInList> Pagination { get; set; } = new(default!, default);
    private ProductsSearchFilterRequest Request { get; set; } = new();
    private int TotalPages { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Request.SearchText = SearchText;
        await FetchDataAsync();

        TotalPages = (int)Math.Ceiling((double)Pagination.TotalItems / Request.Limit);
    }

    private async void OnSearchTextChange()
    {
        NavigationManager.NavigateTo($"/search/{SearchText}");
        Request.SearchText = SearchText;
        await FetchDataAsync();
        StateHasChanged();
    }

    private async Task OnPageChangeAsync(int newPage)
    {
        Request.Offset = (newPage - 1) * Request.Limit;

        await FetchDataAsync();

        await JsRuntime.InvokeVoidAsync("scrollToTop", "search-products"); // Scroll to the top of the product list
    }

    private async Task FetchDataAsync()
    {
        var result = await ProductService.GetProductsBySearch<ProductsInList>(Request);

        if (result.IsFailure)
        {
            ErrorModalService.SetErrorMessage(result.ErrorTypes);
            return;
        }

        Pagination = result.Value;

        TotalPages = (int)Math.Ceiling((double)Pagination.TotalItems / Request.Limit);
    }
}
