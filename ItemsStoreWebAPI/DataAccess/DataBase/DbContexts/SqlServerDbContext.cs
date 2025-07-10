using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataAccess.DataBase.DbContexts
{
    public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : BaseDbContext(options)
    {
    }
}