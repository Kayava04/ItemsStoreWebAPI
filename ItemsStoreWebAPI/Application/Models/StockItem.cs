namespace ItemsStoreWebAPI.Application.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReleasedYear { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}