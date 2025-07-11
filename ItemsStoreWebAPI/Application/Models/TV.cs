namespace ItemsStoreWebAPI.Application.Models
{
    public class TV : StockItem
    {
        public float ScreenSize { get; set; }
        public string Resolution { get; set; }
        public float Frequency { get; set; }
    }
}