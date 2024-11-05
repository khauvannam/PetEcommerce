using Microsoft.JSInterop;

namespace Client_App.Services.Clients;

public class TitleService(IJSRuntime jsRuntime)
{
    public async Task SetTitleAsync(string title)
    {
        await jsRuntime.InvokeVoidAsync("setDocumentTitle", $"{title} | Pet Shop");
    }

    public async Task SetTitleErrorAsync()
    {
        await jsRuntime.InvokeVoidAsync("setDocumentTitle", "404: Not Found");
    }
}
