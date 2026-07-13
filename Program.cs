using System.Globalization;
using Cerium;

var builder =  WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
var app = builder.Build();

app.MapGet("/account/api/public/account/{accountId}", AccountController.GetAccountLookupAccountId);
app.MapGet("/account/api/public/account", AccountController.GetAccountLookupAccountIds);
app.MapPost("/account/api/oauth/token", AccountController.PostOauthToken);

app.MapPost("/fortnite/api/game/v2/profile/{accountId}/client/{operation}", ProfileController.PostProfileOperation);

app.MapGet("/fortnite/api/v2/versioncheck/Windows", FortniteController.GetVersionCheck);
app.MapGet("/fortnite/api/game/v2/enabled_features", FortniteController.GetEnableFeatures);
app.MapPost("/fortnite/api/game/v2/tryPlayOnPlatform/account/{accountId}", FortniteController.PostTryPlayOnPlatform);

app.MapGet("/fortnite/api/cloudstorage/user/{accountId}", CloudstorageController.GetUserFiles);
app.MapGet("/fortnite/api/cloudstorage/system", CloudstorageController.GetSystemFiles);

app.MapGet("/lightswitch/api/service/bulk/status", LightswitchController.GetServiceStatusBulk);

app.MapGet("/fortnite/api/storefront/v2/keychain", StorefrontController.GetKeychain);

app.MapFallback((HttpContext context) =>
{
    Console.WriteLine($"[{context.Request.Method}] \"{context.Request.Path.Value}\"");
    return Results.NotFound();
});

app.Run("http://127.0.0.1:3551");