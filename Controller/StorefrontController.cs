using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class StorefrontController
{
    [CeriumRoute("GET", "/fortnite/api/storefront/v2/keychain")]
    public static IResult GetKeychain(HttpRequest request)
    {
        return Results.Json(new[]
        {
            "1234642F4676A00CE54CA7B32D78AF0C:Nd8vhYp296C+C0TqSIGxu0nBYOFGQ5xBNK5MFjHS8IA="
        });
    }
}