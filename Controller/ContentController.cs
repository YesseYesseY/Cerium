using Cerium.Attributes;
using Cerium.Extensions;

namespace Cerium.Controller;

[CeriumController]
public static class ContentController
{
    [CeriumRoute("GET", "/content/api/pages/fortnite-game")]
    public static IResult GetContentPages(HttpRequest request)
    {
        var buildInfo = request.GetBuildInfo();
        var backgroundStage = buildInfo.Season switch
        {
            10 => "seasonx",
            _ => $"season{buildInfo.Season}"
        };
        return Results.Json(new
        {
            lastModified = DateTime.Today,
            _locale = "en-US",
            _templateName = "blank",
            dynamicbackgrounds = new
            {
                backgrounds = new
                {
                    backgrounds = new[]
                    {
                        new
                        {
                            stage = backgroundStage,
                            _type = "DynamicBackground",
                            key = "Lobby"
                        },
                        new
                        {
                            stage = backgroundStage,
                            _type = "DynamicBackground",
                            key = "Vault"
                        }
                    },
                    _type = "DynamicBackgroundList" // What even are these _types, it works perfectly fine without them
                },
                _title = "dynamicbackgrounds",
                _noIndex = false,
                _activeDate = DateTime.MinValue,
                lastModified = DateTime.Today,
                _locale = "en-US",
                _templateName = "FortniteGameDynamicBackgrounds"
            }
        });
    }
}