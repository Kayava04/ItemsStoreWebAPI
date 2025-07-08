using Dapper;
using ItemsStoreWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.DataBase.Transactions
{
    public class MobileDbTransactions(
        string connectionString,
        ILogger<MobileDbTransactions> logger) : BaseDbTransactions<Mobile>(connectionString)
    {
        protected override async Task<IEnumerable<Mobile>> AddMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<Mobile> items)
        {
            var addedMobiles = new List<Mobile>();
            
            foreach (var item in items)
            {
                // Adding StockItem
                const string insertStockItemSql = @"
                    INSERT INTO StockItems (Name, Description, ReleasedYear, Price, InStock, AddedAt, ModifiedAt)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Description, @ReleasedYear, @Price, @InStock, @AddedAt, @ModifiedAt);";

                var stockItemId = await connection.ExecuteScalarAsync<int>(insertStockItemSql, new
                {
                    item.Name,
                    item.Description,
                    item.ReleasedYear,
                    item.Price,
                    item.InStock,
                    item.AddedAt,
                    item.ModifiedAt
                },
                transaction: transaction);

                // Adding Mobile
                const string insertMobileSql = @"
                    INSERT INTO Mobiles (StockItemId, OS, ScreenSize, BatteryCapacity, RAM, Storage)
                    OUTPUT INSERTED.Id
                    VALUES (@StockItemId, @OS, @ScreenSize, @BatteryCapacity, @RAM, @Storage);";

                var mobileId = await connection.ExecuteScalarAsync<int>(insertMobileSql, new
                {
                    StockItemId = stockItemId,
                    item.OS,
                    item.ScreenSize,
                    item.BatteryCapacity,
                    item.RAM,
                    item.Storage
                },
                transaction: transaction);

                item.Id = mobileId;
                addedMobiles.Add(item);
            }

            logger.LogInformation($"Added {addedMobiles.Count} Mobiles with IDs: {string.Join(", ", addedMobiles.Select(m => m.Id))}");
            return addedMobiles;
        }

        protected override async Task<IEnumerable<Mobile>> UpdateMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<Mobile> items)
        {
            var updatedMobiles = new List<Mobile>();
            
            foreach (var item in items)
            {
                const string getStockItemIdSql = "SELECT StockItemId FROM Mobiles WHERE Id = @Id;";
                var stockItemId = await connection.ExecuteScalarAsync<int?>(getStockItemIdSql, new { item.Id }, transaction);

                if (stockItemId == null)
                    logger.LogWarning($"Mobile with ID: {item.Id} not found");

                // Update StockItem
                const string updateStockItemSql = @"
                    UPDATE StockItems SET
                        Name = @Name,
                        Description = @Description,
                        ReleasedYear = @ReleasedYear,
                        Price = @Price,
                        InStock = @InStock,
                        ModifiedAt = @ModifiedAt
                    WHERE Id = @Id;";

                await connection.ExecuteAsync(updateStockItemSql, new
                {
                    Id = stockItemId,
                    item.Name,
                    item.Description,
                    item.ReleasedYear,
                    item.Price,
                    item.InStock,
                    ModifiedAt = DateTime.UtcNow
                },
                transaction: transaction);

                // Update Mobile
                const string updateMobileSql = @"
                    UPDATE Mobiles SET
                        OS = @OS,
                        ScreenSize = @ScreenSize,
                        BatteryCapacity = @BatteryCapacity,
                        RAM = @RAM,
                        Storage = @Storage
                    WHERE Id = @Id;";

                await connection.ExecuteAsync(updateMobileSql, new
                {
                    item.Id,
                    item.OS,
                    item.ScreenSize,
                    item.BatteryCapacity,
                    item.RAM,
                    item.Storage
                },
                transaction: transaction);

                updatedMobiles.Add(item);
            }

            logger.LogInformation($"Updated {updatedMobiles.Count} Mobiles with IDs: {string.Join(", ", updatedMobiles.Select(m => m.Id))}");
            return updatedMobiles;
        }

        protected override async Task DeleteMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<Mobile> items)
        {
            var deletedMobileIds = new List<int>();
            
            foreach (var item in items)
            {
                // Get StockItem Id
                const string getStockItemIdSql = "SELECT StockItemId FROM Mobiles WHERE Id = @Id;";
                var stockItemId = await connection.ExecuteScalarAsync<int?>(getStockItemIdSql, new { item.Id }, transaction);

                if (stockItemId == null)
                    continue;

                // Delete Mobile
                const string deleteMobileSql = "DELETE FROM Mobiles WHERE Id = @Id;";
                await connection.ExecuteAsync(deleteMobileSql, new { item.Id }, transaction);

                // Delete StockItem
                const string deleteStockItemSql = "DELETE FROM StockItems WHERE Id = @Id;";
                await connection.ExecuteAsync(deleteStockItemSql, new { Id = stockItemId }, transaction);

                deletedMobileIds.Add(item.Id);
            }

            logger.LogInformation($"Deleted {deletedMobileIds.Count} Mobiles with IDs: {string.Join(", ", deletedMobileIds)}");
        }
    }
}