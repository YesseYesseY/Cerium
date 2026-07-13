using System.Globalization;

var builder =  WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
var app = builder.Build();

string AccountId = "c7935d0bf67b617271a3344d4027826e";
string AccountUsername = "YesseY";
string AccountEmail = "YesseY@yesmail.yes";

app.MapPost("/account/api/oauth/token", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();
    var grantType = form["grant_type"].ToString();

    if (grantType == "client_credentials")
    {
        return Results.Json(new
        {
            access_token = "accesstoken",
            expires_in = 14400,
            expires_at = "9999-08-30T20:32:21.466Z",
            token_type = "bearer",
            client_id = "ec684b8c687f479fadea3cb2ad83f5c6",
            internal_client = true,
            client_service = "prod-fn",
            product_id = "prod-fn",
            application_id = "fghi4567FNFBKFz3E4TROb0bmPS8h1GW"
        });
    }

    if (grantType != "password")
    {
        return Results.NotFound();
    }

    return Results.Json(new
    {
        access_token = "accesstoken",
        expires_in = 14400,
        expires_at = "9999-08-30T20:32:21.466Z",
        token_type = "bearer",
        refresh_token = "refreshtoken",
        refresh_expires = 28800,
        refresh_expires_at = "9999-08-30T20:32:21.466Z",
        account_id = AccountId,
        client_id = "ec684b8c687f479fadea3cb2ad83f5c6",
        internal_client = true,
        client_service = "prod-fn",
        displayName = AccountUsername,
        app = "prod-fn",
        in_app_id = AccountId,
        product_id = "prod-fn",
        application_id = "fghi4567FNFBKFz3E4TROb0bmPS8h1GW"
    });
});

app.MapGet("/fortnite/api/v2/versioncheck/Windows", (HttpRequest request) => new
{
    type = "NO_UPDATE"
});

app.MapPost("/fortnite/api/game/v2/tryPlayOnPlatform/account/{accountId}", (string accountId, HttpRequest request) => "true");
app.MapGet("/account/api/public/account/{accountId}", (string accountId, HttpRequest request) => new
{
    id = AccountId,
    displayName = AccountUsername,
    name = AccountUsername,
    email = AccountEmail,
    failedLoginAttempts = 0,
    lastLogin = "2000-01-01T00:00:00.000Z",
    numberOfDisplayNameChanges = 0,
    ageGroup = "UNKNOWN",
    headless = false,
    country = "BE",
    lastName = AccountUsername,
    phoneNumber = "12345667890",
    company = AccountUsername,
    preferredLanguage = "en",
    lastDisplayNameChange = "2000-01-01T00:00:00.000Z",
    canUpdateDisplayName = true,
    tfaEnabled = true,
    emailVerified = true,
    minorVerified = false,
    minorExpected = false,
    minorStatus = "NOT_MINOR",
    guardianChallengeTimestamp = "2000-01-01T00:00:00.000Z",
    siweNotificationEnabled = true,
    cabinedMode = false,
    hasHashedEmail = false,
    lastReviewedSecuritySettings = "2000-01-01T00:00:00.000Z",
});

app.MapGet("/account/api/public/account", (HttpRequest request) =>
{
    return Results.Json(new[]
    {
        new {
            id = AccountId,
            displayName = AccountUsername,
            externalAuths = Array.Empty<object>()
        }
    });
});

app.MapGet("/lightswitch/api/service/bulk/status", (HttpRequest request) => new []
{
    new
    {
        serviceInstanceId = "fortnite",
        status = "UP",
        message = "Fortnite is online",
        maintenanceUri = string.Empty,
        overrideCatalogIds = new[]
        {
            "a7f138b2e51945ffbfdacc1af0541053"
        },
        allowedActions = Array.Empty<object>(),
        banned = false,
        launcherInfoDTO = new Dictionary<string, string>()
        {
            { "appName", "Fortnite" },
            { "catalogItemId", "4fe75bbc5a674f4f9b356b5c90567da5" },
            { "namespace", "fn" }
        }
    }
});

app.MapPost("/fortnite/api/game/v2/profile/{accountId}/client/{operation}", (string accountId, string operation, HttpRequest request) =>
{
    string profileId = "common_core";

    if (request.Query.TryGetValue("profileId", out var val))
        profileId = val.ToString();

    return Results.Json(new
    {
        profileRevision = 1,
        profileId = profileId,
        profileChangesBaseRevision = 1,
        profileChanges = Array.Empty<object>(),
        profileCommandRevision = 1,
        serverTime = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
        responseVersion = 1
    });
});

app.MapGet("/fortnite/api/storefront/v2/keychain", (HttpRequest request) =>
{
    return Results.Json(new[]
    {
        "1234642F4676A00CE54CA7B32D78AF0C:Nd8vhYp296C+C0TqSIGxu0nBYOFGQ5xBNK5MFjHS8IA="
    });
});

app.MapGet("/fortnite/api/game/v2/enabled_features", (HttpRequest request) => Array.Empty<object>());
app.MapGet("/fortnite/api/cloudstorage/user/{accountId}", (string accountId, HttpRequest request) => Array.Empty<object>());
app.MapGet("/fortnite/api/cloudstorage/system", (HttpRequest request) => Array.Empty<object>());

app.MapFallback((HttpContext context) =>
{
    Console.WriteLine($"[{context.Request.Method}] \"{context.Request.Path.Value}\"");
    return Results.NotFound();
});

app.Run("http://127.0.0.1:3551");