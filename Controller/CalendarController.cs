using System.Text.Json.Serialization;
using Cerium.Attributes;
using Cerium.Extensions;

namespace Cerium.Controller;

public record CalendarEvent(string eventType, DateTime activeUntil, DateTime activeSince)
{
}

[CeriumController]
public static class CalendarController
{
    [CeriumRoute("GET", "/fortnite/api/calendar/v1/timeline")]
    public static IResult GetTimeline(HttpRequest request)
    {
        var buildInfo = request.GetBuildInfo();

        Console.WriteLine($"Build: {buildInfo.Build}");

        var minDate = DateTime.MinValue;
        var maxDate = DateTime.MaxValue;

        var clientEvents = new
        {
            states = new[]
            {
                new
                {
                    validFrom = DateTime.MinValue.ToIsoString(),
                    activeEvents = new[]
                    {
                        new CalendarEvent($"EventFlag.LobbySeason{buildInfo.Season}", maxDate, minDate)
                    },
                    state = new
                    {
                        activeStorefronts = Array.Empty<object>(),
                        eventNamedWeights = new {},
                        activeEvents = Array.Empty<object>(),
                        seasonNumber = buildInfo.Season,
                        seasonTemplateId = $"AthenaSeason:athenaseason{buildInfo.Season}",
                        matchXpBonusPoints = 0,
                        eventPunchCardTemplateId = "",
                        seasonBegin = minDate,
                        seasonEnd = maxDate,
                        seasonDisplayedEnd = maxDate,
                        weeklyStoreEnd = maxDate,
                        stwEventStoreEnd = maxDate,
                        stwWeeklyStoreEnd = maxDate,
                        sectionStoreEnds = new
                        {
                            Daily = maxDate,
                            Featured = maxDate
                        },
                        rmtPromotion = "",
                        dailyStoreEnd = maxDate
                    }
                }
            },
            cacheExpire = maxDate
        };

        return Results.Json(new
        {
            channels = new Dictionary<string, object>()
            {
                { "client-events", clientEvents }
            },
            cacheIntervalMins = 999,
            currentTime = DateTime.UtcNow.ToIsoString()
        });
    }
}