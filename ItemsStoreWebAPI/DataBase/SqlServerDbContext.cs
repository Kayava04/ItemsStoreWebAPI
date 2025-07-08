using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataBase
{
    public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : BaseDbContext(options)
    {
    }
}