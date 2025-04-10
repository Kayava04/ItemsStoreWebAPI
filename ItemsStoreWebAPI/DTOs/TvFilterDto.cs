namespace ItemsStoreWebAPI.DTOs
{
    public class TvFilterDto
    {
        public string? Name { get; set; }
        public int? MinSize { get; set; }
        public int? MaxSize { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? ReleasedYear { get; set; }
        public int? InStock { get; set; }
    }
}