using Client_App;
using Client_App.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClientAddress();
builder.Services.AddFetchApiService();
builder.Services.AddPersistenceService();

await builder.Build().RunAsync();
