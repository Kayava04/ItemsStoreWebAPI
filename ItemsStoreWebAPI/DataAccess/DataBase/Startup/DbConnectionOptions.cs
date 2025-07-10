namespace ItemsStoreWebAPI.DataAccess.DataBase.Startup
{
    public class DbConnectionOptions
    {
        public string PostgresDbContext { get; set; } = string.Empty;
        public string SqlServerDbContext { get; set; } = string.Empty;
    }
}