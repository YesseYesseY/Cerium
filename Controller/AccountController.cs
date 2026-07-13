using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class AccountController
{
    public static string AccountId = "c7935d0bf67b617271a3344d4027826e";
    public static string AccountUsername = "YesseY";
    public static string AccountEmail = "YesseY@yesmail.yes";

    [CeriumRoute("POST", "/account/api/oauth/token")]
    public static async Task<IResult> PostOauthToken(HttpRequest request)
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
    }

    [CeriumRoute("GET", "/account/api/public/account/{accountId}")]
    public static IResult GetAccountLookupAccountId(string accountId, HttpRequest request)
    {
        return Results.Json(new
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
    }

    [CeriumRoute("GET", "/account/api/public/account")]
    public static IResult GetAccountLookupAccountIds(HttpRequest request)
    {
        return Results.Json(new[]
        {
            new
            {
                id = AccountId,
                displayName = AccountUsername,
                externalAuths = Array.Empty<object>()
            }
        });
    }
}