using Client_App.DTOs.Identities.Requests;
using Client_App.DTOs.Identities.Responses;
using Microsoft.AspNetCore.Components;

namespace Client_App.Pages.Identity;

public partial class Login : ComponentBase
{
    private LoginFormModel LoginModel { get; } = LoginFormModel.Empty;

    protected override async Task OnInitializedAsync()
    {
        var token = await LocalStorage.GetItemAsync<LoginResponse>("jwt");

        if (token is not null)
            NavigationManager.NavigateTo("/profile");
    }

    private async Task HandleValidSubmit()
    {
        var result = await IdentityService.Login(LoginModel);

        if (result.IsFailure)
            ErrorModalService.SetErrorMessage(result.ErrorTypes).NavigateTo(default);

        var jwtToken = result.Value;

        await LocalStorage.SetItemAsync("jwt", jwtToken);

        NavigationManager.NavigateTo("/");
    }
}
