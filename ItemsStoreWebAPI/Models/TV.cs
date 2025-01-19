namespace ItemsStoreWebAPI.Models
{
    public class TV : StockItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Size { get; set; }
        public string Resolution { get; set; }
        public float Frequency { get; set; }
        public int ReleasedYear { get; set; }
    }
}