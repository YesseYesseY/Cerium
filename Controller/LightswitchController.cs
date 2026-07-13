using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class LightswitchController
{
    [CeriumRoute("GET", "/lightswitch/api/service/bulk/status")]
    public static IResult GetServiceStatusBulk(HttpRequest request)
    {
        return Results.Json(new[]
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
    }
}