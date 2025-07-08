namespace ItemsStoreWebAPI.DTOs.Mobile
{
    public class MobileFilterDto : IFilterDto
    {
        public string? Name { get; set; }
        public float? MinScreenSize { get; set; }
        public float? MaxScreenSize { get; set; }
        public int? MinBatteryCapacity { get; set; }
        public int? MaxBatteryCapacity { get; set; }
        public int? MinRAM { get; set; }
        public int? MaxRAM { get; set; }
        public int? MinStorage { get; set; }
        public int? MaxStorage { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? ReleasedYear { get; set; }
        public int? InStock { get; set; }
    }
}