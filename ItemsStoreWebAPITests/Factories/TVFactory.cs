using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPITests.Factories
{
    public class TVFactory
    {
        public static TV CreateTV(int id, string name, string description, float size, string resolution, float frequency, int releasedYear, decimal price, int inStock)
        {
            return new TV
            {
                ID = id,
                Name = name,
                Description = description,
                Size = size,
                Resolution = resolution,
                Frequency = frequency,
                ReleasedYear = releasedYear,
                Price = price,
                InStock = inStock
            };
        }

        public static TV CreateDefaultTV()
        {
            return CreateTV(1, "LG", "OLED TV", 55, "1920x1080", 100.5f, 2020, 25500, 2);
        }
    }
}