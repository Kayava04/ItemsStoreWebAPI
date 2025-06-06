using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataBase
{
    public class DbInitializer
    {
        public static void Initialize(DbContext context) =>
            context.Database.EnsureCreated();
    }
}