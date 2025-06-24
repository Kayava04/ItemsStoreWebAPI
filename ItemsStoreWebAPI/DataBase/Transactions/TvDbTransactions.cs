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
                    INSERT INTO StockItems (Price, InStock, AddedAt, ModifiedAt)
                    OUTPUT INSERTED.ID
                    VALUES (@Price, @InStock, @AddedAt, @ModifiedAt)", connection, transaction);

                stockItemCommand.Parameters.AddWithValue("@Price", item.Price);
                stockItemCommand.Parameters.AddWithValue("@InStock", item.InStock);
                stockItemCommand.Parameters.AddWithValue("@AddedAt", item.AddedAt);
                stockItemCommand.Parameters.AddWithValue("@ModifiedAt", item.ModifiedAt);

                var stockItemId = (int)(await stockItemCommand.ExecuteScalarAsync())!;

                // Adding TV
                var tvCommand = new SqlCommand(@"
                    INSERT INTO TVs (StockItemId, Name, Description, Size, Resolution, Frequency, ReleasedYear)
                    OUTPUT INSERTED.ID
                    VALUES (@StockItemId, @Name, @Description, @Size, @Resolution, @Frequency, @ReleasedYear)", connection, transaction);

                tvCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                tvCommand.Parameters.AddWithValue("@Name", item.Name);
                tvCommand.Parameters.AddWithValue("@Description", item.Description);
                tvCommand.Parameters.AddWithValue("@Size", item.Size);
                tvCommand.Parameters.AddWithValue("@Resolution", item.Resolution);
                tvCommand.Parameters.AddWithValue("@Frequency", item.Frequency);
                tvCommand.Parameters.AddWithValue("@ReleasedYear", item.ReleasedYear);

                var tvId = (int)(await tvCommand.ExecuteScalarAsync())!;

                item.ID = tvId;
                addedTVs.Add(item);
            }
            
            logger.LogInformation($"Added {addedTVs.Count} TVs with IDs: {string.Join(", ", addedTVs.Select(x => x.ID))}");
            return addedTVs;
        }

        protected override async Task<IEnumerable<TV>> UpdateMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<TV> items)
        {
            var updatedTVs = new List<TV>();
            foreach (var item in items)
            { 
                var getStockItemCommand = new SqlCommand("SELECT StockItemId FROM TVs WHERE ID = @Id", connection, transaction);
                getStockItemCommand.Parameters.AddWithValue("@Id", item.ID);
                var stockItemIdObj = await getStockItemCommand.ExecuteScalarAsync();

                if (stockItemIdObj == null)
                    throw new Exception($"TV with ID {item.ID} not found");

                var stockItemId = (int)stockItemIdObj;

                // Update StockItem
                var updateStockItemCommand = new SqlCommand(@"
                    UPDATE StockItems 
                    SET Price = @Price, InStock = @InStock, ModifiedAt = @ModifiedAt
                    WHERE ID = @StockItemId", connection, transaction);

                updateStockItemCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                updateStockItemCommand.Parameters.AddWithValue("@Price", item.Price);
                updateStockItemCommand.Parameters.AddWithValue("@InStock", item.InStock);
                updateStockItemCommand.Parameters.AddWithValue("@ModifiedAt", item.ModifiedAt);

                await updateStockItemCommand.ExecuteNonQueryAsync();

                // Update TV
                var updateTvCommand = new SqlCommand(@"
                    UPDATE TVs 
                    SET Name = @Name, Description = @Description, Size = @Size, 
                        Resolution = @Resolution, Frequency = @Frequency, ReleasedYear = @ReleasedYear
                    WHERE ID = @Id", connection, transaction);

                updateTvCommand.Parameters.AddWithValue("@Id", item.ID);
                updateTvCommand.Parameters.AddWithValue("@Name", item.Name);
                updateTvCommand.Parameters.AddWithValue("@Description", item.Description);
                updateTvCommand.Parameters.AddWithValue("@Size", item.Size);
                updateTvCommand.Parameters.AddWithValue("@Resolution", item.Resolution);
                updateTvCommand.Parameters.AddWithValue("@Frequency", item.Frequency);
                updateTvCommand.Parameters.AddWithValue("@ReleasedYear", item.ReleasedYear);

                await updateTvCommand.ExecuteNonQueryAsync();

                updatedTVs.Add(item);
            }

            logger.LogInformation($"Updated {updatedTVs.Count} TVs with IDs: {string.Join(", ", updatedTVs.Select(x => x.ID))}");
            return updatedTVs;
        }

        protected override async Task DeleteMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<TV> items)
        {
            var deletedTvs = new List<int>();
            
            foreach (var item in items)
            {
                // Get StockItemId
                var getStockItemCommand = new SqlCommand("SELECT StockItemId FROM TVs WHERE ID = @Id", connection, transaction);
                getStockItemCommand.Parameters.AddWithValue("@Id", item.ID);
                var stockItemIdObj = await getStockItemCommand.ExecuteScalarAsync();

                if (stockItemIdObj == null)
                    continue;

                var stockItemId = (int)stockItemIdObj;

                // Delete TV
                var deleteTvCommand = new SqlCommand("DELETE FROM TVs WHERE ID = @Id", connection, transaction);
                deleteTvCommand.Parameters.AddWithValue("@Id", item.ID);
                await deleteTvCommand.ExecuteNonQueryAsync();

                // Delete StockItem
                var deleteStockItemCommand = new SqlCommand("DELETE FROM StockItems WHERE ID = @StockItemId", connection, transaction);
                deleteStockItemCommand.Parameters.AddWithValue("@StockItemId", stockItemId);
                await deleteStockItemCommand.ExecuteNonQueryAsync();
                
                deletedTvs.Add(item.ID);
            }
            
            logger.LogInformation($"Deleted {deletedTvs.Count} TVs with IDs: {string.Join(", ", deletedTvs)}");
        }
    }
}