using System.Text.Json.Serialization;

namespace Cerium;

[JsonDerivedType(typeof(Item), typeDiscriminator: "base")]
[JsonDerivedType(typeof(LoadoutItem), typeDiscriminator: "loadout")]
public class Item(string profileId, string id, int quantity = 1)
{
    public Guid ItemGuid { get; init; } = Guid.NewGuid();
    public string ProfileId { get; init; } = profileId;
    public string Id { get; set; } = id;
    public int Quantity { get; set; } = quantity;
    public float BuildLimit { get; set; } = -1.0f;
    public bool Seen { get; set; } = false;

    public virtual object Attributes(Account account)
    {
        return new
        {
            item_seen = Seen
        };
    }

    public object Objectify(Account account)
    {
        return new
        {
            templateId = Id,
            quantity = Quantity,
            attributes = Attributes(account)
        };
    }
}