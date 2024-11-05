using Client_App.DTOs.Categories.Responses;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Client_App.Pages;

public partial class Home : ComponentBase
{
    public List<Category> Categories = [];
    public Pagination<ProductsInList>? Products { get; set; }

    [Inject]
    public ICategoryService CategoryService { get; set; } = null!;

    [Inject]
    public IProductService ProductService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await TitleService.SetTitleAsync("Adopt love, adopt a pet");

        var productResult = await ProductService.GetProductsByConditionAsync<ProductsInList>(
            0,
            default,
            true,
            8
        );

        if (!productResult.IsFailure)
        {
            Products = productResult.Value;
        }
        else
        {
            ErrorModalService.SetErrorMessage(productResult.ErrorTypes).NavigateTo();
            return;
        }

        var categoryResult = await CategoryService.GetAllAsync<Category>(4);

        if (!categoryResult.IsFailure)
            Categories = categoryResult.Value.Data;
        else
            ErrorModalService.SetErrorMessage(categoryResult.ErrorTypes).NavigateTo();

        StateHasChanged();
    }
}
