using CoffeePos.Domain.Inventory;

namespace CoffeePos.Infrastructure.Storage;

public static class PilotSeedData
{
    public static readonly Guid DemoTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static List<StockItem> GetPilotStockItems()
    {
        return new List<StockItem>
        {
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888001"),
                DemoTenantId,
                "BEANS-HOUSE",
                "House Blend Whole Beans (Colombia/Ethiopia)",
                "Coffee Beans",
                UnitOfMeasure.Grams,
                15000.0000m, // 15kg
                3000.0000m,
                0.0850m // RM 0.085/g (RM 85/kg)
            ),
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888002"),
                DemoTenantId,
                "BEANS-SOE-GUJI",
                "Ethiopia Guji Single Origin (Natural)",
                "Coffee Beans",
                UnitOfMeasure.Grams,
                6500.0000m, // 6.5kg
                1500.0000m,
                0.1400m // RM 0.14/g (RM 140/kg)
            ),
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888003"),
                DemoTenantId,
                "MILK-FRESH",
                "Meiji Fresh Full Cream Milk (2L)",
                "Dairy",
                UnitOfMeasure.Milliliters,
                24000.0000m, // 24 Litres (12 jugs)
                6000.0000m,
                0.0075m // RM 0.0075/ml (RM 15/2L jug)
            ),
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888004"),
                DemoTenantId,
                "MILK-OAT",
                "Oatly Barista Edition Oat Milk (1L)",
                "Alternative Dairy",
                UnitOfMeasure.Milliliters,
                12000.0000m, // 12 Litres
                3000.0000m,
                0.0160m // RM 0.016/ml (RM 16/1L carton)
            ),
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888005"),
                DemoTenantId,
                "SYRUP-VANILLA",
                "Monin Madagascar Vanilla Syrup (700ml)",
                "Syrups",
                UnitOfMeasure.Milliliters,
                2800.0000m, // 4 bottles
                700.0000m,
                0.0650m
            ),
            new StockItem(
                Guid.Parse("88888888-8888-8888-8888-888888888006"),
                DemoTenantId,
                "PASTRY-CROISSANT",
                "Artisan Butter Croissant (Frozen)",
                "Bakery",
                UnitOfMeasure.Pieces,
                36.0000m,
                10.0000m,
                3.5000m
            )
        };
    }

    public static List<Recipe> GetPilotRecipes()
    {
        var espressoRecipe = new Recipe(
            Guid.Parse("99999999-9999-9999-9999-999999999001"),
            DemoTenantId,
            Guid.Parse("33333333-3333-3333-3333-333333333001"), // Espresso
            "Standard Double Shot Espresso (18g Dose)");
        espressoRecipe.AddItem(Guid.Parse("88888888-8888-8888-8888-888888888001"), 18.0000m); // 18g house beans

        var latteRecipe = new Recipe(
            Guid.Parse("99999999-9999-9999-9999-999999999002"),
            DemoTenantId,
            Guid.Parse("33333333-3333-3333-3333-333333333002"), // Latte
            "Caffe Latte (18g Beans + 200ml Fresh Milk)");
        latteRecipe.AddItem(Guid.Parse("88888888-8888-8888-8888-888888888001"), 18.0000m);
        latteRecipe.AddItem(Guid.Parse("88888888-8888-8888-8888-888888888003"), 200.0000m);

        return new List<Recipe> { espressoRecipe, latteRecipe };
    }
}
