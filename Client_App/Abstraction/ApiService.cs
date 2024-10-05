using System.Text.Json;
using BasedDomain.Results;
using Client_App.Interfaces;
using Exception = System.Exception;

namespace Client_App.Abstraction;

public abstract class ApiService<TResponse, TGetByIdResponse>(
    IHttpClientFactory factory,
    string baseUrl,
    string endpoint
) : ApiService(factory, baseUrl, endpoint), IApiService<TResponse, TGetByIdResponse>
    where TResponse : class
    where TGetByIdResponse : class
{
    public virtual async Task<List<TResponse>> GetAllAsync(int? limit = null, int? offset = null)
    {
        var query =
            limit.HasValue && offset.HasValue ? $"?limit={limit}&offset={offset}" : string.Empty;

        var result = await Client.GetAsync($"{Endpoint}{query}");
        return await HandleResponse<List<TResponse>>(result) ?? [];
    }

    public virtual async Task<TGetByIdResponse> GetByIdAsync(Guid id)
    {
        var result = await Client.GetAsync($"{Endpoint}/{id}");
        return (await HandleResponse<TGetByIdResponse>(result))!;
    }

    public virtual async Task CreateAsync(object item)
    {
        var result = await Client.PostAsync(
            Endpoint,
            new StringContent(JsonSerializer.Serialize(item, Options))
        );

        await HandleResponse(result);
    }

    public virtual async Task UpdateAsync(object item, string id)
    {
        var result = await Client.PutAsync(
            $"{Endpoint}/{id}",
            new StringContent(JsonSerializer.Serialize(item, Options))
        );

        await HandleResponse(result);
    }

    public virtual async Task DeleteAsync(string id)
    {
        var result = await Client.DeleteAsync($"{Endpoint}/{id}");
        await HandleResponse(result);
    }

    protected async Task HandleResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        CheckErrors(content);
    }

    protected async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        where T : class
    {
        var content = await response.Content.ReadAsStringAsync();
        var deserialized = JsonSerializer.Deserialize<T>(content, Options);
        return CheckErrors(deserialized);
    }

    private static T CheckErrors<T>(T item)
    {
        if (item is ErrorType type)
            throw new Exception($"Error: {type}");

        return item;
    }
}

public abstract class ApiService
{
    protected HttpClient Client { get; }
    private IHttpClientFactory Factory { get; }
    protected string Endpoint { get; }
    protected JsonSerializerOptions Options { get; } = new() { PropertyNameCaseInsensitive = true };

    protected ApiService(IHttpClientFactory factory, string baseUrl, string endpoint)
    {
        Factory = factory;
        Endpoint = endpoint;
        Client = Factory.CreateClient(baseUrl);
    }
}
