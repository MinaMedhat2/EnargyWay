// File: Company.Client/Models/UserSession.cs
namespace Company.Client.Models
{
    /// <summary>
    /// Represents the currently logged-in user's session data.
    /// </summary>
    public class UserSession
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // You can add more properties here later, like UserId, Role, etc.
    }
}
