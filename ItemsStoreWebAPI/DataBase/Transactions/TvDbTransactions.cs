using ItemsStoreWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.DataBase.Transactions
{
    public class TvDbTransactions(
        string connectionString,
        ILogger<TvDbTransactions> logger) : BaseDbTransactions<TV>(connectionString)
    {
        protected override async Task<IEnumerable<TV>> AddMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<TV> items)
        {
            var addedTVs = new List<TV>();
            
            foreach (var item in items)
            {
                // Adding StockItem
                var stockItemCommand = new SqlCommand(@"
                    INSERT INTO StockItems (Name, Description, ReleasedYear, Price, InStock, AddedAt, ModifiedAt)
                    OUTPUT INSERTED.ID
                    VALUES (@Name, @Description, @ReleasedYear, @Price, @InStock, @AddedAt, @ModifiedAt);", connection, transaction);

                stockItemCommand.Parameters.AddWithValue("@Name", item.Name);
                stockItemCommand.Parameters.AddWithValue("@Description", item.Description);
                stockItemCommand.Parameters.AddWithValue("@ReleasedYear", item.ReleasedYear);
                stockItemCommand.Parameters.AddWithValue("@Price", item.Price);
                stockItemCommand.Parameters.AddWithValue("@InStock", item.InStock);
                stockItemCommand.Parameters.AddWithValue("@AddedAt", item.AddedAt);
                stockItemCommand.Parameters.AddWithValue("@ModifiedAt", item.ModifiedAt);

                var stockItemId = (int)(await stockItemCommand.ExecuteScalarAsync())!;

                // Adding TV
                var tvCommand = new SqlCommand(@"
                    INSERT INTO TVs (StockItemId, Size, Resolution, Frequency)
                    OUTPUT INSERTED.ID
                    VALUES (@StockItemId, @Size, @Resolution, @Frequency);", connection, transaction);

                tvCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                tvCommand.Parameters.AddWithValue("@Size", item.ScreenSize);
                tvCommand.Parameters.AddWithValue("@Resolution", item.Resolution);
                tvCommand.Parameters.AddWithValue("@Frequency", item.Frequency);

                var tvId = (int)(await tvCommand.ExecuteScalarAsync())!;

                item.Id = tvId;
                addedTVs.Add(item);
            }
            
            logger.LogInformation($"Added {addedTVs.Count} TVs with IDs: {string.Join(", ", addedTVs.Select(x => x.Id))}");
            return addedTVs;
        }

        protected override async Task<IEnumerable<TV>> UpdateMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<TV> items)
        {
            var updatedTVs = new List<TV>();
            
            foreach (var item in items)
            { 
                var getStockItemCommand = new SqlCommand("SELECT StockItemId FROM TVs WHERE ID = @Id;", connection, transaction);
                getStockItemCommand.Parameters.AddWithValue("@Id", item.Id);
                var stockItemIdObj = await getStockItemCommand.ExecuteScalarAsync();

                if (stockItemIdObj == null)
                    logger.LogWarning($"TV with ID: {item.Id} not found");

                var stockItemId = (int)stockItemIdObj!;

                // Update StockItem
                var updateStockItemCommand = new SqlCommand(@"
                    UPDATE StockItems
                    SET Name = @Name, Description = @Description, ReleasedYear = @ReleasedYear,
                        Price = @Price, InStock = @InStock, ModifiedAt = @ModifiedAt
                    WHERE ID = @StockItemId;", connection, transaction);

                updateStockItemCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                updateStockItemCommand.Parameters.AddWithValue("@Name", item.Name);
                updateStockItemCommand.Parameters.AddWithValue("@Description", item.Description);
                updateStockItemCommand.Parameters.AddWithValue("@ReleasedYear", item.ReleasedYear);
                updateStockItemCommand.Parameters.AddWithValue("@Price", item.Price);
                updateStockItemCommand.Parameters.AddWithValue("@InStock", item.InStock);
                updateStockItemCommand.Parameters.AddWithValue("@ModifiedAt", item.ModifiedAt);

                await updateStockItemCommand.ExecuteNonQueryAsync();

                // Update TV
                var updateTvCommand = new SqlCommand(@"
                    UPDATE TVs 
                    SET Size = @Size, Resolution = @Resolution, Frequency = @Frequency
                    WHERE ID = @Id;", connection, transaction);

                updateTvCommand.Parameters.AddWithValue("@Id", item.Id);
                updateTvCommand.Parameters.AddWithValue("@Size", item.ScreenSize);
                updateTvCommand.Parameters.AddWithValue("@Resolution", item.Resolution);
                updateTvCommand.Parameters.AddWithValue("@Frequency", item.Frequency);

                await updateTvCommand.ExecuteNonQueryAsync();

                updatedTVs.Add(item);
            }

            logger.LogInformation($"Updated {updatedTVs.Count} TVs with IDs: {string.Join(", ", updatedTVs.Select(x => x.Id))}");
            return updatedTVs;
        }

        protected override async Task DeleteMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<TV> items)
        {
            var deletedTvs = new List<int>();
            
            foreach (var item in items)
            {
                // Get StockItem Id
                var getStockItemCommand = new SqlCommand("SELECT StockItemId FROM TVs WHERE ID = @Id;", connection, transaction);
                getStockItemCommand.Parameters.AddWithValue("@Id", item.Id);
                var stockItemIdObj = await getStockItemCommand.ExecuteScalarAsync();

                if (stockItemIdObj == null)
                    continue;

                var stockItemId = (int)stockItemIdObj;

                // Delete TV
                var deleteTvCommand = new SqlCommand("DELETE FROM TVs WHERE ID = @Id;", connection, transaction);
                deleteTvCommand.Parameters.AddWithValue("@Id", item.Id);
                await deleteTvCommand.ExecuteNonQueryAsync();

                // Delete StockItem
                var deleteStockItemCommand = new SqlCommand("DELETE FROM StockItems WHERE ID = @StockItemId;", connection, transaction);
                deleteStockItemCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                await deleteStockItemCommand.ExecuteNonQueryAsync();
                
                deletedTvs.Add(item.Id);
            }
            
            logger.LogInformation($"Deleted {deletedTvs.Count} TVs with IDs: {string.Join(", ", deletedTvs)}");
        }
    }
}