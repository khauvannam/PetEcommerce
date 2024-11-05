using Blazored.LocalStorage;
using Client_App.DTOs.Identities.Responses;
using Client_App.Helpers.Jwt;
using Client_App.Interfaces;

namespace Client_App.Services.States;

public class AuthStateService(ILocalStorageService localStorage, ITokenService tokenService)
{
    private DateTime _lastChecked = DateTime.MinValue;

    public async Task CheckAndRefreshTokenAsync()
    {
        if ((DateTime.Now - _lastChecked).TotalMinutes < 15)
            return;

        var token = await localStorage.GetItemAsync<LoginResponse>("jwt");

        if (token != null && JwtHelper.CheckExpiredToken(token.AccessToken))
        {
            _lastChecked = DateTime.Now;
            await tokenService.Refresh(token.AccessToken);
        }
    }
}
