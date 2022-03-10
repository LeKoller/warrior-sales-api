using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Services
{
    public class ProductsService
    {
        public static List<Product> CreateSeedList()
        {
            List<Product> list = new();

            Product StoutBeer = new()
            {
                Name = "Stout Beer",
                Category = "Bevarages",
                Description = "A black beer.",
                Price = 16.4f,
                Stock = 100
            };
            Product WinterBoot = new()
            {
                Name = "Winter Boot",
                Category = "Shoes",
                Description = "A warm boot for the winters.",
                Price = 174.8f,
                Stock = 40
            };
            Product NintendoSwitch = new()
            {
                Name = "Nintendo Switch",
                Category = "Video Games",
                Description = "A Nintendo console.",
                Price = 3100f,
                Stock = 30
            };

            list.Add(StoutBeer);
            list.Add(WinterBoot);
            list.Add(NintendoSwitch);

            return list;
        }
    }
}
