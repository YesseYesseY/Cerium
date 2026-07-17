using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class WaitingRoomController
{
    [CeriumRoute("GET", "/waitingroom/api/waitingroom")]
    public static IResult GetWaitingRoom(HttpRequest request)
    {
        return Results.NoContent();
    }
}