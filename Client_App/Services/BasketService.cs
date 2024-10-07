using Client_App.Abstraction;

namespace Client_App.Services;

public class BasketService(IHttpClientFactory factory, string baseUrl, string endpoint)
    : ApiService(factory, baseUrl, endpoint);
