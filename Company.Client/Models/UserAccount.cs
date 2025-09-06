// File: Company.Client/Models/UserAccount.cs
namespace Company.Client.Models
{
    // This class represents the user's account details fetched from the API.
    public class UserAccount
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
