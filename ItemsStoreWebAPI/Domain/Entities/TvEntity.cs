namespace ItemsStoreWebAPI.Domain.Entities
{
    public class TvEntity
    {
        public int Id { get; set; }
        public int StockItemId { get; set; }

        public float ScreenSize { get; set; }
        public string Resolution { get; set; } = string.Empty;
        public float Frequency { get; set; }
        
        public StockItemEntity StockItem { get; set; } = null!;
    }
}