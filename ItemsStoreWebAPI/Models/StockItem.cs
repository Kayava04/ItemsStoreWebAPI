namespace ItemsStoreWebAPI.Models
{
    public class StockItem
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; }
    }
}