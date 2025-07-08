namespace ItemsStoreWebAPI.Entities
{
    public class MobileEntity
    {
        public int Id { get; set; }
        public int StockItemId { get; set; }
        public string OS { get; set; } = string.Empty;
        public float ScreenSize { get; set; }
        public int BatteryCapacity { get; set; }
        public int RAM  { get; set; }
        public int Storage { get; set; }
        
        public StockItemEntity StockItem { get; set; } = null!;
    }
}