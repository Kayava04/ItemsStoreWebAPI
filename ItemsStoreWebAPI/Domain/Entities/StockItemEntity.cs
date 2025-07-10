namespace ItemsStoreWebAPI.Domain.Entities
{
    public class StockItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleasedYear { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}