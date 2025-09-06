// File Location: Company.Client/Models/LoginResult.cs
using System.Text.Json.Serialization;

public class LoginResult
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("userRole")]
    public string UserRole { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}
