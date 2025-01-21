using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPITests.Factories
{
    public class TVFactory : ITVFactory
    {
        public TV CreateTV()
        {
            return new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };
        }
    }
}