namespace ItemsStoreWebAPI.Application.Models
{
    public class Mobile : StockItem
    {
        public string OS { get; set; }
        public float ScreenSize { get; set; }
        public int BatteryCapacity { get; set; }
        public int RAM { get; set; }
        public int Storage { get; set; }
    }
}