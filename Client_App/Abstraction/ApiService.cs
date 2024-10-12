using System.Text.Json;
using Client_App.Domains.Share;
using Client_App.Interfaces;

namespace Client_App.Abstraction;

public abstract class ApiService(IHttpClientFactory factory, string baseUrl, string endpoint)
    : IApiService
{
    protected HttpClient Client { get; } = factory.CreateClient(baseUrl);
    protected string Endpoint { get; } = endpoint;
    private JsonSerializerOptions Options { get; } = new() { PropertyNameCaseInsensitive = true };

    public virtual async Task<Pagination<T>> GetAllAsync<T>(int? limit = null, int? offset = null)
        where T : class
    {
        var query = CreateQuery(limit, offset);
        var response = await Client.GetAsync($"{Endpoint}{query}");
        return await HandleResponse<Pagination<T>>(response);
    }

    public virtual async Task<T> GetByIdAsync<T>(Guid id)
        where T : class
    {
        var response = await Client.GetAsync($"{Endpoint}/{id}");
        return await HandleResponse<T>(response);
    }

    public virtual async Task CreateAsync<TRequest>(TRequest item)
    {
        var response = await Client.PostAsync(Endpoint, SerializeItem(item!));
        await HandleResponse(response);
    }

    public virtual async Task UpdateAsync<TRequest>(TRequest item, string id)
    {
        var response = await Client.PutAsync($"{Endpoint}/{id}", SerializeItem(item!));
        await HandleResponse(response);
    }

    public virtual async Task DeleteAsync(string id)
    {
        var response = await Client.DeleteAsync($"{Endpoint}/{id}");
        await HandleResponse(response);
    }

    private static string CreateQuery(int? limit, int? offset)
    {
        return (limit.HasValue && offset.HasValue)
            ? $"?limit={limit}&offset={offset}"
            : string.Empty;
    }

    private StringContent SerializeItem(object item)
    {
        return new StringContent(JsonSerializer.Serialize(item, Options));
    }

    // Handle response without returning any value
    protected async Task HandleResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        CheckErrors(content);
    }

    // Handle response with returning a deserialized object
    protected async Task<TResult> HandleResponse<TResult>(HttpResponseMessage response)
        where TResult : class
    {
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResult>(content, Options)!;
        return CheckErrors(result);
    }

    // Error-checking logic
    private static TItem CheckErrors<TItem>(TItem item)
        where TItem : class
    {
        // Implement error-checking logic if necessary
        return item;
    }
}
