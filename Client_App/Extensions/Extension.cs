using Blazored.LocalStorage;
using Client_App.Helpers.Jwt;
using Client_App.Interfaces;
using Client_App.Services.APIs;
using Client_App.Services.Clients;
using Client_App.Services.Share;
using Client_App.Services.States;

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

        services.AddHttpClient(
            nameof(IdentityService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5175/");
            }
        );
    }

    public static void AddFetchApiService(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ITokenService, TokenService>();

        // Modal service
        services.AddScoped<ErrorModalService>();
        services.AddScoped<TitleService>();
        services.AddScoped<ToggleSearchInputService>();
        services.AddScoped<ProductDetailModalService>();
        services.AddScoped<AuthStateService>();

        services.AddTransient<CustomValidatorService>();
        services.AddTransient<JwtHelper>();
    }

    public static void AddPersistenceService(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
    }
}
