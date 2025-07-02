using System.Text;
using Dapper;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.DTOs.Mobile;
using ItemsStoreWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.Repositories
{
    public class MobileDbStorage(
        string connectionString,
        ILogger<MobileDbStorage> logger) : IStorage<Mobile>
    {
        public async Task<Mobile?> AddAsync(Mobile mobile)
        {
            const string insertStockItemSql = @"
                INSERT INTO StockItems (Name, Description, ReleasedYear, Price, InStock, AddedAt, ModifiedAt)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Description, @ReleasedYear, @Price, @InStock, @AddedAt, @ModifiedAt);";

            const string insertMobileSql = @"
                INSERT INTO Mobiles (StockItemId, OS, ScreenSize, BatteryCapacity, RAM, Storage)
                OUTPUT INSERTED.Id
                VALUES (@StockItemId, @OS, @ScreenSize, @BatteryCapacity, @RAM, @Storage);";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var tx = connection.BeginTransaction();
            
            var stockItemId = await connection.ExecuteScalarAsync<int>(
                insertStockItemSql,
                new
                {
                    mobile.Name,
                    mobile.Description,
                    mobile.ReleasedYear,
                    mobile.Price,
                    mobile.InStock,
                    mobile.AddedAt,
                    mobile.ModifiedAt
                },
                transaction: tx);
            
            var mobileId = await connection.ExecuteScalarAsync<int>(
                insertMobileSql,
                new
                {
                    StockItemId = stockItemId,
                    mobile.OS,
                    mobile.ScreenSize,
                    mobile.BatteryCapacity,
                    mobile.RAM,
                    mobile.Storage
                },
                transaction: tx);
            
            await tx.CommitAsync();
            
            logger.LogInformation($"Added Mobile with ID: {mobile.Id}");
            return mobile;
        }

        public async Task<Mobile?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT
                    s.Id, s.Name, s.Description, s.ReleasedYear, s.Price, s.InStock, s.AddedAt, s.ModifiedAt,
                    m.OS, m.ScreenSize, m.BatteryCapacity, m.RAM, m.Storage
                FROM StockItems s
                JOIN Mobiles m ON m.StockItemId = s.Id
                WHERE s.Id = @Id;";

            await using var connection = new SqlConnection(connectionString);
            var result = await connection.QueryFirstOrDefaultAsync<Mobile>(query, new { Id = id });

            if (result == null)
            {
                logger.LogWarning($"Mobile with ID: {id} not found");
                return null;
            }
            
            logger.LogInformation($"Found Mobile with ID: {result.Id}");
            return result;
        }

        public async Task<IEnumerable<Mobile>> GetAllAsync(IFilterDto? filter = null)
        {
            var query = new StringBuilder(@"
                SELECT
                    s.Id, s.Name, s.Description, s.ReleasedYear, s.Price, s.InStock, s.AddedAt, s.ModifiedAt,
                    m.OS, m.ScreenSize, m.BatteryCapacity, m.RAM, m.Storage
                FROM StockItems s
                JOIN Mobiles m ON m.StockItemId = s.Id
                WHERE 1 = 1;");
            
            var parameters = new DynamicParameters();

            if (filter is MobileFilterDto f)
            {
                if (!string.IsNullOrWhiteSpace(f.Name))
                {
                    query.Append(" AND s.Name LIKE @Name");
                    parameters.Add("@Name", $"%{f.Name}%");
                }

                if (f.MinScreenSize != null) { query.Append(" AND m.ScreenSize >= @MinScreenSize"); parameters.Add("@MinScreenSize", f.MinScreenSize); }
                if (f.MaxScreenSize != null) { query.Append(" AND m.ScreenSize <= @MaxScreenSize"); parameters.Add("@MaxScreenSize", f.MaxScreenSize); }
                if (f.MinBatteryCapacity != null) { query.Append(" AND m.BatteryCapacity >= @MinBatteryCapacity"); parameters.Add("@MinBatteryCapacity", f.MinBatteryCapacity); }
                if (f.MaxBatteryCapacity != null) { query.Append(" AND m.BatteryCapacity <= @MaxBatteryCapacity"); parameters.Add("@MaxBatteryCapacity", f.MaxBatteryCapacity); }
                if (f.MinRAM != null) { query.Append(" AND m.RAM >= @MinRAM"); parameters.Add("@MinRAM", f.MinRAM); }
                if (f.MaxRAM != null) { query.Append(" AND m.RAM <= @MaxRAM"); parameters.Add("@MaxRAM", f.MaxRAM); }
                if (f.MinStorage != null) { query.Append(" AND m.Storage >= @MinStorage"); parameters.Add("@MinStorage", f.MinStorage); }
                if (f.MaxStorage != null) { query.Append(" AND m.Storage <= @MaxStorage"); parameters.Add("@MaxStorage", f.MaxStorage); }
                if (f.MinPrice != null) { query.Append(" AND s.Price >= @MinPrice"); parameters.Add("@MinPrice", f.MinPrice); }
                if (f.MaxPrice != null) { query.Append(" AND s.Price <= @MaxPrice"); parameters.Add("@MaxPrice", f.MaxPrice); }
                if (f.ReleasedYear != null) { query.Append(" AND s.ReleasedYear = @ReleasedYear"); parameters.Add("@ReleasedYear", f.ReleasedYear); }
                if (f.InStock != null) { query.Append(" AND s.InStock >= @InStock"); parameters.Add("@InStock", f.InStock); }
            }
            
            var finalQuery = query.ToString();
            
            await using var connection = new SqlConnection(connectionString);
            var mobiles = await connection.QueryAsync<Mobile>(finalQuery, parameters);
            
            var result = mobiles.ToList();
            
            logger.LogInformation($"Receiving all Mobiles. Total count: {result.Count}");
            return result;
        }

        public async Task<Mobile?> UpdateAsync(int id, Mobile updatedMobile)
        {
            const string updateStockItemSql = @"
                UPDATE StockItems
                SET Name = @Name, Description = @Description, ReleasedYear = @ReleasedYear,
                    Price = @Price, InStock = @InStock, ModifiedAt = @ModifiedAt
                WHERE Id = @Id;";

            const string updateMobileSql = @"
                UPDATE Mobiles
                SET OS = @OS, ScreenSize = @ScreenSize, BatteryCapacity = @BatteryCapacity,
                    RAM = @RAM, Storage = @Storage
                WHERE StockItemId = @Id;";
            
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var tx = connection.BeginTransaction();
            
            await connection.ExecuteAsync(updateStockItemSql, new
            {
                updatedMobile.Name,
                updatedMobile.Description,
                updatedMobile.ReleasedYear,
                updatedMobile.Price,
                updatedMobile.InStock,
                ModifiedAt = DateTime.UtcNow,
                Id = id
            },
            transaction: tx);
            
            var mobileId = await connection.ExecuteAsync(updateMobileSql, new
            {
                updatedMobile.OS,
                updatedMobile.ScreenSize,
                updatedMobile.BatteryCapacity,
                updatedMobile.RAM,
                updatedMobile.Storage,
                Id = id
            },
            transaction: tx);
            
            await tx.CommitAsync();
            
            if (mobileId < 1)
            {
                logger.LogWarning($"Attempted to update non-existent Mobile with ID: {id}");
                return null;
            }
            
            logger.LogInformation($"Updated Mobile with ID: {updatedMobile.Id}");
            return updatedMobile;
        }

        public async Task DeleteAsync(int id)
        {
            const string deleteMobileSql = "DELETE FROM Mobiles WHERE Id = @Id;";
            const string deleteStockItemSql = "DELETE FROM StockItems WHERE Id = @Id;";
            
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var tx = connection.BeginTransaction();

            var mobileId = await connection.ExecuteAsync(deleteMobileSql, new { Id = id }, transaction: tx);
            await connection.ExecuteAsync(deleteStockItemSql, new { Id = id }, transaction: tx);
            
            await tx.CommitAsync();
            
            if (mobileId > 0)
                logger.LogInformation($"Deleted Mobile with ID: {id}");
            else
                logger.LogWarning($"Attempted to delete non-existent Mobile with ID: {id}");
        }
    }
}