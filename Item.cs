namespace Cerium;

public class Item(string id, int quantity = 1)
{
    public string Id { get; set; } = id;
    public int Quantity { get; set; } = quantity;
    public Dictionary<string, object> Attributes { get; set; } = new();
    public float BuildLimit { get; set; } = -1.0f;

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