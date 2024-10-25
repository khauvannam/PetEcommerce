using Client_App.Abstraction;
using Client_App.DTOs.Baskets.Responses;

namespace Client_App.Services.APIs;

public class BasketService(IHttpClientFactory factory, string baseUrl, string endpoint)
    : ApiService(factory, baseUrl, endpoint)
{
    private Basket? Basket { get; set; }
}
