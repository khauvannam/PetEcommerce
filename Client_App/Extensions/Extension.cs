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
                client.BaseAddress = new Uri("http://localhost:8082/");
            }
        );

        services.AddHttpClient(
            nameof(BasketService),
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:8080/api/basket");
            }
        );
        services.AddScoped<CategoryService>();
    }
}
