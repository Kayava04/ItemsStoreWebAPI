namespace ItemsStoreWebAPI.DTOs.Mobile
{
    public class RequestMobileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OS { get; set; }
        public float ScreenSize { get; set; }
        public int BatteryCapacity { get; set; }
        public int RAM { get; set; }
        public int Storage { get; set; }
        public int ReleasedYear { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}