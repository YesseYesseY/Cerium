using System.Text.Json;
using System.Text.Json.Serialization;
using Cerium.Attributes;

namespace Cerium.Controller;

public class CatalogConfigItem
{
    public required string Id { get; set; }
    public required int Quantity { get; set; }
    public required int Price { get; set; }
    public required string CurrencyType { get; set; }
}

public class CatalogConfigStorefront
{
    public required string Name { get; set; }
    public required CatalogConfigItem[] Items { get; set; }
}

public class CatalogConfig
{
    public required CatalogConfigStorefront[] Storefronts { get; init; }
}

[CeriumController]
public static class StorefrontController
{
    private static object CatalogOffer(string itemId, int price, int quantity = 1, string currencyType = "MtxCurrency")
    {
        return new
        {
            offerId = "uwu",
            prices = new[]
            {
                new
                {
                    currencyType = currencyType,
                    currencySubType = "",
                    regularPrice = price,
                    dynamicRegularPrice = price,
                    finalPrice = price,
                    saleExpiration = DateTime.MaxValue,
                    basePrice = price
                }
            },
            metaInfo = new[]
            {
                new
                {
                    key = "templateId",
                    value = itemId
                }
            },
            requirements = new[]
            {
                new
                {
                    requirementType = "DenyOnItemOwnership",
                    requiredId = itemId,
                    minQuantity = quantity
                }
            },
            offerType = "StaticPrice",
            refundable = true,
            itemGrants = new[]
            {
                new
                {
                    templateId = itemId,
                    quantity = quantity
                }
            }
        };
    }

    [CeriumRoute("GET", "/fortnite/api/storefront/v2/keychain")]
    public static IResult GetKeychain(HttpRequest request)
    {
        return Results.Json(new[]
        {
            "1234642F4676A00CE54CA7B32D78AF0C:Nd8vhYp296C+C0TqSIGxu0nBYOFGQ5xBNK5MFjHS8IA="
        });
    }

    [CeriumRoute("GET", "/fortnite/api/storefront/v2/catalog")]
    public static IResult GetCatalog(HttpRequest request)
    {
        if (CatalogConfig is null)
            return Results.NoContent();

        List<object> storefronts = [];

        for (var i = 0; i < CatalogConfig.Storefronts.Length; i++)
        {
            var storefront = CatalogConfig.Storefronts[i];
            var i1 = i;
            storefronts.Add(new
            {
                name = storefront.Name,
                catalogEntries = storefront.Items.Select(item => CatalogOffer(item.Id, item.Price, item.Quantity, item.CurrencyType))
            });
        }

        return Results.Json(new
        {
            refreshIntervalHrs = 24,
            dailyPurchaseHrs = 24,
            expiration = DateTime.UtcNow.AddHours(24),
            storefronts = storefronts
        });
    }

    public static CatalogConfig? CatalogConfig;

    public static void Init()
    {
        var catalogPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Configs", "Catalog.json");
        CatalogConfig = JsonSerializer.Deserialize<CatalogConfig>(File.ReadAllText(catalogPath));
    }
}