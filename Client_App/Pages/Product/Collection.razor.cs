using Client_App.DTOs.Categories.Responses;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client_App.Pages.Product;

public partial class Collection : ComponentBase
{
    [Parameter]
    public string? CategoryEndpoint { get; set; } = string.Empty;

    private Category? Category { get; set; }
    public Pagination<ProductsInList>? ProductsPagination { get; set; }
    public Pagination<Category>? CategoriesPagination { get; set; }

    private static int Limit => 10;
    private int TotalPages { get; set; }

    [Inject]
    public required IProductService ProductService { get; set; }

    [Inject]
    public required ICategoryService CategoryService { get; set; }

    protected override void OnInitialized()
    {
        if (string.IsNullOrEmpty(CategoryEndpoint))
            NavigationManager.NavigateTo("/products/all", true); // Force reload
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadDataAsync();
        TotalPages = (int)Math.Ceiling((double)ProductsPagination!.TotalItems / Limit);
    }

    private async Task LoadDataAsync()
    {
        var categoryResult = await CategoryService.GetByEndpointAsync<Category>(CategoryEndpoint!);

        if (categoryResult.IsFailure)
        {
            ErrorModalService.SetErrorMessage(categoryResult.ErrorTypes).NavigateTo();
            await TitleService.SetTitleErrorAsync();
            return;
        }

        Category = categoryResult.Value;

        await TitleService.SetTitleAsync(Category.CategoryName!);

        var categoryPaginationResult = await CategoryService.GetAllAsync<Category>();

        if (categoryPaginationResult.IsFailure)
        {
            ErrorModalService.SetErrorMessage(categoryPaginationResult.ErrorTypes);
            return;
        }

        CategoriesPagination = categoryPaginationResult.Value;

        await FetchProductAsync(default);

        StateHasChanged();
    }

    private async Task FetchProductAsync(int offset)
    {
        var result = CategoryEndpoint switch
        {
            "all" => await ProductService.GetProductsByConditionAsync<ProductsInList>(
                offset * Limit,
                default,
                false,
                Limit
            ),
            "best-sellers" => await ProductService.GetProductsByConditionAsync<ProductsInList>(
                offset * Limit,
                default,
                true,
                Limit
            ),
            _ => await ProductService.GetProductsByConditionAsync<ProductsInList>(
                offset * Limit,
                Category!.CategoryId,
                false,
                Limit
            ),
        };

        if (result.IsFailure)
        {
            ErrorModalService.SetErrorMessage(result.ErrorTypes).NavigateTo();
            return;
        }

        ProductsPagination = result.Value;
    }

    private async Task OnPageClick(int newPage)
    {
        await FetchProductAsync(newPage - 1);

        await JsRuntime.InvokeVoidAsync("scrollToTop", "list-products");
    }
}
