namespace Cerium;

public class Item(string id, int quantity = 1)
{
    public string Id { get; set; } = id;
    public int Quantity { get; set; } = quantity;

    public object Objectify()
    {
        return new
        {
            templateId = Id,
            quantity = Quantity,
            attributes = new { }
        };
    }
}