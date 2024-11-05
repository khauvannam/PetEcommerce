using System.Text;
using System.Text.Json;
using Client_App.DTOs.Share;
using Client_App.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace Client_App.Abstraction;

public abstract class ApiService(IHttpClientFactory factory, string baseUrl, string endpoint)
    : BaseApiService(factory, baseUrl, endpoint),
        IApiService
{
    public virtual async Task<Result<Pagination<T>>> GetAllAsync<T>(
        int? limit = null,
        int? offset = null
    )
        where T : class
    {
        var query = CreateQuery(limit, offset);
        var response = await Client.GetAsync($"{Endpoint}{query}");
        return await HandleResponse<Pagination<T>>(response);
    }

    public virtual async Task<Result<T>> GetByIdAsync<T>(int id)
        where T : class
    {
        var response = await Client.GetAsync($"{Endpoint}/{id}");
        return await HandleResponse<T>(response);
    }

    public virtual async Task<Result> CreateAsync<TRequest>(TRequest item)
        where TRequest : class
    {
        HttpContent content = HasIBrowserFile(item)
            ? SerializeItemToMultipart(item)
            : SerializeItemToJson(item);
        var response = await Client.PostAsync(Endpoint, content);
        return await HandleResponse(response);
    }

    public virtual async Task<Result> UpdateAsync<TRequest>(TRequest item, int id)
        where TRequest : class
    {
        HttpContent content = HasIBrowserFile(item)
            ? SerializeItemToMultipart(item)
            : SerializeItemToJson(item);
        var response = await Client.PutAsync($"{Endpoint}/{id}", content);
        return await HandleResponse(response);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var response = await Client.DeleteAsync($"{Endpoint}/{id}");
        await HandleResponse(response);
    }

    private static string CreateQuery(int? limit, int? offset)
    {
        return limit.HasValue && offset.HasValue ? $"?limit={limit}&offset={offset}" : string.Empty;
    }
}

public abstract class BaseApiService(IHttpClientFactory factory, string baseUrl, string endpoint)
{
    protected HttpClient Client { get; } = factory.CreateClient(baseUrl);
    protected string Endpoint { get; } = endpoint;
    private JsonSerializerOptions Options { get; } = new() { PropertyNameCaseInsensitive = true };

    protected static StringContent SerializeItemToJson<TItem>(TItem item)
        where TItem : class
    {
        var content = new StringContent(
            JsonSerializer.Serialize(item),
            Encoding.UTF8,
            "application/json"
        );

        return content;
    }

    protected static MultipartFormDataContent SerializeItemToMultipart<TItem>(TItem item)
        where TItem : class
    {
        var content = new MultipartFormDataContent();

        // Assuming 'item' has properties that should be sent as fields
        foreach (var prop in item.GetType().GetProperties())
        {
            var value = prop.GetValue(item)?.ToString();
            if (!string.IsNullOrEmpty(value))
                content.Add(new StringContent(value), prop.Name);
        }

        return content;
    }

    // Handle response without returning any value
    protected async Task<Result> HandleResponse(HttpResponseMessage response)
    {
        try
        {
            if (response.IsSuccessStatusCode)
                return Result.Success();

            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ErrorType>(content, Options);
            return Result.Failure(
                error ?? new ErrorType("UnknownError", "An unknown error occurred")
            );
        }
        catch (HttpRequestException httpRequestException)
        {
            return Result.Failure(
                new ErrorType("HttpRequestException", httpRequestException.Message)
            );
        }
        catch (Exception exception)
        {
            return Result.Failure(new ErrorType(exception.Message, exception.StackTrace!));
        }
    }

    protected async Task<Result<T>> HandleResponse<T>(HttpResponseMessage response)
        where T : class
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonSerializer.Deserialize<ErrorType>(content, Options)!;
                return Result.Failure<T>(new ErrorType(errorResult.Code, errorResult.Description));
            }

            var result = JsonSerializer.Deserialize<T>(content, Options)!;

            return Result.Success(result);
        }
        catch (NullReferenceException nullReferenceException)
        {
            return Result.Failure<T>(
                new ErrorType(nullReferenceException.Source!, nullReferenceException.Message)
            );
        }
        catch (JsonException jsonException)
        {
            return Result.Failure<T>(new ErrorType(jsonException.HelpLink!, jsonException.Message));
        }
        catch (HttpRequestException httpRequestException)
        {
            return Result.Failure<T>(
                new ErrorType("httpRequestException ", httpRequestException.Message)
            );
        }
        catch (Exception exception)
        {
            return Result.Failure<T>(new ErrorType(exception.Message, exception.StackTrace!));
        }
    }

    protected static bool HasIBrowserFile<TRequest>(TRequest item)
        where TRequest : class
    {
        return item.GetType()
            .GetProperties()
            .Any(p => typeof(IBrowserFile).IsAssignableFrom(p.PropertyType));
    }
}
