using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class CloudstorageController
{
    private static Dictionary<string, string> StorageFiles { get; set; }= new();

    [CeriumRoute("GET", "/fortnite/api/cloudstorage/system")]
    public static IResult GetSystemFiles(HttpRequest request)
    {
        List<object> ret = [];
        foreach (var thing in StorageFiles)
        {
            ret.Add(new
            {
                uniqueFilename = thing.Key,
                filename = thing.Key,
                hash = "679e36413432f152559403329f1682c46f50f119",
                hash256 = "a15a3179d3f3c3b6f79f13784ea9e18d0152f9f9718d2f5f04248bf7c6b5bb24",
                length = thing.Value.Length,
                contentType = "application/octet-stream",
                uploaded = DateTime.Today.AddMonths(-2),
                storageType = "DSS",
                storageIds = new { },
                doNotCache = true
            });
        }
        return Results.Json(ret);
    }

    [CeriumRoute("GET", "/fortnite/api/cloudstorage/system/{uniqueFilename}")]
    public static IResult GetSystemFile(string uniqueFilename, HttpRequest request)
    {
        if (!StorageFiles.TryGetValue(uniqueFilename, out var file))
            return Results.NoContent();

        return Results.Text(file);
    }

    [CeriumRoute("GET", "/fortnite/api/cloudstorage/user/{accountId}")]
    public static IResult GetUserFiles(string accountId, HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }

    public static void Init()
    {
        var storagePath = Path.Join(Utils.ConfigPath, "CloudStorage");
        foreach (var filePath in Directory.GetFiles(storagePath))
            StorageFiles[Path.GetFileName(filePath)] = File.ReadAllText(filePath);
    }
}