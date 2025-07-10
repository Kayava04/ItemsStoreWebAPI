using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataAccess.DataBase.Startup
{
    public class DbInitializer
    {
        public static void Initialize(DbContext context) =>
            context.Database.EnsureCreated();
    }
}