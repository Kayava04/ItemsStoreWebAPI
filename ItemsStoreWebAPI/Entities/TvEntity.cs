namespace ItemsStoreWebAPI.Entities
{
    public class TvEntity
    {
        public Guid Id { get; set; }
        public Guid StockItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Size { get; set; }
        public string Resolution { get; set; } = string.Empty;
        public float Frequency { get; set; }
        public int ReleasedYear { get; set; }
        
        public StockItemEntity StockItem { get; set; } = null!;
    }
}