// File Location: Company.Client/Services/AuthService.cs
using Company.Client.Models; // This allows us to use the LoginResult model

public class AuthService
{
    // This property will hold the user's data after a successful login.
    // It's 'null' if no one is logged in.
    public LoginResult? CurrentUser { get; private set; }

    // A simple way to check if a user is logged in.
    public bool IsLoggedIn => CurrentUser != null;

    // This event will be used to notify other components (like MainLayout)
    // that the user's login state has changed (either logged in or logged out).
    public event Action? OnChange;

    // This method will be called from Login.razor after a successful API call.
    // It stores the user's data and notifies everyone that a login has occurred.
    public void SetUser(LoginResult user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    // This method will be called to log the user out.
    // It clears the user's data and notifies everyone that a logout has occurred.
    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    // This is a helper method that simply triggers the OnChange event.
    private void NotifyStateChanged() => OnChange?.Invoke();
}
