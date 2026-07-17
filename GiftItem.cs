namespace Cerium;

public class GiftItemEntry(string itemType, int quantity = 1)
{
    public string ItemType { get; set; } = itemType;
    public int Quantity { get; set; } = quantity;

    public GiftItemEntry(Item item) : this(item.Id, item.Quantity)
    {
    }
}

public class GiftItem(string id, params GiftItemEntry[] items) : Item("athena", id, 1)
{
    public DateTime GiftedOn { get; set; } = DateTime.UtcNow;
    public GiftItemEntry[] Items { get; set; } = items;

    public override object Attributes(Account account)
    {
        return new
        {
            giftedOn = GiftedOn,
            lootList = Items.Select(e => new
            {
                itemType = e.ItemType,
                itemProfile = ProfileId,
                quantity = e.Quantity
            })
        };
    }
}