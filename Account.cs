using System.Text.Json.Serialization;

namespace Cerium;

public class Account
{
    public string Username { get; set; }
    public Guid Id { get; }

    [JsonIgnore]
    public string Email => $"{Id:N}@yesmail.com";

    public Account()
    {
        Username = "NoUsername";
        Id = Guid.Empty;
    }

    public Account(string username)
    {
        Username = username;
        Id = Guid.NewGuid();
    }
}