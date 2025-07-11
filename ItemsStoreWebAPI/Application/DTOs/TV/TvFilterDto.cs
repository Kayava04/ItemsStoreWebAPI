using ItemsStoreWebAPI.Application.DTOs.Interfaces;

namespace ItemsStoreWebAPI.Application.DTOs.TV
{
    public class TvFilterDto : IFilterDto
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