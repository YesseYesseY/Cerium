using System.Text.Json;

namespace Cerium.Managers;

public static class AccountManager
{
    private static readonly Dictionary<Guid, Account> AccountCache = new();
    private static readonly string AccountsPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Accounts");

    private static Account Create(string username)
    {
        var account = new Account(username);
        AccountCache[account.Id] = account;

        File.WriteAllText(Path.Join(AccountsPath, $"{account.Id:N}.json"), JsonSerializer.Serialize(account));

        return account;
    }

    public static Account GetOrCreateFromUsername(string username)
    {
        foreach (var account in AccountCache.Values)
        {
            if (account.Username == username)
            {
                return account;
            }
        }

        return Create(username);
    }

    public static Account? GetFromAccountId(Guid guid)
    {
        return AccountCache.GetValueOrDefault(guid);
    }

    public static void Init()
    {
        Directory.CreateDirectory(AccountsPath);

        foreach (var filePath in Directory.GetFiles(AccountsPath))
        {
            var fileText = File.ReadAllText(filePath);
            var account = JsonSerializer.Deserialize<Account>(fileText);

            if (account is null) continue;

            AccountCache[account.Id] = account;
            Console.WriteLine($"Added account {account.Username} to cache");
        }
    }
}