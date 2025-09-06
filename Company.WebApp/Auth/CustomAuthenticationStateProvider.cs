namespace Company.WebApp.Auth;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "userSession");
            if (string.IsNullOrWhiteSpace(userSessionJson))
            {
                return new AuthenticationState(_anonymous);
            }

            var userSession = JsonSerializer.Deserialize<UserSession>(userSessionJson);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userSession.Username),
                new Claim(ClaimTypes.Role, userSession.Role)
            };
            var identity = new ClaimsIdentity(claims, "apiauth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;
        if (userSession != null)
        {
            var userSessionJson = JsonSerializer.Serialize(userSession);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "userSession", userSessionJson);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userSession.Username),
                new Claim(ClaimTypes.Role, userSession.Role)
            };
            var identity = new ClaimsIdentity(claims, "apiauth");
            claimsPrincipal = new ClaimsPrincipal(identity);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "userSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public class UserSession
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
