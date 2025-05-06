namespace ItemsStoreWebAPI.Entities
{
    public class StockItemEntity
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}