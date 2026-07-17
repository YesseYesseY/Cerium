using Cerium.Attributes;
using Cerium.Extensions;
using Cerium.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Cerium.Controller;

[CeriumController]
public static class AccountController
{
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

        var username = form["username"].ToString();
        var account = AccountManager.GetOrCreateFromUsername(username);
        account.CurrentBuildInfo = request.GetBuildInfo();

        return Results.Json(new
        {
            access_token = "accesstoken",
            expires_in = 14400,
            expires_at = "9999-08-30T20:32:21.466Z",
            token_type = "bearer",
            refresh_token = "refreshtoken",
            refresh_expires = 28800,
            refresh_expires_at = "9999-08-30T20:32:21.466Z",
            account_id = account.Id,
            client_id = "ec684b8c687f479fadea3cb2ad83f5c6",
            internal_client = true,
            client_service = "prod-fn",
            displayName = account.Username,
            app = "prod-fn",
            in_app_id = account.Username,
            product_id = "prod-fn",
            application_id = "fghi4567FNFBKFz3E4TROb0bmPS8h1GW"
        });
    }

    [CeriumRoute("GET", "/account/api/public/account/{accountId}")]
    public static IResult GetAccountLookupAccountId(Guid accountId, HttpRequest request)
    {
        var account = AccountManager.GetFromAccountId(accountId);
        if (account is null)
            return Results.NotFound();

        return Results.Json(new
        {
            id = account.Id,
            displayName = account.Username,
            name = account.Username,
            email = account.Email,
            failedLoginAttempts = 0,
            lastLogin = "2000-01-01T00:00:00.000Z",
            numberOfDisplayNameChanges = 0,
            ageGroup = "UNKNOWN",
            headless = false,
            country = "BE",
            lastName = account.Username,
            phoneNumber = "12345667890",
            company = "Cerium",
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
    public static async Task<IResult> GetAccountLookupAccountIds(HttpRequest request)
    {
        var ret = new List<object>();

        foreach (var accountIdStr in request.Query["accountId"].ToArray())
        {
            if (accountIdStr is null) continue;

            var accountId = Guid.Parse(accountIdStr);
            var account = AccountManager.GetFromAccountId(accountId);
            if (account is null) continue;

            ret.Add(new
            {
                id = account.Id,
                displayName = account.Username,
                externalAuths = Array.Empty<object>()
            });

            ret.Add(new
            {
                id = account.Id.ToString("N"),
                displayName = account.Username,
                externalAuths = Array.Empty<object>()
            });
        }

        return Results.Json(ret);
    }
}