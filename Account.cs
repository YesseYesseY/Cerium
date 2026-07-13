using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace Cerium;

public class Account
{
    public Guid Id { get; }
    public string Username { get; set; }
    public DateTime Created { get; }
    public Dictionary<string, Profile> Profiles { get; set; }

    [JsonIgnore]
    public string Email => $"{Id:N}@yesmail.com";

    public Account()
    {
        Id = Guid.Empty;
        Username = "NoUsername";
        Created = DateTime.UtcNow;
        Profiles = new Dictionary<string, Profile>();
    }

    public Account(string username)
    {
        Id = Guid.NewGuid();
        Username = username;
        Created = DateTime.UtcNow;
        Profiles = new Dictionary<string, Profile>();
        AddProfile(new AthenaProfile());
        AddProfile(new CommonCoreProfile());
        AddProfile(new CommonPublicProfile());
    }

    void AddProfile(Profile profile)
    {
        Profiles[profile.Id] = profile;
    }

    public Profile? GetProfile(string profileId)
    {
        return Profiles[profileId];
    }
}