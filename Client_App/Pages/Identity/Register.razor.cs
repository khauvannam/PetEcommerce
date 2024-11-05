using Client_App.DTOs.Identities.Requests;
using Client_App.DTOs.Identities.Responses;
using Microsoft.AspNetCore.Components;

namespace Client_App.Pages.Identity;

public partial class Register
{
    [SupplyParameterFromForm]
    private RegisterFormModel? RegisterModel { get; set; }

    private string FirstName { get; set; } = string.Empty;

    private string LastName { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var token = await LocalStorage.GetItemAsync<LoginResponse>("jwt");

        if (token is not null)
            NavigationManager.NavigateTo("/profile");

        RegisterModel ??= RegisterFormModel.Empty;
    }

    private void HandleUsernameInput()
    {
        RegisterModel!.Username = $"{FirstName}{LastName}".Replace(" ", "").ToLower();
        Console.WriteLine(RegisterModel.Username);
    }

    private async Task HandleValidSubmit()
    {
        var result = await IdentityService.Register(RegisterModel!);

        if (result.IsFailure)
        {
            ErrorModalService.SetErrorMessage(result.ErrorTypes).NavigateTo(default);
            return;
        }

        NavigationManager.NavigateTo("/login");
    }
}
