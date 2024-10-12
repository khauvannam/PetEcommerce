using Client_App.Components.Home;
using Client_App.Domains.Categories.Responses;
using Client_App.Domains.Comments.Responses;
using Client_App.Domains.Products.Responses;
using Client_App.Interfaces;
using Client_App.Services;

namespace Client_App.Extensions;

public static class Extension
{
    public static void AddHttpClientAddress(this IServiceCollection services)
    {
        services.AddHttpClient(
            nameof(ProductService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5081");
            }
        );

        services.AddHttpClient(
            nameof(BasketService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
            }
        );
    }

    public static void AddFetchApiService(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICommentService, CommentService>();
    }
}
