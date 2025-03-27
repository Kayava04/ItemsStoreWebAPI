namespace ItemsStoreWebAPI.Models
{
    public class TVFilter
    {
        public List<int>? Ids { get; set; }
        public decimal[]? Price { get; set; }
        public float[]? Size { get; set; }
        public float[]? Frequency { get; set; }
        public int[]? ReleasedYear { get; set; }
        public int[]? InStock { get; set; }
    }
}