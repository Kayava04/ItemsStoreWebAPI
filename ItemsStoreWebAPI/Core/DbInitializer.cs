using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Core
{
    public class DbInitializer
    {
        public static void Initialize(DbContext context) =>
            context.Database.EnsureCreated();
    }
}