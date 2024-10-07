using Client_App.Abstraction;

namespace Client_App.Services;

public class IdentityService(IHttpClientFactory factory, string baseUrl, string endpoint)
    : ApiService(factory, baseUrl, endpoint);
