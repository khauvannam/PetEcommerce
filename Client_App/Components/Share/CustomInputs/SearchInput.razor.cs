using Client_App.DTOs.Products.Requests;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Microsoft.AspNetCore.Components;

namespace Client_App.Components.Share.CustomInputs;

public partial class SearchInput : ComponentBase
{
    private ElementReference _searchInputRef; // ElementReference for the input field
    private ProductsSearchFilterRequest Request { get; set; } = new();
    private bool IsLoadingHidden { get; set; } = true;
    private Pagination<ProductsInList>? Pagination { get; set; }

    public void Dispose()
    {
        SearchInputService.SetIsHidden(true);
        Request.SearchText = string.Empty;
        SearchInputService.UnsubscribeEvents(StateHasChanged);
    }

    private async Task SearchProductAsync(string? value)
    {
        Request.SearchText = value;
        Request.Limit = 4;

        if (Request.SearchText!.Length > 2)
        {
            Pagination = new Pagination<ProductsInList>(default!, default);

            IsLoadingHidden = false;

            await Task.Delay(4000);

            var result = await ProductService.GetProductsBySearch<ProductsInList>(Request);

            IsLoadingHidden = true;

            if (result.IsFailure)
            {
                ErrorModalService.SetErrorMessage(result.ErrorTypes).NavigateTo(default);
                return;
            }

            Pagination = result.Value;
        }
    }

    protected override Task OnInitializedAsync()
    {
        SearchInputService.SubscribeEvents(StateHasChanged);
        return Task.CompletedTask;
    }

    private void OnCloseAsync()
    {
        SearchInputService.SetIsHidden(true);
        Request.SearchText = string.Empty;
    }

    private void PreventFormSubmit()
    {
        if (Pagination!.TotalItems <= 0)
            return;

        var uri = $"/search/{Request.SearchText}";

        NavigationManager.NavigateTo(uri, true);
    }
}
